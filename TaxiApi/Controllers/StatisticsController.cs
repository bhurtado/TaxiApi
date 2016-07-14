using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using TaxiApi.Enteties;
using TaxiApi.Helpers;
using TaxiApi.Models;

namespace TaxiApi.Controllers
{
    public class StatisticsController : ApiController
    {
        private readonly TaxiDBEntities DBContext = new TaxiDBEntities();

        [Authorize]
        [HttpGet]
        [Route("api/statistics/yearreports/{userId}/{regnr}")]
        public List<int> YearReports(int userId, string regnr)
        {
            var yearList = new List<int>();

            try
            {
                foreach (var report in DBContext.Report)
                {
                    if (report.Employee_Id == userId && report.Vehicle_Id == regnr)
                    {
                        yearList.Add(report.ReportDate.Year);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            var list = yearList.Distinct().ToList();
            list.OrderByDescending(i => i);

            return list;
        }

        [Authorize]
        [HttpGet]
        [Route("api/statistics/getyears/{regnr}")]
        public List<int> YearReports(string regnr)
        {
            var yearList = new List<int>();

            try
            {
                foreach (var report in DBContext.Report)
                {
                    if (report.Vehicle_Id == regnr)
                    {
                        yearList.Add(report.ReportDate.Year);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            var list = yearList.Distinct().ToList();
            list.OrderByDescending(i => i);

            return list;
        }

        [Authorize]
        [HttpGet]
        [Route("api/statistics/yearreports/{userId}")]
        public List<int> YearReports(int userId)
        {
            var yearList = new List<int>();

            try
            {
                foreach (var report in DBContext.Report)
                {
                    if (report.Employee_Id == userId)
                    {
                        yearList.Add(report.ReportDate.Year);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            var list = yearList.Distinct().ToList();
            list.OrderByDescending(i => i);

            return list;
        }

        [Authorize]
        [HttpGet]
        [Route("api/statistics/driveryearmonthstatistics/{regnr}/{userid}/{year}/{month}")]
        public DrivenVehicleStatistics DriverYearMonthStatisics(string regnr, int userid, int year, int month)
        {
            var statistics = new DrivenVehicleStatistics();
            statistics.Reports = new List<ReportModel>();

            try
            {
                if (month == 13)
                {
                    foreach (var report in DBContext.Report)
                    {
                        if (report.Employee_Id == userid && report.Vehicle_Id == regnr)
                        {
                            var newReport = new ReportModel()
                            {
                                Distance = report.Distance.Value,
                                Employee_ID = report.Employee_Id,
                                ReportDate = report.ReportDate,
                                TotalPrice = report.TotalPrice.Value,
                                Vehicle_ID = report.Vehicle_Id,
                                PrimaryFuel_ID = report.PrimaryFuel_ID,
                                PrimaryFuelAmount = report.PrimaryFuelAmount.Value,
                                PrimaryFuelUnitPrice = report.PrimaryFuelUnitPrice.Value
                            };
                            if (report.SecondaryFuel_ID != null)
                            {
                                newReport.SecondaryFuel_ID = report.SecondaryFuel_ID;
                                newReport.SecondaryFuelAmount = report.SecondaryFuelAmount;
                                newReport.SecondaryFuelUnitPrice = report.SecondaryFuelUnitPrice;
                            }
                            statistics.Reports.Add(newReport);
                        }
                    }
                    if (statistics.Reports.Count > 0)
                        return CountStatistics.FixStatistics(statistics);
                }

                if (month == 0)
                {
                    foreach (var report in DBContext.Report)
                    {
                        if (report.Employee_Id == userid && report.ReportDate.Year == year && report.Vehicle_Id == regnr)
                        {
                            var newReport = new ReportModel()
                            {
                                Distance = report.Distance.Value,
                                Employee_ID = report.Employee_Id,
                                ReportDate = report.ReportDate,
                                TotalPrice = report.TotalPrice.Value,
                                Vehicle_ID = report.Vehicle_Id,
                                PrimaryFuel_ID = report.PrimaryFuel_ID,
                                PrimaryFuelAmount = report.PrimaryFuelAmount.Value,
                                PrimaryFuelUnitPrice = report.PrimaryFuelUnitPrice.Value
                            };
                            if (report.SecondaryFuel_ID != null)
                            {
                                newReport.SecondaryFuel_ID = report.SecondaryFuel_ID;
                                newReport.SecondaryFuelAmount = report.SecondaryFuelAmount;
                                newReport.SecondaryFuelUnitPrice = report.SecondaryFuelUnitPrice;
                            }
                            statistics.Reports.Add(newReport);
                        }
                    }
                    if (statistics.Reports.Count > 0)
                        return CountStatistics.FixStatistics(statistics);

                }
                if (month > 0)
                {
                    foreach (var report in DBContext.Report)
                    {
                        if (report.Vehicle_Id == regnr && report.Employee_Id == userid && report.ReportDate.Year == year && report.ReportDate.Month == month)
                        {
                            var newReport = new ReportModel()
                            {
                                Distance = report.Distance.Value,
                                Employee_ID = report.Employee_Id,
                                ReportDate = report.ReportDate,
                                TotalPrice = report.TotalPrice.Value,
                                Vehicle_ID = report.Vehicle_Id,
                                PrimaryFuel_ID = report.PrimaryFuel_ID,
                                PrimaryFuelAmount = report.PrimaryFuelAmount.Value,
                                PrimaryFuelUnitPrice = report.PrimaryFuelUnitPrice.Value
                            };
                            if (report.SecondaryFuel_ID != null)
                            {
                                newReport.SecondaryFuel_ID = report.SecondaryFuel_ID;
                                newReport.SecondaryFuelAmount = report.SecondaryFuelAmount;
                                newReport.SecondaryFuelUnitPrice = report.SecondaryFuelUnitPrice;
                            }
                            statistics.Reports.Add(newReport);
                        }
                    }

                    if (statistics.Reports.Count > 0)
                        return CountStatistics.FixStatistics(statistics);
                }

            }
            catch (Exception)
            {

                throw;
            }

            return new DrivenVehicleStatistics();
        }

        [Authorize]
        [HttpGet]
        [Route("api/statistics/getDriverVehicleAvarage/{regnr}/{userid}")]
        public DrivenVehicleStatistics DrivenVehicleStatistic(string regnr, int userid)
        {


            var statistics = new DrivenVehicleStatistics();
            statistics.Reports = new List<ReportModel>();

            foreach (var information in DBContext.Report)
            {
                if (information.Employee_Id == userid && regnr == information.Vehicle_Id)
                {

                    var yearReport = new ReportModel()
                    {


                        Distance = information.Distance.Value,
                        Employee_ID = information.Employee_Id,
                        ReportDate = information.ReportDate,
                        TotalPrice = information.TotalPrice.Value,
                        Vehicle_ID = information.Vehicle_Id,
                        PrimaryFuel_ID = information.PrimaryFuel_ID,
                        PrimaryFuelAmount = information.PrimaryFuelAmount.Value,
                        PrimaryFuelUnitPrice = information.PrimaryFuelUnitPrice.Value
                    };


                    if (information.SecondaryFuel_ID != null)
                    {
                        yearReport.SecondaryFuel_ID = information.SecondaryFuel_ID;
                        yearReport.SecondaryFuelAmount = information.SecondaryFuelAmount;
                        yearReport.SecondaryFuelUnitPrice = information.SecondaryFuelUnitPrice;
                    }
                    statistics.Reports.Add(yearReport);
                }
            }
            return CountStatistics.FixStatistics(statistics);
        }

        [Authorize]
        [HttpGet]
        [Route("api/statistics/getstatisticsfromstart/{userid}")]
        public DrivenVehicleStatistics FromStartStatistics(int userid)
        {
            var statistics = new DrivenVehicleStatistics();
            statistics.Reports = new List<ReportModel>();
            foreach (var report in DBContext.Report)
            {
                if (report.Employee_Id == userid)
                {
                    var newReport = new ReportModel()
                    {
                        Distance = report.Distance.Value,
                        Employee_ID = report.Employee_Id,
                        ReportDate = report.ReportDate,
                        TotalPrice = report.TotalPrice.Value,
                        Vehicle_ID = report.Vehicle_Id,
                        PrimaryFuel_ID = report.PrimaryFuel_ID,
                        PrimaryFuelAmount = report.PrimaryFuelAmount.Value,
                        PrimaryFuelUnitPrice = report.PrimaryFuelUnitPrice.Value
                    };
                    if (report.SecondaryFuel_ID != null)
                    {
                        newReport.SecondaryFuel_ID = report.SecondaryFuel_ID;
                        newReport.SecondaryFuelAmount = report.SecondaryFuelAmount;
                        newReport.SecondaryFuelUnitPrice = report.SecondaryFuelUnitPrice;
                    }
                    statistics.Reports.Add(newReport);
                }
            }
            return CountStatistics.FixStatistics(statistics);
        }
    }
}
