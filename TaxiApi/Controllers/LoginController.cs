using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.ModelBinding;
using TaxiApi.Enteties;
using TaxiApi.Helpers;
using TaxiApi.Models;

namespace TaxiApi.Controllers
{
    public class LoginController : ApiController
    {
        private TaxiDBEntities DBContext = new TaxiDBEntities();
        
        [HttpPost]
        [Route("api/login/login")]
        public HttpResponseMessage Login(LoginModel loginModel)
        {
            try
            {
               var user = DBContext.Employee.FirstOrDefault(x => x.UserName == loginModel.Username);

                if (user != null)
                {
                    var hash = new HashModel() { Password = loginModel.Password , Salt = user.Salt};
                    var hashedPassword = PasswordHashHelper.PasswordHasher(hash);
                    if (hashedPassword == user.Pword)
                    {
                        return new HttpResponseMessage(HttpStatusCode.OK);
                    }

                }
            }
            catch (Exception)
            {
                
                throw;
            }

            return new HttpResponseMessage(HttpStatusCode.Forbidden);
        }


        [HttpGet]
        [Route("api/login/getinfo/{username}")]
        public UserModel GetUserInfo(string username)
        {
            var confirmeduser = DBContext.Employee.FirstOrDefault(x => x.UserName == username);
            if (confirmeduser != null)
            {
                var user = new UserModel() {Id = confirmeduser.Id,Firstname = confirmeduser.FirstName, Lastname = confirmeduser.LastName, IsAdmin = confirmeduser.Admin && confirmeduser.Admin};
                return user;
            }
            return null;
        }
    }
}
