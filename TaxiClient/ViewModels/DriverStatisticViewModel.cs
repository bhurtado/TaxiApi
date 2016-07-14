using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using TaxiClient.Models;
using TaxiClient.Annotations;
using ReportModel = TaxiClient.Models.ReportModel;
using UserModel = TaxiClient.Models.UserModel;
using UserVehicleReportModel = TaxiClient.Models.UserVehicleReportModel;
using VehicleModel = TaxiClient.Models.VehicleModel;

namespace TaxiClient.ViewModels
{
    public enum Months {Alltime = 13 , Year = 0 , January, February, Mars, April, May, June, July, August, September, October, November, December }

    class DriverStatisticViewModel : INotifyPropertyChanged
    {
        private UserModel _user;
        private List<VehicleModel> _vehicles;
        private List<ReportModel> _reports;
        private VehicleModel _selectedVehicle;
        private ReportModel _selectedReport;
        private DrivenVehicleStatistics _fromStartAvarage;
        private decimal _primaryAmountAvarage;
        private decimal _primaryUnitPriceAvarage;
        private decimal _secondaryAmountAvarage;
        private decimal _secondaryUnitPriceAvarage;
        private decimal _primaryAvarageCost;
        private decimal _secondaryAvarageCost;
        private Months _month;
        private List<int> _reportedYears;
        private int _selectedYear;
        private string _statisticLabel;
        private int _selectedIndex;
        private decimal _selectedReportPrimaryAmount;
        private int _selectedReportIndex;
        private decimal _selectedReportPrimaryUnitPrice;
        private decimal _selectedReportSecondaryAmount;
        private decimal _selectedReportSecondaryUnitPrice;
        private int _selectedReportDistance;
        private decimal _selectedReportTotalCost;
        private int _selectedVehicleIndex;


        public DriverStatisticViewModel(UserModel user)
        {
            _user = user;
            FillWindowWithInfo();

            DriverName = _user.Firstname + " " + _user.Lastname;

            GetReports = new DelegateCommand(x => GetSelectedStatistics());
        }

        public int SelectedVehicleIndex
        {
            get { return _selectedVehicleIndex; }
            set
            {
                _selectedVehicleIndex = value;
                OnPropertyChanged();
            }
        }

        public int SelectedYearIndex
        {
            get { return _selectedIndex; }
            set
            {
                _selectedIndex = value;
                OnPropertyChanged();
            }
        }

        public int SelectedReportIndex
        {
            get { return _selectedReportIndex; }
            set
            {
                _selectedReportIndex = value;
                OnPropertyChanged();
            }
        }

        public DelegateCommand AllTimeAvarage { get; set; }
        public DelegateCommand GetReports { get; set; }

        public int SelectedYear
        {
            get { return _selectedYear; }
            set
            {
                _selectedYear = value;
                GetSelectedStatistics();
                OnPropertyChanged();
            }
        }

        public Months SelectedMonth
        {
            get { return _month; }
            set
            {
                _month = value;
                GetSelectedStatistics();
                OnPropertyChanged();
            }
        }

        public string StatisticLabel
        {
            get { return _statisticLabel; }
            set
            {
                _statisticLabel = value;
                OnPropertyChanged();
            }
        }



        public string DriverName { get; set; }

        public decimal PrimaryAmountAvarage
        {
            get { return _primaryAmountAvarage; }
            set
            {
                _primaryAmountAvarage = value;
                OnPropertyChanged();
            }
        }

        public decimal PrimaryUnitPriceAvarage
        {
            get { return _primaryUnitPriceAvarage; }
            set
            {
                _primaryUnitPriceAvarage = value;
                OnPropertyChanged();
            }
        }

        public decimal SecondaryAmountAvarage
        {
            get { return _secondaryAmountAvarage; }
            set
            {
                _secondaryAmountAvarage = value;
                OnPropertyChanged();
            }
        }

        public decimal SecondaryUnitPriceAvarage
        {
            get { return _secondaryUnitPriceAvarage; }
            set
            {
                _secondaryUnitPriceAvarage = value;
                OnPropertyChanged();
            }
        }

