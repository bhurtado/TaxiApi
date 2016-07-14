using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using TaxiApi.Models;

namespace TaxiApi.Helpers
{
    public static class PasswordHashHelper
    {
        public static string PasswordHasher(HashModel hash)
        {
            var cArray = hash.Salt.ToCharArray();
            Array.Reverse(cArray);

            string saltAndPassword = String.Concat(hash.Password, new string(cArray));
            string hashedPwd = FormsAuthentication.HashPasswordForStoringInConfigFile(saltAndPassword, "sha1");

            return hashedPwd;
        }

        public static string CreateSalt(string toSalt)
        {
            var cArray = toSalt.ToCharArray();
            var saltArray = new char[3];
            var c = 0;
            for (int i = 0; i < cArray.Length; i++)
            {
                if (i % 2 == 0)
                {
                    saltArray[c] = cArray[i];
                    c++;
                    
                }
            }

            var salt = new string(saltArray);

            var finishedSalt = FormsAuthentication.HashPasswordForStoringInConfigFile(salt, "sha1");
            return finishedSalt;
        }
       
    }
}