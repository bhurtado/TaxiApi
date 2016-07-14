using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media;
using TaxiClient.Models;

namespace TaxiClient.Helpers
{
    class Validators
    {
        public static bool ValidateRegNr(string regNr)
        {
            if(!string.IsNullOrWhiteSpace(regNr))
            return Regex.Match(regNr.ToLower(), ("^[a-z]{3}[0-9]{3}$")).Success;
            return false;
        }

        public static bool ValidateName(string firstName)
        {
            if (!string.IsNullOrWhiteSpace(firstName))
                return Regex.Match(firstName.ToLower(), ("^[a-ö]{2,49}")).Success;
            return false;
        }

        public static bool ValidateYear(int? year)
        {
            if(year != null)
            return Regex.Match(year.ToString(), ("^[1-2]{1}[0-9]{3}$")).Success;
            return false;

        }

        public static bool ValidateTripMeter(int? tripmeter)
        {
            if (tripmeter != null)
            return Regex.Match(tripmeter.ToString(), ("^[0-9]{0,9}$")).Success;
            return false;
        }

        public static bool ValidateColor(ColorModel color)
        {
            return color != null;
        }

        public static bool ValidateFuel(FuelModel fuel)
        {
            return fuel != null;
        }

        public static bool ValidateModel(ModelsModel model)
        {
            return model != null;
        }

        public static bool ValidateManufacturer(ManufacturerModel manufacturer)
        {
            return manufacturer != null;
        }

        public static bool ValidateUserNameLength(string userName)
        {
            return userName?.Length == 6;
        }

        public static bool ValidateTextBox(string text)
        {
            if(!string.IsNullOrEmpty(text))
            return Regex.Match(text.ToLower(), ("^[a-z]{2,20}$")).Success;
            return false;
        }

        public static bool ValidateHybrid(bool checkbox, FuelModel hybrid, FuelModel fuel)
        {
            if (checkbox)
            {
                if (hybrid != fuel)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }


            return true;
        }

        public static bool ValidateTripmeterValue(int startTrip, int stopTrip)
        {
            return startTrip < stopTrip;
        }

        public static bool ValidateDecimal(string checkDecimal)
        {
            if (!string.IsNullOrEmpty(checkDecimal))
                return true;
            return false;
        }
    }
}
