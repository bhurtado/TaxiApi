using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaxiClient.Models
{
    public class VehicleModel
    {
        public string RegNr { get; set; }
        public int Manufacturer_ID { get; set; }
        public int Color_ID { get; set; }
        //public int Fuel { get; set; }
        public int Model_ID { get; set; }
        public int YearModel { get; set; }
        public int TripMeter { get; set; }
        public int PrimaryFuel_ID { get; set; }
        public int? SecondaryFuel_ID { get; set; }
        
    }
}