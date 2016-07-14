using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaxiApi.Models
{
    public class EmployeeModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Salt { get; set; }
        public bool IsAdmin { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}