using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaxiApi.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public bool IsAdmin { get; set; }
    }
}