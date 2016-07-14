using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TaxiApi.Models;

namespace TaxiApi.Helpers
{
    public static class CountStatistics
    {
        public static DrivenVehicleStatistics FixStatistics(DrivenVehicleStatistics fixReports)
        {

            int trip = 0;
            decimal primaryfuelamount = 0;
            decimal primaryfuelunitprice = 0;
            decimal secondaryfuelamount = 0;
            decimal secondaryfuelunitPrice = 0;
            int reports = 0;

            foreach (var report in fixReports.Reports)
            {
                primaryfuelunitprice += report.PrimaryFuelUnitPrice;
                primaryfuelamount += report.PrimaryFuelAmount;

                if (report.SecondaryFuel_ID != null)
                {
                    secondaryfuelunitPrice += report.SecondaryFuelUnitPrice.Value;
                    secondaryfuelamount += report.SecondaryFuelAmount.Value;
                }
                trip += report.Distance;
                reports++;
            }



            fixReports.PrimaryFuelAmountBeginning = Math.Round(primaryfuelamount/reports, 2);
            fixReports.PrimaryFuelUnitBeginning = Math.Round(primaryfuelunitprice/reports, 2);
            


            if (secondaryfuelamount > 0)
            {

                fixReports.SecondFuelAmountBeginning = Math.Round(secondaryfuelamount / reports, 2);
                fixReports.SecondFuelUnitBeginning = Math.Round(secondaryfuelunitPrice / reports, 2);
            }
            return fixReports;
          


        }
    }
}