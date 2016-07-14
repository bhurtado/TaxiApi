using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using TaxiApi.Enteties;
using TaxiApi.Models;

namespace TaxiApi.Controllers
{
    public class DriverController : ApiController
    {
        private readonly TaxiDBEntities DBContext = new TaxiDBEntities();
        [Authorize]
        [HttpGet]
        [Route("api/driver/getvehicle/{userId}")]
        public IHttpActionResult GetVehicle(int userId)
        {
            var list = new List<VehicleModel>();

            foreach (var report in DBContext.Report)
            {
                if (report.Employee_Id == userId)
                {

                    foreach (var vehicle in DBContext.Vehicle)
                    {
                        if (vehicle.RegNr == report.Vehicle_Id)
                        {
                            var usedVehicle = new VehicleModel();

                            usedVehicle.RegNr = vehicle.RegNr;
                            usedVehicle.Color_ID = vehicle.Color_Id.Value;
                            usedVehicle.Manufacturer_ID = vehicle.Manufact_Id;
                            usedVehicle.Model_ID = vehicle.Model_Id;
                            usedVehicle.TripMeter = vehicle.TripMeter;
                            usedVehicle.YearModel = vehicle.YearModel.Value;

                            foreach (var vehicleFuel in DBContext.VehicleFuel)
                            {
                                if (vehicleFuel.Vehicle_Id == vehicle.RegNr)
                                {

                                    if (vehicleFuel.Id == report.PrimaryFuel_ID)
                                    {
                                        usedVehicle.PrimaryFuel_ID = vehicleFuel.Fuel_Id;
                                    }
                                    if (vehicleFuel.Id == report.SecondaryFuel_ID)
                                    {
                                        usedVehicle.SecondaryFuel_ID = vehicleFuel.Fuel_Id;
                                    }
                                }

                            }
                            list.Add(usedVehicle);
                        }

                    }
                }
            }
            var distinctList = list.GroupBy(x => x.RegNr).Select(x => x.First());
            return Ok(distinctList.ToList());

        }
        [Authorize]
        [HttpGet]
        [Route("api/driver/getreports/{regnr}/{userid}")]
        public List<ReportModel> GetReports(string regnr, int userid)
        {
            var list = new List<ReportModel>();

            foreach (var report in DBContext.Report)
            {
                if (report.Vehicle_Id == regnr && report.Employee_Id == userid)
                {
                    var newReport = new ReportModel()
                    {
                        Id = report.Id,
                        PrimaryFuel_ID = report.PrimaryFuel_ID,
                        PrimaryFuelAmount =(decimal) report.PrimaryFuelAmount.Value,
                        SecondaryFuelAmount = report.SecondaryFuelAmount,
                        SecondaryFuelUnitPrice = report.SecondaryFuelUnitPrice,
                        SecondaryFuel_ID = report.SecondaryFuel_ID,
                        PrimaryFuelUnitPrice =(decimal) report.PrimaryFuelUnitPrice,
                        TotalPrice = (decimal) report.TotalPrice,
                        Distance = (int) report.Distance,
                        Vehicle_ID = report.Vehicle_Id,
                        Employee_ID = report.Employee_Id,
                        ReportDate = report.ReportDate
                    };
                    list.Add(newReport);
                }
            }
            
            return list;
        }
    }
}
