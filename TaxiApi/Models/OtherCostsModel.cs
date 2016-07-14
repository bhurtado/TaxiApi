using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaxiApi.Models
{
    public class OtherCostsModel
    {
        public int? Id { get; set; }
        public string MaintenanceType { get; set; }
        public decimal Cost { get; set; }
        public string Vehicle_Id { get; set; }
        public DateTime ReportDate { get; set; }
    }
}