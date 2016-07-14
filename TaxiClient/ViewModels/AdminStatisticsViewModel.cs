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

namespace TaxiClient.ViewModels
{
    class AdminStatisticsViewModel : INotifyPropertyChanged
    {
        private readonly UserModel _user;
        private string _fullName;
        private List<UserModel> _employees;
        private UserModel _selectedEmployee;
        private List<VehicleModel> _vehicles;
        private VehicleModel _selectedVehicle;

        public AdminStatisticsViewModel(UserModel user)
        {
            _user = user;
            FullName = "Logged in as " + _user.Firstname + " " +_user.Lastname;

            GetEmployees();
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

        public UserModel SelectedEmployee
        {
            get { return _selectedEmployee; }
            set
            {
                _selectedEmployee = value;
                GetSelectedEmployeeVehicles(_selectedEmployee.Id);
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

        public VehicleModel SelectedVehicle
        {
            get { return _selectedVehicle; }
            set
            {
                _selectedVehicle = value;
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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
