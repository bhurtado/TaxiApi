using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TaxiClient.Models;
using TaxiClient.Annotations;

namespace TaxiClient.ViewModels
{
    class DriverViewModel : INotifyPropertyChanged
    {
        private readonly UserModel _user;
        private string _driverName;

        public DriverViewModel(UserModel user)
        {
            _user = user;

            NewReport = new DelegateCommand(x => OpenNewReport());
            MyReports = new DelegateCommand(x => OpenMyReports(user));

            DriverName = "Welcome " + _user.Firstname + " " + _user.Lastname;

        }

        public string DriverName
        {
            get { return _driverName; }
            set
            {
                _driverName = value;
            }
        }


        public DelegateCommand NewReport { get; set; }
        public DelegateCommand MyReports { get; set; }

        public void OpenNewReport()
        {
            var window = new NewReportWindow();
            window.DataContext = new NewReportViewModel(_user);
            window.Show();
        }

        public void OpenMyReports(UserModel user)
        {
            var window = new DriverStatisticsWindow();
            window.DataContext = new DriverStatisticViewModel(user);
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