        public decimal PrimaryAvarageCost
        {
            get { return _primaryAvarageCost; }
            set
            {
                _primaryAvarageCost = value;
                OnPropertyChanged();
            }
        }

        public decimal SecondaryAvarageCost
        {
            get { return _secondaryAvarageCost; }
            set
            {
                _secondaryAvarageCost = value;
                OnPropertyChanged();
            }
        }

        public decimal SelectedReportPrimaryAmount
        {
            get { return _selectedReportPrimaryAmount; }
            set
            {
                _selectedReportPrimaryAmount = value;
                OnPropertyChanged();
            }
        }


        public decimal SelectedReportPrimaryUnitPrice
        {
            get { return _selectedReportPrimaryUnitPrice; }
            set
            {
                _selectedReportPrimaryUnitPrice = value;
                OnPropertyChanged();
            }
        }

        public decimal SelectedReportSecondaryAmount
        {
            get { return _selectedReportSecondaryAmount; }
            set
            {
                _selectedReportSecondaryAmount = value;
                OnPropertyChanged();
            }
        }

        public decimal SelectedReportSecondaryUnitPrice
        {
            get { return _selectedReportSecondaryUnitPrice; }
            set
            {
                _selectedReportSecondaryUnitPrice = value;
                OnPropertyChanged();
            }
        }

        public int SelectedReportDistance
        {
            get { return _selectedReportDistance; }
            set
            {
                _selectedReportDistance = value;
                OnPropertyChanged();
            }
        }

        public decimal SelectedReportTotalCost
        {
            get { return _selectedReportTotalCost; }
            set
            {
                _selectedReportTotalCost = value;
                OnPropertyChanged();
            }
        }

        public DrivenVehicleStatistics Statistics
        {
            get { return _fromStartAvarage; }
            set
            {
                _fromStartAvarage = value;
                PrimaryAmountAvarage = Statistics.PrimaryFuelAmountBeginning;
                PrimaryUnitPriceAvarage = Statistics.PrimaryFuelUnitBeginning;
                SecondaryAmountAvarage = Statistics.SecondFuelAmountBeginning;
                SecondaryUnitPriceAvarage = Statistics.SecondFuelUnitBeginning;
                PrimaryAvarageCost = Math.Round(PrimaryAmountAvarage * PrimaryUnitPriceAvarage, 2);
                SecondaryAvarageCost = Math.Round(SecondaryAmountAvarage * SecondaryUnitPriceAvarage, 2);
                OnPropertyChanged();
            }
        }

        public VehicleModel SelectedVehicle
        {
            get { return _selectedVehicle; }
            set
            {
                _selectedVehicle = value;
                OnPropertyChanged();
                GetVehicleReports();
                GetYearForVehicle();
                
            }
        }
        
        public List<VehicleModel> Vehicles
        {
            get { return _vehicles; }
            set
            {
                _vehicles = value;
                OnPropertyChanged();
            }
        }

        public ReportModel SelectedReport
        {
            get { return _selectedReport; }
            set
            {
                _selectedReport = value;
                if (_selectedReport != null)
                {
                    SelectedReportPrimaryAmount = _selectedReport.PrimaryFuelAmount;
                    SelectedReportPrimaryUnitPrice = _selectedReport.PrimaryFuelUnitPrice;
                    SelectedReportTotalCost = _selectedReport.TotalPrice;
                    SelectedReportDistance = _selectedReport.Distance;
                    if (SelectedReport.SecondaryFuel_ID != null)
                    {
                        SelectedReportSecondaryAmount = _selectedReport.SecondaryFuelAmount.Value;
                        SelectedReportSecondaryUnitPrice = _selectedReport.SecondaryFuelUnitPrice.Value;
                    }
                    

                }
                OnPropertyChanged();
            }
        }

        public List<ReportModel> Reports
        {
            get { return _reports; }
            set
            {
                _reports = value;
                SelectedReportPrimaryAmount = 0;
                SelectedReportPrimaryUnitPrice = 0;
                SelectedReportSecondaryAmount = 0;
                SelectedReportSecondaryUnitPrice = 0;
                SelectedReportDistance = 0;
                SelectedReportTotalCost = 0;
                OnPropertyChanged();
            }
        }

