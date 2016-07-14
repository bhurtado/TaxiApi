using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TaxiApi.Models
{
    public class LoginResponse
    {
        public HttpResponseMessage ResponseMessage { get; set; }
        public bool CheckUser { get; set; }
    }
}
