using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TaxiClient.Models;
using TaxiClient.ViewModels;

namespace TaxiClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Login();
            //AddUser();
            //AddVehicle();
            //Vehicles();
            //DriverLogin();
            //AddInfo();
        }

        public void AddVehicle()
        {
            var window = new AddVehicleWindow1();
            window.Show();
        }

        public void Login()
        {
            var window = new LoginCBWindow();
            window.Show();
        }

        public void AddUser()
        {
            var window = new AddEmployeeWindow();
            window.Show();
        }

        public void Vehicles()
        {
            var window = new AdminStartWindow();
            window.DataContext = new AdminStartViewModel(new UserModel());
            window.Show();
        }

        public void DriverLogin()
        {
            var window = new DriverWindow();
            window.DataContext = new DriverViewModel(new UserModel() {IsAdmin = false, Id = 9999, Firstname = "Charlie", Lastname = "Hepbo"});
            window.Show();
        }

        public void AddInfo()
        {
            var window = new AddInfoWindow();
            window.DataContext = new AddInfoViewModel();
            window.Show();
        }
    }
}
