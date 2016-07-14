using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiClient.Models
{
    public class ReportModel
    {
        public int Id { get; set; }
        public int Employee_ID { get; set; }
        public int PrimaryFuel_ID { get; set; }
        public int Distance { get; set; }
        public string Vehicle_ID { get; set; }
        public DateTime ReportDate { get; set; }
        public decimal PrimaryFuelAmount { get; set; }
        public decimal PrimaryFuelUnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal? SecondaryFuelAmount { get; set; }
        public int? SecondaryFuel_ID { get; set; }
        public decimal? SecondaryFuelUnitPrice { get; set; }

    }
}
