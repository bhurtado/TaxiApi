using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiClient.Models;

namespace TaxiClient.Helpers
{
    class CountStatistics
    {
        public static StatisticsModel Count(List<ReportModel> reports)
        {
            var statistics = new StatisticsModel();
            
            foreach (var report in reports)
            {
                statistics.TotalCost += report.TotalPrice;
                statistics.Distance += report.Distance;
                
                if (report.SecondaryFuel_ID != null)
                {
                    statistics.TotalCost += report.SecondaryFuelUnitPrice.Value;
                }
            }
            if (reports.Count > 0)
            {
                if(statistics.AvarageCost > 0)
                statistics.AvarageCost = statistics.TotalCost/reports.Count;

                if(statistics.AvarageDistance > 0)
                statistics.AvarageDistance = statistics.Distance/reports.Count;

                if(statistics.Distance > 0)
                statistics.AvarageFuelTenKm = statistics.TotalCost/statistics.Distance*10;
            }

            return statistics;
        }

        public static VehicleStatisticsModel VehicleStatistics(List<ReportModel> reports)
        {
            var statistics = new VehicleStatisticsModel();
            if (reports.Count > 0)
            {
                
            
            foreach (var report in reports)
            {
                statistics.Distance += report.Distance;
                statistics.TotalCost += report.TotalPrice;
                statistics.AvaragePrimaryFuelAmount += report.PrimaryFuelAmount;
                statistics.AvaragePrimaryFuelCost += report.PrimaryFuelUnitPrice*report.PrimaryFuelAmount;

                if (report.SecondaryFuel_ID != null)
                {
                    statistics.AvarageSecondaryFuelAmount += report.SecondaryFuelAmount.Value;
                    statistics.AvarageSecondaryFuelCost += report.SecondaryFuelUnitPrice.Value*report.SecondaryFuelAmount.Value;
                }
            }
            statistics.AvarageDistance = Math.Round((decimal) statistics.Distance / reports.Count,2);
            statistics.AvaragePrimaryFuelCostTenKm = Math.Round(statistics.AvaragePrimaryFuelCost / statistics.Distance * 10,2);
            statistics.AvaragePrimaryFuelAmount = Math.Round(statistics.AvaragePrimaryFuelAmount/reports.Count,2);
            statistics.AvaragePrimaryFuelCost = Math.Round(statistics.AvaragePrimaryFuelCost/reports.Count,2);
            statistics.AvarageCost = Math.Round(statistics.TotalCost/reports.Count,2);
            

            if (statistics.AvarageSecondaryFuelAmount != 0)
            {
                statistics.AvarageSecondaryFuelCostTenKm = Math.Round(statistics.AvarageSecondaryFuelAmount/statistics.Distance*10,2);
                statistics.AvarageSecondaryFuelAmount = Math.Round(statistics.AvarageSecondaryFuelAmount/reports.Count,2);
                statistics.AvarageSecondaryFuelCost = Math.Round(statistics.AvarageSecondaryFuelCost/reports.Count,2);
            }
            }

            return statistics;
        }
        
    }
}
