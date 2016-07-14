using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiClient.Models
{
    class OtherCostsModel
    {
        public int? Id { get; set; }
        public string MaintenanceType { get; set; }
        public decimal Cost { get; set; }
        public string Vehicle_Id { get; set; }
        public DateTime ReportDate { get; set; }
    }
}
