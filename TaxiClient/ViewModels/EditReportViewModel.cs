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
using TaxiClient.Models;
using TaxiClient.Annotations;
using TaxiClient.Helpers;

namespace TaxiClient.ViewModels
{
    class EditReportViewModel : INotifyPropertyChanged
    {
        private UserModel _user;
        private List<ReportModel> _reports;
        private List<UserModel> _employees;
        private UserModel _selectedEmployee;
        private ReportModel _selectedReport;
        private int _primaryFuelId;
        private int _distance;
        private string _regNr;
        private DateTime _reportDate;
        private decimal _primaryFuelAmount;
        private decimal _primaryFuelUnitPrice;
        private decimal _totalPrice;
        private decimal? _secondaryFuelAmount;
        private int? _secondaryFuelId;
        private decimal? _secondaryFuelUnitPrice;
        private int _tripmeterAtDate;
        private string _fullNameAndCar;
        private string _vehicleTripmeter;

        public EditReportViewModel(UserModel user)
        {
            _user = user;
            GetEmployees();

            Update = new DelegateCommand(x => UpdateReport(), x => CanUpdateReport());
        }

        public string FullNameAndCar
        {
            get { return _fullNameAndCar; }
            set
            {
                _fullNameAndCar = value;
                OnPropertyChanged();
            }
        }

        public int TripmeterAtDate
        {
            get { return _tripmeterAtDate; }
            set
            {
                _tripmeterAtDate = value;
                OnPropertyChanged();
            }
        }

        public int PrimaryFuelId
        {
            get { return _primaryFuelId; }
            set
            {
                _primaryFuelId = value;
                OnPropertyChanged();
            }
        }

        public int Distance
        {
            get { return _distance; }
            set
            {
                _distance = value;
                OnPropertyChanged();
                Update.RaiseCanExecuteChanged();
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

        public DateTime ReportDate
        {
            get { return _reportDate; }
            set
            {
                _reportDate = value;
                GetTripMeter();
                OnPropertyChanged();
            }
        }

        public decimal PrimaryFuelAmount
        {
            get { return _primaryFuelAmount; }
            set
            {
                _primaryFuelAmount = value;
                OnPropertyChanged();
                Update.RaiseCanExecuteChanged();
            }
        }

        public decimal PrimaryFuelUnitPrice
        {
            get { return _primaryFuelUnitPrice; }
            set
            {
                _primaryFuelUnitPrice = value;
                OnPropertyChanged();
            }
        }

        public decimal TotalPrice
        {
            get { return _totalPrice; }
            set
            {
                _totalPrice = value;
                OnPropertyChanged();
            }
        }

        public decimal? SecondaryFuelAmount
        {
            get { return _secondaryFuelAmount; }
            set
            {
                _secondaryFuelAmount = value;
                OnPropertyChanged();
            }
        }

        public int? SecondaryFuelId
        {
            get { return _secondaryFuelId; }
            set
            {
                _secondaryFuelId = value;
                OnPropertyChanged();
            }
        }

        public decimal? SecondaryFuelUnitPrice
        {
            get { return _secondaryFuelUnitPrice; }
            set
            {
                _secondaryFuelUnitPrice = value;
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
                    PrimaryFuelId = _selectedReport.PrimaryFuel_ID;
                    RegNr = _selectedReport.Vehicle_ID;
                    ReportDate = _selectedReport.ReportDate;
                    Distance = TripmeterAtDate;
                    PrimaryFuelAmount = _selectedReport.PrimaryFuelAmount;
                    PrimaryFuelUnitPrice = _selectedReport.PrimaryFuelUnitPrice;
                    TotalPrice = _selectedReport.TotalPrice;
                    if (_selectedReport.SecondaryFuel_ID != null)
                    {
                        SecondaryFuelId = _selectedReport.SecondaryFuel_ID;
                        SecondaryFuelAmount = _selectedReport.SecondaryFuelAmount;
                        SecondaryFuelUnitPrice = _selectedReport.SecondaryFuelUnitPrice;
                    }
                }
                if(_selectedReport != null)
                FullNameAndCar = SelectedEmployee.Firstname + " " + _selectedEmployee.Lastname + " " + _selectedReport.Vehicle_ID;

                OnPropertyChanged();
            }
        }

        public UserModel SelectedEmployee
        {
            get { return _selectedEmployee; }
            set
            {
                _selectedEmployee = value;
                GetReports();
                OnPropertyChanged();
            }
        }

        public List<ReportModel> Reports
        {
            get { return _reports; }
            set
            {
                _reports = value;
                OnPropertyChanged();
            }
        }

        public List<UserModel> Employees
        {
            get { return _employees; }
            set
            {
                _employees = value;
                OnPropertyChanged();
            }
        }

        public string VehicleTripmeter
        {
            get { return _vehicleTripmeter; }
            set
            {
                _vehicleTripmeter = value;
                OnPropertyChanged();
            }
        }

        public DelegateCommand Update { get; set; }

        public void GetEmployees()
        {
            try
            {
                string getEmployees = $"http://moggeapi.azurewebsites.net/api/user/getusers";
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Helpers.CredentialHelper.Credential);
                var jsonEmployees = client.GetAsync(getEmployees).Result.Content.ReadAsStringAsync().Result;
                var employees = JsonConvert.DeserializeObject<List<UserModel>>(jsonEmployees);

                Employees = employees;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                throw;
            }
            
        }

