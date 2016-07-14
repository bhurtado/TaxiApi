using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using TaxiClient.Annotations;
using TaxiClient.Helpers;


namespace TaxiClient.Models
{
     
    class AddUserViewModel : INotifyPropertyChanged
    {
        private readonly EmployeeModel _employee;

        private string _firstName;
        private string _userName;
        private string _lastName;
        private string _password;
        private bool _isAdmin;

        public AddUserViewModel(EmployeeModel employee)
        {
            _employee = employee;

            AddUser = new DelegateCommand(x => SaveUser(), x => CanAddUser());

            UserName = _userName;
            FirstName = _firstName;
            LastName = _lastName;

        }
       
        public DelegateCommand AddUser { get; set; }

        public bool CanAddUser()
        {
            return Validators.ValidateUserNameLength(UserName) &&
                   Validators.ValidateName(FirstName) &&
                   Validators.ValidateName(LastName);

        }

        public async void SaveUser()
        {
          
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

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                OnPropertyChanged();
                AddUser.RaiseCanExecuteChanged();
            }
        }

        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                OnPropertyChanged();
                AddUser.RaiseCanExecuteChanged();
                
            }
        }

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                OnPropertyChanged();
                AddUser.RaiseCanExecuteChanged();
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
