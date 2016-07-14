using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaxiClient.Models
{
    public class DrivenVehicleStatistics
    {
        public decimal PrimaryFuelAmountBeginning { get; set; }
        public decimal PrimaryFuelUnitBeginning { get; set; }
        public decimal SecondFuelAmountBeginning { get; set; }
        public decimal SecondFuelUnitBeginning { get; set; }

        public List<ReportModel> Reports { get; set; } 
    }
}