        public void GetReports()
        {
            string getReports = $"http://moggeapi.azurewebsites.net/api/report/getuserreports/{_selectedEmployee.Id}";
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Helpers.CredentialHelper.Credential);
                var jsonReports = client.GetAsync(getReports).Result.Content.ReadAsStringAsync().Result;
                var reports = JsonConvert.DeserializeObject<List<ReportModel>>(jsonReports);
                Reports = reports;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                throw;
            }
          
        }

        public bool CanUpdateReport()
        {
            if (TripmeterAtDate <= Distance && 
                Validators.ValidateTripMeter(Distance) &&
                Validators.ValidateDecimal(PrimaryFuelAmount.ToString()))
                return true;

            return false;
        }

        public async void UpdateReport()
        {
            var report = new ReportModel()
            {

                Id = _selectedReport.Id,
                Vehicle_ID = _selectedReport.Vehicle_ID,
                ReportDate = _reportDate,
                Distance = _distance - TripmeterAtDate,
                Employee_ID = _selectedReport.Employee_ID,
                PrimaryFuel_ID = _primaryFuelId,
                PrimaryFuelAmount = _primaryFuelAmount,
                PrimaryFuelUnitPrice = _primaryFuelUnitPrice,
                TotalPrice = _totalPrice
            };
            if (SecondaryFuelId != null)
            {
                report.SecondaryFuel_ID = _secondaryFuelId;
                report.SecondaryFuelAmount = _secondaryFuelAmount;
                report.SecondaryFuelUnitPrice = _secondaryFuelUnitPrice;
            }

            var messageboxResult = MessageBox.Show("You are about to ubdate a report, is this right?", "Update",
                MessageBoxButton.YesNo);
            if (messageboxResult == MessageBoxResult.Yes)
            {
                try
                {
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                        Helpers.CredentialHelper.Credential);
                    client.BaseAddress = new Uri("http://moggeapi.azurewebsites.net/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await client.PutAsJsonAsync("api/report/updatereport/", report);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                    throw;
                }
            }

        }

        public void GetTripMeter()
        {
            string getVehicle = $"http://moggeapi.azurewebsites.net/api/vehicle/gettripmeter/{_selectedReport.Vehicle_ID}";

            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Helpers.CredentialHelper.Credential);
                var jsonReports = client.GetAsync(getVehicle).Result.Content.ReadAsStringAsync().Result;
                var tripmeter = JsonConvert.DeserializeObject<int>(jsonReports);

                TripmeterAtDate = tripmeter;


                foreach (var report in Reports)
                {
                    if (report.Vehicle_ID == _selectedReport.Vehicle_ID && report.ReportDate <= _reportDate)
                    {
                        TripmeterAtDate += report.Distance;
                    }
                }
                FullNameAndCar = _selectedEmployee.Firstname + " " + _selectedEmployee.Lastname + " " + _selectedReport.Vehicle_ID;
                VehicleTripmeter = $"Tripmeter ({TripmeterAtDate})";
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
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