        public List<int> ReportedYears
        {
            get { return _reportedYears; }
            set
            {
                _reportedYears = value;
                SelectedYearIndex = 0;
                OnPropertyChanged();
            }
        }

        public void GetVehicleReports()
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Helpers.CredentialHelper.Credential);

                string getreports = $"http://moggeapi.azurewebsites.net/api/driver/getreports/{_selectedVehicle.RegNr}/{_user.Id}";
                string getvehicleStatistics = $"http://moggeapi.azurewebsites.net/api/statistics/getDriverVehicleAvarage/{_selectedVehicle.RegNr}/{_user.Id}";

                var jsonReports = client.GetAsync(getreports).Result.Content.ReadAsStringAsync().Result;
                var reports = JsonConvert.DeserializeObject<List<ReportModel>>(jsonReports);
                Reports = reports;

                var jsonStatistics = client.GetAsync(getvehicleStatistics).Result.Content.ReadAsStringAsync().Result;
                var statistics = JsonConvert.DeserializeObject<DrivenVehicleStatistics>(jsonStatistics);
                Statistics = statistics;

                SelectedReportIndex = -1;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                throw;
            }
           
        }

        public async void FillWindowWithInfo()
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Helpers.CredentialHelper.Credential);
                string getVehicles = $"http://moggeapi.azurewebsites.net/api/driver/Getvehicle/{_user.Id}";
                var jsonVehicles = client.GetAsync(getVehicles).Result.Content.ReadAsStringAsync().Result;
                var vehicles = JsonConvert.DeserializeObject<List<VehicleModel>>(jsonVehicles);

                Vehicles = vehicles;

                SelectedVehicleIndex = 0;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                throw;
            }
           
        }

        public void GetYearForVehicle()
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Helpers.CredentialHelper.Credential);
                string getReportYears = $"http://moggeapi.azurewebsites.net/api/statistics/yearreports/{_user.Id}/{SelectedVehicle.RegNr}";
                var jsonYears = client.GetAsync(getReportYears).Result.Content.ReadAsStringAsync().Result;
                var years = JsonConvert.DeserializeObject<List<int>>(jsonYears);
                ReportedYears = null;
                ReportedYears = years;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                throw;
            }
           
            
        }

        public void GetSelectedStatistics()
        {
            Months value = _month;
            int month = (int)value;

            string getStatistics = $"http://moggeapi.azurewebsites.net/api/statistics/driveryearmonthstatistics/{_selectedVehicle.RegNr}/{_user.Id}/{_selectedYear}/{month}";

            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Helpers.CredentialHelper.Credential);
                var jsonStatistics = client.GetAsync(getStatistics).Result.Content.ReadAsStringAsync().Result;
                var statistics = JsonConvert.DeserializeObject<DrivenVehicleStatistics>(jsonStatistics);
                Statistics = statistics;
                Reports = statistics.Reports;

                if (value == Months.Alltime)
                {
                    StatisticLabel = _month.ToString();
                }

                if (value != Months.Year && value != Months.Alltime)
                {
                    StatisticLabel = _month + " " + _selectedYear;
                }
                if (value == Months.Year)
                {
                    StatisticLabel = _selectedYear.ToString();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                throw;
            }

            
        }
        public void GetSelectedReportStatistics()
        {
            Months value = _month;

            string getStatistics = $"http://moggeapi.azurewebsites.net/api/statistics/driverreportstatistics/{_selectedReport}";
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Helpers.CredentialHelper.Credential);
                var jsonStatistics = client.GetAsync(getStatistics).Result.Content.ReadAsStringAsync().Result;
                var statistics = JsonConvert.DeserializeObject<DrivenVehicleStatistics>(jsonStatistics);
                Statistics = statistics;
                Reports = statistics.Reports;

                if (value == Months.Alltime)
                {
                    StatisticLabel = _month.ToString();
                }

                if (value != Months.Year && value != Months.Alltime)
                {
                    StatisticLabel = _month + " " + _selectedYear;
                }
                else
                {
                    StatisticLabel = _selectedYear.ToString();
                }
            }
            catch (Exception)
            {
                
                throw;
            }
          
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



    }
}
