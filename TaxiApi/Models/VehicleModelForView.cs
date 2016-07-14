using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaxiApi.Models
{
    public class VehicleModelForView
    {
        public string RegNr { get; set; }
        public string Color { get; set; }
        public string Manufacturer { get; set; }
        public string PrimaryFuel { get; set; }
        public string SecondaryFuel { get; set; }
        public string Model { get; set; }
        public int Tripmeter { get; set; }
        public int Yearmodel { get; set; }
    }
}