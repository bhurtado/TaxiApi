using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using TaxiClient.Helpers;
using TaxiClient.Annotations;
using TaxiClient.Models;

namespace TaxiClient.ViewModels
{
    class VehicleViewModel : INotifyPropertyChanged
    {
        private List<VehicleModelForView> _vehicles;
        private VehicleModelForView _selectedVehicle;
        private string _color;
        private string _manufacturer;
        private string _primaryFuel;
        private string _secondaryFuel;
        private string _model;
        private int _tripmeter;
        private int _yearModel;
        private string _regNr;
        private int _selectedVehicleDistance;
        private decimal _selectedVehicleAvarageDistance;
        private decimal _selectedVehicleTotalCost;
        private decimal _selectedVehicleAvarageFuelTenKm;
        private decimal _selectedVehicleAvarageCost;
        private decimal _selectedVehicleAvaragePrimaryFuelAmount;
        private decimal _selectedVehicleAvarageSecondaryFuelAmount;
        private decimal _selectedVehicleAvaragePrimaryFuelCost;
        private decimal _selectedVehicleAvarageSecondaryFuelCost;
        private Months _selectedMonth;
        private List<int> _years;
        private int _selectedYear;
        private int _selectedVehicleIndex;
        private int _selectedYearIndex;


        public VehicleViewModel()
        {
            GetVehicles();
            NewVehicle = new DelegateCommand(x => AddNewVehicle());
            GetStatistics = new DelegateCommand(x => GetVehicleStatistics());
        }


        public int SelectedYearIndex
        {
            get { return _selectedYearIndex; }
            set
            {
                _selectedYearIndex = value;
                OnPropertyChanged();
            }
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

        public Months SelectedMonth
        {
            get { return _selectedMonth; }
            set
            {
                _selectedMonth = value;
                OnPropertyChanged();
            }
        }

        public int SelectedYear
        {
            get { return _selectedYear; }
            set
            {
                _selectedYear = value;
                OnPropertyChanged();
            }
        }

        public List<int> Years
        {
            get { return _years; }
            set
            {
                _years = value; 
                OnPropertyChanged();
            }
        }

        

        public int YearModel
        {
            get { return _yearModel; }
            set
            {
                _yearModel = value;
                OnPropertyChanged();
            }
        }

        public int Tripmeter
        {
            get { return _tripmeter; }
            set
            {
                _tripmeter = value;
                OnPropertyChanged();
            }
        }

        public string Model
        {
            get { return _model; }
            set
            {
                _model = value; 
                OnPropertyChanged();
            }
        }

        public string SecondaryFuel
        {
            get { return _secondaryFuel; }
            set
            {
                _secondaryFuel = value;
                OnPropertyChanged();
            }
        }

        public string PrimaryFuel
        {
            get { return _primaryFuel; }
            set
            {
                _primaryFuel = value;
                OnPropertyChanged();
            }
        }

        public string Manufacturer
        {
            get { return _manufacturer; }
            set
            {
                _manufacturer = value;
                OnPropertyChanged();
            }
        }

        public string Color
        {
            get { return _color; }
            set
            {
                _color = value;
                OnPropertyChanged();
            }
        }

        public string RegNr
        {
            get { return _regNr; }
            set
            {
                _regNr = value;
                OnPropertyChanged();
            }
        }

        public int SelectedVehicleDistance
        {
            get { return _selectedVehicleDistance; }
            set
            {
                _selectedVehicleDistance = value;
                OnPropertyChanged();
            }
        }

        public decimal SelectedVehicleAvarageDistance
        {
            get { return _selectedVehicleAvarageDistance; }
            set
            {
                _selectedVehicleAvarageDistance = value;
                OnPropertyChanged();
            }
        }

        public decimal SelectedVehicleTotalCost
        {
            get { return _selectedVehicleTotalCost; }
            set
            {
                _selectedVehicleTotalCost = value;
                OnPropertyChanged();
            }
        }

        public decimal SelectedVehicleAvarageFuelTenKm
        {
            get { return _selectedVehicleAvarageFuelTenKm; }
            set
            {
                _selectedVehicleAvarageFuelTenKm = value;
                OnPropertyChanged();
            }
        }

        public decimal SelectedVehicleAvarageCost
        {
            get { return _selectedVehicleAvarageCost; }
            set
            {
                _selectedVehicleAvarageCost = value;
                OnPropertyChanged();
            }
        }

        public decimal SelectedVehicleAvaragePrimaryFuelAmount
        {
            get { return _selectedVehicleAvaragePrimaryFuelAmount; }
            set
            {
                _selectedVehicleAvaragePrimaryFuelAmount = value;
                OnPropertyChanged();
            }
        }

        public List<VehicleModelForView> Vehicles
        {
            get { return _vehicles; }
            set
            {
                _vehicles = value;
                OnPropertyChanged();
            }
        }

        public decimal SelectedVehicleAvarageSecondaryFuelAmount
        {
            get { return _selectedVehicleAvarageSecondaryFuelAmount; }
            set
            {
                _selectedVehicleAvarageSecondaryFuelAmount = value;
                OnPropertyChanged();
            }
        }

        public decimal SelectedVehicleAvaragePrimaryFuelCost
        {
            get { return _selectedVehicleAvaragePrimaryFuelCost; }
            set
            {
                _selectedVehicleAvaragePrimaryFuelCost = value;
                OnPropertyChanged();
            }
        }

        public decimal SelectedVehicleAvarageSecondaryFuelCost
        {
            get { return _selectedVehicleAvarageSecondaryFuelCost; }
            set
            {
                _selectedVehicleAvarageSecondaryFuelCost = value;
                OnPropertyChanged();
            }
        }

        public VehicleModelForView SelectedVehicle
        {
            get { return _selectedVehicle; }
            set
            {
                _selectedVehicle = value;

                Color = _selectedVehicle.Color;
                Manufacturer = _selectedVehicle.Manufacturer;
                Model = _selectedVehicle.Model;
                PrimaryFuel = _selectedVehicle.PrimaryFuel;
                SecondaryFuel = _selectedVehicle.SecondaryFuel;
                Tripmeter = _selectedVehicle.Tripmeter;
                YearModel = _selectedVehicle.Yearmodel;
                RegNr = _selectedVehicle.RegNr;
                GetYears();
                GetVehicleStatistics();
                OnPropertyChanged();
            }
        }

        public DelegateCommand NewVehicle { get; set; }
        public DelegateCommand GetStatistics { get; set; }

        public void GetVehicles()
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Helpers.CredentialHelper.Credential);
                var jsonString = client.GetAsync("http://moggeapi.azurewebsites.net/api/vehicle/getforviewvehicles/").Result.Content.ReadAsStringAsync().Result;
                var vehicleList = JsonConvert.DeserializeObject<List<VehicleModelForView>>(jsonString);

