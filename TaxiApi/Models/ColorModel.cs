using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaxiApi.Models
{
    public class ColorModel
    {
        public int Id { get; set; }
        public string ColorName { get; set; }
        public bool Metallic { get; set; }
    }
}