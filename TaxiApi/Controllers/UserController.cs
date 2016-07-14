using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using TaxiApi.Enteties;
using TaxiApi.Helpers;
using TaxiApi.Models;

namespace TaxiApi.Controllers
{
    public class UserController : ApiController
    {
        private TaxiDBEntities DBContext = new TaxiDBEntities();

        [Authorize]
        [HttpGet]
        [Route("api/user/getaccesslevel")]
        public UserModel GetAccessLevel()
        {
            var username = ClaimsPrincipal.Current.Identity.Name;
            var user = DBContext.Employee.FirstOrDefault(x => x.UserName == username);
            return new UserModel() {Id = user.Id , Firstname = user.FirstName, Lastname = user.LastName, IsAdmin = user.Admin};
        }

        [Authorize]
        [HttpPost]
        [Route("api/user/getuser/{username}")]
        public EmployeeModel GetUser(LoginModel userName)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            return new EmployeeModel();
        }

        [Authorize]
        [HttpGet]
        [Route("api/user/getusers")]
        public List<UserModel> GetUsers()
        {
            var list = new List<UserModel>();

            foreach (var employee in DBContext.Employee)
            {
                list.Add(new UserModel()
                {
                    Id = employee.Id,
                    Firstname = employee.FirstName,
                    Lastname = employee.LastName,
                    IsAdmin = employee.Admin
                });
            }
            return list;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("api/user/adduser")]
        public IHttpActionResult AddUser(EmployeeModel employee)
        {

            var findUser = DBContext.Employee.FirstOrDefault(x => x.UserName == employee.UserName);
            if (findUser != null)
            {
                
            }
            var newEmployee = new Employee();
            newEmployee.Admin = employee.IsAdmin;
            newEmployee.UserName = employee.UserName;
            newEmployee.FirstName = employee.FirstName;
            newEmployee.LastName = employee.LastName;
            newEmployee.Salt = PasswordHashHelper.CreateSalt(employee.UserName);
            newEmployee.Pword = PasswordHashHelper.PasswordHasher(new HashModel() { Password = employee.Password, Salt = newEmployee.Salt });

            try
            {
                DBContext.Employee.Add(newEmployee);
                DBContext.SaveChanges();
                return Ok();

            }
            catch (Exception)
            {
                return BadRequest();
                throw;
            }

        }

    }
}
