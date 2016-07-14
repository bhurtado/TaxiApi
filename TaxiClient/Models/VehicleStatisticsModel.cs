using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiClient.Models
{
    class VehicleStatisticsModel
    {
        public int Distance { get; set; }
        public decimal AvarageDistance { get; set; }
        public decimal AvarageCost { get; set; }
        public decimal TotalCost { get; set; }
        public decimal AvaragePrimaryFuelCostTenKm { get; set; }
        public decimal AvaragePrimaryFuelAmount { get; set; }
        public decimal AvarageSecondaryFuelAmount { get; set; }
        public decimal AvaragePrimaryFuelCost { get; set; }
        public decimal AvarageSecondaryFuelCost { get; set; }
        public decimal AvarageSecondaryFuelCostTenKm { get; set; }
    }
}
