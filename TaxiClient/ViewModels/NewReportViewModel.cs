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
using System.Windows.Media;
using Newtonsoft.Json;
using TaxiClient.Annotations;
using TaxiClient.Helpers;
using TaxiClient.Models;

namespace TaxiClient.ViewModels
{
    class NewReportViewModel : INotifyPropertyChanged
    {
       
        private readonly UserModel _user;
        private int _id;
        private int _fuelId;
        private string _firstName;
        private string _lastName;
        private string _regNr;
        private List<FuelModel> _fuels;
        private List<VehicleModel> _vehicles;
        private int _tripMeter;
        private decimal _fuelAmount;
        private decimal _unitPrice;
        private decimal _totalPrice;
        private string _fuelOne;
        private string _hybridFuel;
        private bool _isTexBoxVisible;
        private decimal _hybridAmount;
        private decimal _hybridUnitPrice;
        private int _hybridFuelType;


        private VehicleModel _vehicle;
        private int _distance;
        private int _tripMeterNow;
        private List<VehicleModel> _multiList;
        private bool _isAdmin;
        private List<UserModel> _users;
        private UserModel _selectedUser;
        private bool _isDriverReport;
        private DateTime _employeeReportDate;
        private string _infoLabel;


        public NewReportViewModel(UserModel user)
        {
            _user = user;
            IsAdmin = _user.IsAdmin;

            GetVehicles();
        
            AddReport = new DelegateCommand(x => SaveReport(), x => CanSaveReport());

            EmployeeId = user.Id;
            FirstName = user.Firstname;
            LastName = user.Lastname;
        }


        public string InfoLabel
        {
            get { return _infoLabel; }
            set
            {
                _infoLabel = value;
                OnPropertyChanged();
            }
        }

        public bool IsDriverReport
        {
            get { return _isDriverReport; }
            set
            {
                _isDriverReport = value;
                OnPropertyChanged();
                if (Users == null && IsDriverReport)
                {
                    EmployeeReportDate = DateTime.Now;
                    GetEmployees();
                }
            }
        }

        public DateTime EmployeeReportDate
        {
            get { return _employeeReportDate; }
            set
            {
                _employeeReportDate = value;
                OnPropertyChanged();
            }
        }

        public VehicleModel Vehicle
        {
            get { return _vehicle; }
            set
            {
                _vehicle = value;
                OnPropertyChanged();
                if (Vehicle != null)
                    TripMeterNow = Vehicle.TripMeter;
                AddReport.RaiseCanExecuteChanged();
            }
        }
       
