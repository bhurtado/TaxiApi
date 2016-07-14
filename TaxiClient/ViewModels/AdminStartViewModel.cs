using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TaxiClient.Annotations;
using TaxiClient.Models;

namespace TaxiClient.ViewModels
{
    class AdminStartViewModel : INotifyPropertyChanged
    {
        
        private bool _admin;
        private UserModel _user;
        private string _welcome;

        public AdminStartViewModel(UserModel userModel)
        {
            _user = userModel;
            Admin = _user.IsAdmin;

            Welcome = "Welcome " + _user.Firstname + " " + _user.Lastname;

            Vehicles = new DelegateCommand(x => GetVehicles());
            EditEmployees = new DelegateCommand(x => Employees());
            NewReport = new DelegateCommand(x => OpenNewReport());
            EditReports = new DelegateCommand(x => OpenEditReports());
            OtherCosts = new DelegateCommand(x => OpenOtherCosts());

        }

        public string Welcome
        {
            get { return _welcome; }
            set
            {
                _welcome = value;
                OnPropertyChanged();
            }
        }

        public bool Admin
        {
            get { return _admin; }
            set { _admin = value; }
        }

        public DelegateCommand Vehicles { get; set; }
        public DelegateCommand EditEmployees { get; set; }
        public DelegateCommand NewReport { get; set; }
        public DelegateCommand EditReports { get; set; }
        public DelegateCommand OtherCosts { get; set; }

        public void GetVehicles()
        {
            var window = new VehicleWindow();
            window.DataContext = new VehicleViewModel();
            window.Show();
        }

        public void Employees()
        {
            var window = new EmployeesWindow();
            window.DataContext = new EmployeesViewModel(_user);
            window.Show();
        }

        public void OpenNewReport()
        {
            var window = new NewReportWindow();
            window.DataContext = new NewReportViewModel(_user);
            window.Show();
        }

        public void OpenEditReports()
        {
            var window = new EditReportWindow();
            window.DataContext = new EditReportViewModel(_user);
            window.Show();
        }

        public void OpenOtherCosts()
        {
            var window = new OtherCostsWindow();
            window.DataContext = new OtherCostsViewModel(_user);
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