                Vehicles = vehicleList;
                SelectedVehicleIndex = 0;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                throw;
            }
        

            
        }

        public void GetVehicleStatistics()
        {
            var value = _selectedMonth;
            var month = (int) value;
            try
            {
                var getVehiclereports = $"http://moggeapi.azurewebsites.net/api/report/getvehiclereports/{RegNr}/{month}/{SelectedYear}";

                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Helpers.CredentialHelper.Credential);
                var jsonString = client.GetAsync(getVehiclereports).Result.Content.ReadAsStringAsync().Result;
                var reportList = JsonConvert.DeserializeObject<List<ReportModel>>(jsonString);

                var statistics = CountStatistics.VehicleStatistics(reportList);

                SelectedVehicleAvarageCost = statistics.AvarageCost;
                SelectedVehicleAvarageDistance = statistics.AvarageDistance;
                SelectedVehicleAvarageFuelTenKm = statistics.AvaragePrimaryFuelCostTenKm;
                SelectedVehicleAvaragePrimaryFuelAmount = statistics.AvaragePrimaryFuelAmount;
                SelectedVehicleAvaragePrimaryFuelCost = statistics.AvaragePrimaryFuelCost;
                SelectedVehicleTotalCost = statistics.TotalCost;
                SelectedVehicleDistance = statistics.Distance;

                if (statistics.AvarageSecondaryFuelAmount > 0)
                {
                    SelectedVehicleAvarageSecondaryFuelAmount = statistics.AvarageSecondaryFuelAmount;
                    SelectedVehicleAvarageSecondaryFuelCost = statistics.AvarageSecondaryFuelCost;
                }
                else
                {
                    SelectedVehicleAvarageSecondaryFuelAmount = 0;
                    SelectedVehicleAvarageSecondaryFuelCost = 0;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                throw;
            }
           


        }

        public void GetYears()
        {
            var getVehicleYears = $"http://moggeapi.azurewebsites.net/api/statistics/getyears/{RegNr}";
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Helpers.CredentialHelper.Credential);
                var jsonString = client.GetAsync(getVehicleYears).Result.Content.ReadAsStringAsync().Result;
                var years = JsonConvert.DeserializeObject<List<int>>(jsonString);

                Years = years;
                SelectedYearIndex = 0;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                throw;
            }
           
        }

        public void AddNewVehicle()
        {
            var window = new AddVehicleWindow1();
            window.DataContext = new AddVehicleInfoViewModel(new VehicleModel());
            window.Show();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
