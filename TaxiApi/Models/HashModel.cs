using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaxiApi.Models
{
    public class HashModel
    {
        public string Salt { get; set; }
        public string Password { get; set; }
    }
}