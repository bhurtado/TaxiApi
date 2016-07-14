using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Web.Http;
using TaxiApi.Enteties;
using TaxiApi.Models;

namespace TaxiApi.Controllers
{
    public class ReportController : ApiController
    {
        private TaxiDBEntities DBContext = new TaxiDBEntities();

        [Authorize]
        [HttpGet]
        [Route("api/report/getuserreports/{userId}")]
        public List<ReportModel> GetReports(int userId)
        {
            try
            {
                var list = new List<ReportModel>();

                foreach (var report in DBContext.Report)
                {
                    if (report.Employee_Id == userId)
                    {
                        list.Add(new ReportModel()
                        {
                            Id = report.Id,
                            Employee_ID = report.Employee_Id,
                            Vehicle_ID = report.Vehicle_Id,
                            ReportDate = report.ReportDate,

                            PrimaryFuel_ID = report.PrimaryFuel_ID,
                            PrimaryFuelAmount = report.PrimaryFuelAmount.Value,
                            PrimaryFuelUnitPrice = report.PrimaryFuelUnitPrice.Value,

                            SecondaryFuelAmount = report.SecondaryFuelAmount,
                            SecondaryFuelUnitPrice = report.SecondaryFuelUnitPrice,
                            SecondaryFuel_ID = report.SecondaryFuel_ID,

                            Distance = report.Distance.Value,

                            TotalPrice = report.TotalPrice.Value
                        });
                    }
                }
                return list;
            }
            catch (Exception e )
            {
                throw;
            }
        }
        [Authorize]
        [HttpGet]
        [Route("api/report/getuserreports/{userId}/{month}/{year}")]
        public List<ReportModel> GetReports(int userId, int month, int year)
        {
            var list = new List<ReportModel>();
            try
            {
                if (month == 13)
                {
                    foreach (var report in DBContext.Report)
                    {
                        if (report.Employee_Id == userId)
                        {
                            list.Add(new ReportModel()
                            {
                                Id = report.Id,
                                Employee_ID = report.Employee_Id,
                                Vehicle_ID = report.Vehicle_Id,
                                ReportDate = report.ReportDate,

                                PrimaryFuel_ID = report.PrimaryFuel_ID,
                                PrimaryFuelAmount = report.PrimaryFuelAmount.Value,
                                PrimaryFuelUnitPrice = report.PrimaryFuelUnitPrice.Value,

                                SecondaryFuelAmount = report.SecondaryFuelAmount,
                                SecondaryFuelUnitPrice = report.SecondaryFuelUnitPrice,
                                SecondaryFuel_ID = report.SecondaryFuel_ID,

                                Distance = report.Distance.Value,

                                TotalPrice = report.TotalPrice.Value
                            });
                        }
                    }
                }
                if (month == 0)
                {
                    foreach (var report in DBContext.Report)
                    {
                        if (report.Employee_Id == userId && report.ReportDate.Year == year)
                        {
                            list.Add(new ReportModel()
                            {
                                Id = report.Id,
                                Employee_ID = report.Employee_Id,
                                Vehicle_ID = report.Vehicle_Id,
                                ReportDate = report.ReportDate,

                                PrimaryFuel_ID = report.PrimaryFuel_ID,
                                PrimaryFuelAmount = report.PrimaryFuelAmount.Value,
                                PrimaryFuelUnitPrice = report.PrimaryFuelUnitPrice.Value,

                                SecondaryFuelAmount = report.SecondaryFuelAmount,
                                SecondaryFuelUnitPrice = report.SecondaryFuelUnitPrice,
                                SecondaryFuel_ID = report.SecondaryFuel_ID,

                                Distance = report.Distance.Value,

                                TotalPrice = report.TotalPrice.Value
                            });
                        }
                    }
                }

                if (month > 0)
                {
                    foreach (var report in DBContext.Report)
                    {
                        if (report.Employee_Id == userId && report.ReportDate.Year == year &&
                            report.ReportDate.Month == month)
                        {
                            list.Add(new ReportModel()
                            {
                                Id = report.Id,
                                Employee_ID = report.Employee_Id,
                                Vehicle_ID = report.Vehicle_Id,
                                ReportDate = report.ReportDate,

                                PrimaryFuel_ID = report.PrimaryFuel_ID,
                                PrimaryFuelAmount = report.PrimaryFuelAmount.Value,
                                PrimaryFuelUnitPrice = report.PrimaryFuelUnitPrice.Value,

                                SecondaryFuelAmount = report.SecondaryFuelAmount,
                                SecondaryFuelUnitPrice = report.SecondaryFuelUnitPrice,
                                SecondaryFuel_ID = report.SecondaryFuel_ID,

                                Distance = report.Distance.Value,

                                TotalPrice = report.TotalPrice.Value
                            });
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return list;
        }

        [Authorize]
        [HttpGet]
        [Route("api/report/getvehiclereports/{regnr}/{month}/{year}")]
        public List<ReportModel> GetReports(string regNr, int month, int year)
        {
            var list = new List<ReportModel>();


            if (month == 13)
            {
                foreach (var report in DBContext.Report)
                {
                    if (report.Vehicle_Id == regNr)
                    {
                        var newReport = new ReportModel()
                        {
                            Id = report.Id,
                            ReportDate = report.ReportDate,
                            Distance = report.Distance.Value,
                            Employee_ID = report.Employee_Id,
                            Vehicle_ID = report.Vehicle_Id,
                            PrimaryFuel_ID = report.PrimaryFuel_ID,
                            PrimaryFuelAmount = report.PrimaryFuelAmount.Value,
                            TotalPrice = report.TotalPrice.Value,
                            PrimaryFuelUnitPrice = report.PrimaryFuelUnitPrice.Value,
                        };

                        if (report.SecondaryFuel_ID != null)
                        {
                            newReport.SecondaryFuel_ID = report.SecondaryFuel_ID;
                            newReport.SecondaryFuelAmount = report.SecondaryFuelAmount;
                            newReport.SecondaryFuelUnitPrice = report.SecondaryFuelUnitPrice;
                        }
                        list.Add(newReport);
                    }
                }
                return list;
            }
            if (month == 0 && year > 0)
            {
                foreach (var report in DBContext.Report)
                {
                    if (report.Vehicle_Id == regNr && report.ReportDate.Year == year)
                    {
                        var newReport = new ReportModel()
                        {
                            Id = report.Id,
                            ReportDate = report.ReportDate,
                            Distance = report.Distance.Value,
                            Employee_ID = report.Employee_Id,
                            Vehicle_ID = report.Vehicle_Id,
                            PrimaryFuel_ID = report.PrimaryFuel_ID,
                            PrimaryFuelAmount = report.PrimaryFuelAmount.Value,
                            TotalPrice = report.TotalPrice.Value,
                            PrimaryFuelUnitPrice = report.PrimaryFuelUnitPrice.Value,
                        };

                        if (report.SecondaryFuel_ID != null)
                        {
                            newReport.SecondaryFuel_ID = report.SecondaryFuel_ID;
                            newReport.SecondaryFuelAmount = report.SecondaryFuelAmount;
                            newReport.SecondaryFuelUnitPrice = report.SecondaryFuelUnitPrice;
                        }
                        list.Add(newReport);
                    }
                }
                return list;
            }

            if (month > 0)
            {
                foreach (var report in DBContext.Report)
                {
                    if (report.Vehicle_Id == regNr && report.ReportDate.Year == year &&
                        report.ReportDate.Month == month)
                    {
                        var newReport = new ReportModel()
                        {
                            Id = report.Id,
                            ReportDate = report.ReportDate,
                            Distance = report.Distance.Value,
                            Employee_ID = report.Employee_Id,
                            Vehicle_ID = report.Vehicle_Id,
                            PrimaryFuel_ID = report.PrimaryFuel_ID,
                            PrimaryFuelAmount = report.PrimaryFuelAmount.Value,
                            TotalPrice = report.TotalPrice.Value,
                            PrimaryFuelUnitPrice = report.PrimaryFuelUnitPrice.Value,
                        };

                        if (report.SecondaryFuel_ID != null)
                        {
                            newReport.SecondaryFuel_ID = report.SecondaryFuel_ID;
                            newReport.SecondaryFuelAmount = report.SecondaryFuelAmount;
                            newReport.SecondaryFuelUnitPrice = report.SecondaryFuelUnitPrice;
                        }
                        list.Add(newReport);
                    }
                }
            }
            return list;
        }
        [Authorize]
        [HttpPost]
        [Route("api/report/addreport")]
        public IHttpActionResult AddReport(ReportModel report)
        {
            try
            {
                var newreport = new Report()
                {
                    Vehicle_Id = report.Vehicle_ID,
                    Employee_Id = report.Employee_ID,

                    ReportDate = report.ReportDate,

                    PrimaryFuel_ID = report.PrimaryFuel_ID,
                    PrimaryFuelAmount = report.PrimaryFuelAmount,
                    PrimaryFuelUnitPrice = report.PrimaryFuelUnitPrice,

                    Distance = report.Distance,

                    TotalPrice = report.TotalPrice,

                    SecondaryFuel_ID = report.SecondaryFuel_ID,
                    SecondaryFuelAmount = report.SecondaryFuelAmount,
                    SecondaryFuelUnitPrice = report.SecondaryFuelUnitPrice,

                    Employee = DBContext.Employee.FirstOrDefault(x => x.Id == report.Employee_ID),
                    Vehicle = DBContext.Vehicle.FirstOrDefault(x => x.RegNr == report.Vehicle_ID)
                };

                var list = DBContext.VehicleFuel.Where(vehicleFuel => vehicleFuel.Vehicle_Id == report.Vehicle_ID).ToList();



                foreach (var fuel in list)
                {
                    if (fuel.Fuel_Id == newreport.PrimaryFuel_ID)
                    {
                        newreport.VehicleFuel = fuel;
                    }
                    if (fuel.Fuel_Id == newreport.SecondaryFuel_ID)
                    {
                        newreport.VehicleFuel1 = fuel;
                    }
                }
               
                DBContext.Report.Add(newreport);
                DBContext.SaveChanges();
                return Ok();

            }
            catch (Exception e)
            {
                return BadRequest();
                throw;
            }

        }

       


        [Authorize]
        [HttpPut]
        [Route("api/report/updatereport")]
        public IHttpActionResult UpdateReport(ReportModel report)
        {
            int? a = report.Distance;

            try
            {


                var original = DBContext.Report.Find(report.Id);

                var updatedOriginal = new Report();

                updatedOriginal = original;
                updatedOriginal.ReportDate = report.ReportDate;
                updatedOriginal.PrimaryFuelUnitPrice = report.PrimaryFuelUnitPrice;
                updatedOriginal.PrimaryFuelAmount = report.PrimaryFuelAmount;
                updatedOriginal.TotalPrice = report.TotalPrice;


                var pointAfterOriginal =
                    DBContext.Report.Where(p => p.ReportDate > updatedOriginal.ReportDate)
                        .OrderBy(p => p.ReportDate)
                        .FirstOrDefault();
                if (pointAfterOriginal != null)
                {
                    var b = pointAfterOriginal.Distance;
                    var updatedPointAfterOriginal = pointAfterOriginal;

                    //Update distance on selected report //
                    //a = New distance
                    //b = distance on the report registered after updated report (Date)
                    b = b - a;
                    a = a + b;
                    ///////////////////////////////////////

                    updatedOriginal.Distance = a;
                    updatedPointAfterOriginal.Distance = b;

                    DBContext.Entry(pointAfterOriginal).CurrentValues.SetValues(updatedPointAfterOriginal);

                }
                if (report.SecondaryFuel_ID != null)
                {
                    updatedOriginal.SecondaryFuelAmount = report.SecondaryFuelAmount;
                    updatedOriginal.SecondaryFuelUnitPrice = report.SecondaryFuelUnitPrice;
                }
                else if (report.Distance != 0)
                {
                    updatedOriginal.Distance = original.Distance + a;
                }

                if (original != null)
                {

                    DBContext.Entry(original).CurrentValues.SetValues(updatedOriginal);
                    DBContext.SaveChanges();
                    return Ok();

                }
                return BadRequest();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


    }
}