        public int TripMeterNow
        {
            get { return _tripMeterNow; }
            set
            {
                _tripMeterNow = value;
                OnPropertyChanged();
            }
        }
        public int EmployeeId
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }
        public int PrimaryFuel_ID
        {
            get { return _fuelId; }
            set
            {
                _fuelId = value;
                OnPropertyChanged();
            }
        }
        public int TripMeter
        {
            get { return _tripMeter; }
            set
            {

                _tripMeter = value;
                OnPropertyChanged();
                AddReport.RaiseCanExecuteChanged();
                Calculate();
            }
        }
        public int SecondaryFuel_ID
        {
            get { return _hybridFuelType; }
            set
            {
                _hybridFuelType = value;
                OnPropertyChanged();
            }
        }
        public bool IsTextBoxVisible
        {
            get { return _isTexBoxVisible; }
            set
            {
                _isTexBoxVisible = value;
                OnPropertyChanged();
            }
        }
        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }
        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }
        public string RegNr
        {
            get { return _regNr; }
            set
            {
                _regNr = value;
                GetFuelList();
            }
        }
        public string PrimaryFuelType
        {
            get { return _fuelOne; }
            set
            {
                _fuelOne = value;
                OnPropertyChanged();
            }
        }
        public string SecondaryFuelType
        {
            get { return _hybridFuel; }
            set
            {
                _hybridFuel = value;
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
            }
        }
        public decimal PrimaryFuelAmount
        {
            get { return _fuelAmount; }
            set
            {
                _fuelAmount = value;
                OnPropertyChanged();
                Calculate();
            }
        }
        public decimal SecondaryFuelAmount
        {
            get { return _hybridAmount; }
            set
            {
                _hybridAmount = value;
                OnPropertyChanged();
                Calculate();
            }
        }
        public decimal PrimaryFuelUnitPrice
        {
            get { return _unitPrice; }
            set
            {
                _unitPrice = value;
                OnPropertyChanged();
                Calculate();
            }
        }
        public decimal SecondaryFuelUnitPrice
        {
            get { return _hybridUnitPrice; }
            set
            {
                _hybridUnitPrice = value;
                OnPropertyChanged();
                Calculate();
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

        public bool IsAdmin
        {
            get { return _isAdmin; }
            set
            {
                _isAdmin = value;
                OnPropertyChanged();
            }
        }

        public UserModel SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value; 
                OnPropertyChanged();
            }
        }

        public List<UserModel> Users
        {
            get { return _users; }
            set
            {
                _users = value;
                OnPropertyChanged();
            }
        }

        public List<FuelModel> Fuels
        {
            get { return _fuels; }
            set
            {
                _fuels = value;
                OnPropertyChanged();
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

        public DelegateCommand AddReport { get; set; }

        public bool CanSaveReport()
        {
            if (!IsDriverReport)
            {
                return Vehicle != null && Validators.ValidateTripmeterValue(Vehicle.TripMeter, TripMeter);
               
            }
            else
            {
                return true;
            }
                
        }

        public async void SaveReport()
        {
            try
            {
               
                MessageBoxResult result;
                var report = new ReportModel();

                if (IsTextBoxVisible)
                {

                    report.Vehicle_ID = RegNr;
                    

                    if (IsDriverReport)
                    {
                        report.Employee_ID = _selectedUser.Id;
                        report.ReportDate = _employeeReportDate;
                        report.Distance = 0;

                    }
                    else
                    {
                        report.Distance = Distance;
                        report.Employee_ID = EmployeeId;
                        report.ReportDate = DateTime.Now;
                    }

                    report.PrimaryFuel_ID = PrimaryFuel_ID;
                    report.PrimaryFuelUnitPrice = PrimaryFuelUnitPrice;
                    report.PrimaryFuelAmount = PrimaryFuelAmount;

                    report.SecondaryFuel_ID = SecondaryFuel_ID;
                    report.SecondaryFuelAmount = SecondaryFuelAmount;
                    report.SecondaryFuelUnitPrice = SecondaryFuelUnitPrice;
                    report.TotalPrice = _totalPrice;
                }
                else
                {
                    report.Vehicle_ID = RegNr;
                   

                    if (IsDriverReport)
                    {
                        report.Employee_ID = _selectedUser.Id;
                        report.ReportDate = _employeeReportDate;
                        Distance = 0;
                    }
                    else
                    {
                        report.Employee_ID = EmployeeId;
                        report.ReportDate = DateTime.Now;
                        report.Distance = Distance;
                    }

                    report.PrimaryFuel_ID = PrimaryFuel_ID;
                    report.PrimaryFuelUnitPrice = PrimaryFuelUnitPrice;
                    report.PrimaryFuelAmount = PrimaryFuelAmount;

                    report.SecondaryFuel_ID = null;
                    report.SecondaryFuelAmount = null;
                    report.SecondaryFuelUnitPrice = null;

                    report.TotalPrice = TotalPrice;
                };

                if (report.SecondaryFuel_ID != null)
                {
                    result = MessageBox.Show("Add report?\n" +
                               "Date: " + report.ReportDate.ToShortDateString() + "\n" +
                               "Vehicle: " + report.Vehicle_ID + "\n" +
                               "Distance: " + report.Distance + "\n" +
                               "Primary Fuel Amount: " + report.PrimaryFuelAmount + "at " + report.PrimaryFuelUnitPrice +
                               "\n" +

                               "Secondary Fuel Amount: " + report.SecondaryFuelAmount + "at " +
                               report.SecondaryFuelUnitPrice + "\n" +
                               "Total Cost: " + report.TotalPrice, _user.Firstname + " " + _user.Lastname, MessageBoxButton.YesNo);
                }
                else
                {
                    result = MessageBox.Show("Add report?\n" +
                               "Date: " + report.ReportDate.ToShortDateString() + "\n" +
                               "Vehicle: " + report.Vehicle_ID + "\n" +
                               "Distance: " + report.Distance + "\n" +
                               "Primary Fuel Amount: " + report.PrimaryFuelAmount + "at " + report.PrimaryFuelUnitPrice + "\n" +
                               "Total Cost: " + report.TotalPrice, _user.Firstname + " " + _user.Lastname, MessageBoxButton.YesNo);
                }


                if (result == MessageBoxResult.Yes)
                {
                    
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Helpers.CredentialHelper.Credential);
                    client.BaseAddress = new Uri("http://moggeapi.azurewebsites.net/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await client.PostAsJsonAsync("api/report/addreport", report);

                    if (response.IsSuccessStatusCode)
                    {
                        TripMeterNow = 0;
                        GetVehicles();

                        InfoLabel = "Succesfully saved!";
                    }
                }
                else
                {
                    InfoLabel = "Something went wrong, please try again.";
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Something went wrong, please try again.");
                throw;
            }
        }

        

        public void GetFuelList()
        {

            PrimaryFuelUnitPrice = 0;
            PrimaryFuelAmount = 0;
            SecondaryFuelAmount = 0;
            SecondaryFuelUnitPrice = 0;
            TripMeter = 0;


            if (_regNr != null)
            {
                var list = new List<FuelModel>();
                try
                {
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Helpers.CredentialHelper.Credential);

                    var jsonString =
                        client.GetAsync("http://localhost:51587/api/vehicle/getvehicleinfo/" + RegNr)
                            .Result.Content.ReadAsStringAsync()
                            .Result;

                    list = JsonConvert.DeserializeObject<List<FuelModel>>(jsonString).ToList();
                    Fuels = list;


                    var array = list.ToArray();
                    PrimaryFuelType = array[0].FuelType;
                    PrimaryFuel_ID = array[0].Id;
                    if (array.Length > 1)
                    {
                        SecondaryFuelType = array[1].FuelType;
                        SecondaryFuel_ID = array[1].Id;
                        IsTextBoxVisible = true;
                        Calculate();
                    }
                    else
                    {
                        SecondaryFuelType = null;
                        IsTextBoxVisible = false;
                        Calculate();
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Someting went wrong, Please try again.");
                    throw;
                }
                
            }

        }

        public void GetVehicles()
        {
            var list = new List<VehicleModel>();
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Helpers.CredentialHelper.Credential);
                var jsonString = client.GetAsync("http://moggeapi.azurewebsites.net/api/vehicle/getvehicles").Result.Content.ReadAsStringAsync().Result;
                list = JsonConvert.DeserializeObject<List<VehicleModel>>(jsonString);
                Vehicles = list;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                throw;
            }
            
        }

        public void GetEmployees()
        {
            var list = new List<UserModel>();
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Helpers.CredentialHelper.Credential);
                var jsonString = client.GetAsync("http://moggeapi.azurewebsites.net/api/User/getusers").Result.Content.ReadAsStringAsync().Result;
                list = JsonConvert.DeserializeObject<List<UserModel>>(jsonString);
                Users = list;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                throw;
            }
            
        }

        public void Calculate()
        {
            var totalPrimaryFuelCost = PrimaryFuelAmount * PrimaryFuelUnitPrice;
            var totalHybridCost = SecondaryFuelUnitPrice * SecondaryFuelAmount;

            TotalPrice = totalHybridCost + totalPrimaryFuelCost;

            if (Vehicle != null)
                if (TripMeter - Vehicle.TripMeter > 0)
                {
                    Distance = TripMeter - Vehicle.TripMeter;
                }
                else
                {
                    Distance = 0;
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
