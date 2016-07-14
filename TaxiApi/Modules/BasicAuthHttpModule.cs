using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using TaxiApi.Enteties;
using TaxiApi.Helpers;
using TaxiApi.Models;

namespace TaxiApi.Modules
{
    public class BasicAuthHttpModule : IHttpModule
    {
        private static TaxiDBEntities DBContext = new TaxiDBEntities();
        private const string Realm = "TaxiApi";

        public void Init(HttpApplication context)
        {
            // Register event handlers
            context.AuthenticateRequest += OnApplicationAuthenticateRequest;
            context.EndRequest += OnApplicationEndRequest;
        }
        
        private static void SetPrincipal(IPrincipal principal)
        {
            Thread.CurrentPrincipal = principal;
            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = principal;
            }
        }

        /*// TODO: Here is where you would validate the username and password.
        private static bool CheckPassword(string username, string password)
        {
            return username == "user" && password == "password";
        }*/

        private static void AuthenticateUser(string credentials)
        {
            try
            {
                //var encoding = Encoding.GetEncoding("iso-8859-1");
                var encoding = Encoding.UTF8;
                credentials = encoding.GetString(Convert.FromBase64String(credentials));

                int separator = credentials.IndexOf(':');
                string name = credentials.Substring(0, separator);
                string password = credentials.Substring(separator + 1);

                /*if (CheckPassword(name, password))
                {
                    var identity = new GenericIdentity(name);
                    SetPrincipal(new GenericPrincipal(identity, null));
                }
                else
                {
                    // Invalid username or password.
                    HttpContext.Current.Response.StatusCode = 401;
                }*/

                // name och password är variablerna du ska jobba med här
                var user = DBContext.Employee.FirstOrDefault(x => x.UserName == name);

                if (user != null &&
                    user.Pword ==
                    PasswordHashHelper.PasswordHasher(new HashModel() {Password = password, Salt = user.Salt}))
                {
                    var identity = new GenericIdentity(name);
                    if(user.Admin)
                        SetPrincipal(new GenericPrincipal(identity, new[] { "Admin" }));
                    else
                    {
                        SetPrincipal(new GenericPrincipal(identity, new[] { "Driver" }));
                    }
                        
                }
                else
                {
                    // Invalid username or password.
                    HttpContext.Current.Response.StatusCode = 401;
                }
            }

            catch(FormatException)
            {
                // Credentials were not formatted correctly.
                HttpContext.Current.Response.StatusCode = 401;
            }
        }

        private static void OnApplicationAuthenticateRequest(object sender, EventArgs e)
        {
            var request = HttpContext.Current.Request;
            var authHeader = request.Headers["Authorization"];
            if (authHeader != null)
            {
                var authHeaderVal = AuthenticationHeaderValue.Parse(authHeader);

                // RFC 2617 sec 1.2, "scheme" name is case-insensitive
                if (authHeaderVal.Scheme.Equals("basic",
                        StringComparison.OrdinalIgnoreCase) &&
                    authHeaderVal.Parameter != null)
                {
                    AuthenticateUser(authHeaderVal.Parameter);
                }
            }
        }

        // If the request was unauthorized, add the WWW-Authenticate header 
        // to the response.
        private static void OnApplicationEndRequest(object sender, EventArgs e)
        {
            var response = HttpContext.Current.Response;
            if (response.StatusCode == 401)
            {
                response.Headers.Add("WWW-Authenticate",
                    string.Format("Basic realm=\"{0}\"", Realm));
            }
        }

        public void Dispose()
        {
        }
    }
}
