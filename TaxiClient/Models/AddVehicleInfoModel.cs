using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiClient.Models
{
    class AddVehicleInfoModel
    {
        public List<ColorModel> Colors { get; set; }
        public List<ModelsModel> Models { get; set; }
        public List<FuelModel> Fuels { get; set; }
        public List<ManufacturerModel> Manufacturers { get; set; }
    }
}
