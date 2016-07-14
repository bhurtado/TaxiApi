using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using TaxiClient.Helpers;
using TaxiClient.Models;
using TaxiClient.Annotations;

namespace TaxiClient.ViewModels
{
    class EmployeesViewModel : INotifyPropertyChanged
    {
        private UserModel _user;
        private List<UserModel> _employees;
        private UserModel _selectedEmployee;
        private List<VehicleModel> _vehicles;
        private string _fullName;
        private decimal _totalCost;
        private decimal _avarageCost;
        private decimal _avarageDistance;
        private int _distance;
        private decimal _avarageCostTenKm;
        private Months _selectedMonth;
        private List<int> _years;
        private int _selectedEmployeeIndex;
        private int _selectedYearsIndex;
        private int _selectedYear;
        private string _employeeStatisticsLabel;

        public EmployeesViewModel(UserModel user)
        {
            _user = user;

            FullName = _user.Firstname + " " + _user.Lastname;
            GetEmployees();
           
            AddEmployee = new DelegateCommand(x => NewEmployee());
        }

        public int SelectedEmployeeIndex
        {
            get { return _selectedEmployeeIndex; }
            set
            {
                _selectedEmployeeIndex = value;
                OnPropertyChanged();
            }
        }

        public int SelectedYearsIndex
        {
            get { return _selectedYearsIndex; }
            set
            {
                _selectedYearsIndex = value;
                OnPropertyChanged();
            }
        }

        public Months SelectedMonth
        {
            get { return _selectedMonth; }
            set
            {
                _selectedMonth = value;
                GetReports();
                OnPropertyChanged();
            }
        }

        public int SelectedYear
        {
            get { return _selectedYear; }
            set
            {
                _selectedYear = value;
                GetReports();
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

        public string FullName
        {
            get { return _fullName; }
            set
            {
                _fullName = value;
                OnPropertyChanged();
            }
        }

        public string EmployeeStatisticsLabel
        {
            get { return _employeeStatisticsLabel; }
            set
            {
                _employeeStatisticsLabel = value; 
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

        public decimal TotalCost
        {
            get { return _totalCost; }
            set
            {
                _totalCost = value;
                OnPropertyChanged();
            }
        }

        public decimal AvarageCost
        {
            get { return _avarageCost; }
            set
            {
                _avarageCost = value;
                OnPropertyChanged();
            }
        }

        public decimal AvarageDistance
        {
            get { return _avarageDistance; }
            set
            {
                _avarageDistance = value;
                OnPropertyChanged();
            }
        }

        public decimal AvarageCostTenKm
        {
            get { return _avarageCostTenKm; }
            set
            {
                _avarageCostTenKm = value;
                OnPropertyChanged();
            }
        }

        public UserModel SelectedEmployee
        {
            get { return _selectedEmployee; }
            set
            {
                _selectedEmployee = value;
                GetSelectedEmployeeVehicles(_selectedEmployee.Id);
                GetYears();
                GetReports();
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

        public List<VehicleModel> Vehicles
        {
            get { return _vehicles; }
            set
            {
                _vehicles = value;
                OnPropertyChanged();
            }
        }

        public void GetYears()
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Helpers.CredentialHelper.Credential);
                string getYears = $"http://moggeapi.azurewebsites.net/api/statistics/yearreports/{_selectedEmployee.Id}";

                var jsonYears = client.GetAsync(getYears).Result.Content.ReadAsStringAsync().Result;
                var yearList = JsonConvert.DeserializeObject<List<int>>(jsonYears);
                Years = yearList;

                SelectedYearsIndex = 0;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                throw;
            }
         
        }



        public void GetEmployees()
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Helpers.CredentialHelper.Credential);
                string getUsers = $"http://moggeapi.azurewebsites.net/api/user/getusers";

                var jsonUsers = client.GetAsync(getUsers).Result.Content.ReadAsStringAsync().Result;
                var employees = JsonConvert.DeserializeObject<List<UserModel>>(jsonUsers);
                Employees = employees;

                SelectedEmployeeIndex = 0;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                throw;
            }
            
        }

        public void GetSelectedEmployeeVehicles(int userId)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Helpers.CredentialHelper.Credential);
                string getVehicles = $"http://moggeapi.azurewebsites.net/api/vehicles/getvehicles/{_selectedEmployee.Id}";
                var jsonUsers = client.GetAsync(getVehicles).Result.Content.ReadAsStringAsync().Result;
                var vehicles = JsonConvert.DeserializeObject<List<VehicleModel>>(jsonUsers);
                Vehicles = vehicles;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                throw;
            }
           
        }

        public void GetReports()
        {
            Months value = _selectedMonth;
            int month = (int)value;

            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Helpers.CredentialHelper.Credential);
                string getReports = $"http://moggeapi.azurewebsites.net/api/report/getuserreports/{_selectedEmployee.Id}/{month}/{_selectedYear}";

                var jsonUsers = client.GetAsync(getReports).Result.Content.ReadAsStringAsync().Result;
                var reports = JsonConvert.DeserializeObject<List<ReportModel>>(jsonUsers);
                if (reports.Count > 0)
                {
                    var fixedStatistics = CountStatistics.Count(reports);

                    AvarageCost = Math.Round(fixedStatistics.AvarageCost, 2);
                    AvarageDistance = Math.Round(fixedStatistics.AvarageDistance, 2);
                    Distance = fixedStatistics.Distance;
                    TotalCost = Math.Round(fixedStatistics.TotalCost, 2);
                    AvarageCostTenKm = Math.Round(fixedStatistics.AvarageFuelTenKm, 2);

                    if (_selectedMonth == Months.Alltime)
                    {
                        EmployeeStatisticsLabel = _fullName + "\t" + _selectedMonth;
                    }
                    if (_selectedMonth == Months.Year)
                    {
                        if (SelectedYear != 0)
                            EmployeeStatisticsLabel = _fullName + "\t" + SelectedYear;
                        else
                        {
                            EmployeeStatisticsLabel = _fullName;
                        }
                    }
                    if (_selectedMonth > 0 && month < 13)
                    {
                        EmployeeStatisticsLabel = _fullName + "\t" + _selectedMonth + " " + SelectedYear;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                throw;
            }
        }



        public void NewEmployee()
        {
            var window = new AddEmployeeWindow();
            window.DataContext = new AddUserViewModel(new EmployeeModel());
            window.Show();
        }

        public DelegateCommand AddEmployee { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
