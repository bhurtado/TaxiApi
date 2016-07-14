using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiClient.Models
{
    class StatisticsModel
    {
        public int Distance { get; set; }
        public decimal AvarageDistance { get; set; }
        public decimal AvarageCost { get; set; }
        public decimal TotalCost { get; set; }
        public decimal AvarageFuelTenKm { get; set; }
    }
}
