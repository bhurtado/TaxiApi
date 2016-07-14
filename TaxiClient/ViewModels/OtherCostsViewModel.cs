using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using Newtonsoft.Json;
using TaxiClient.Models;
using TaxiClient.Annotations;

namespace TaxiClient.ViewModels
{
    class OtherCostsViewModel : INotifyPropertyChanged
    {
        private UserModel _user;
        private List<VehicleModel> _vehicles;
        private VehicleModel _vehicle;
        private OtherCostsModel _selecfedCost;
        private List<OtherCostsModel> _costs;
        private string _groupBoxLabel;
        private decimal _costTextBox;
        private string _maintenanceTypTextBox;
        private int _maintenanceSelectedIndex;
        private string _savedOrNotLabel;

        public OtherCostsViewModel(UserModel user)
        {
            _user = user;

            GetVehicles();

            AddCost = new DelegateCommand(x => AddMaintenance(), x => CanAddMaintenance());
            Clear = new DelegateCommand(x => ClearOtherCostModel());
            Delete = new DelegateCommand(x => DeleteCost(),x => CanDeleteCost());
        }

        public int MaintenanceSelectedIndex
        {
            get { return _maintenanceSelectedIndex; }
            set
            {
                _maintenanceSelectedIndex = value;
                OnPropertyChanged();
            }
        }

        public string GroupBoxLabel
        {
            get { return _groupBoxLabel; }
            set
            {
                _groupBoxLabel = value;
                OnPropertyChanged();
            }
        }

        public decimal CostTextBox
        {
            get { return _costTextBox; }
            set
            {
                _costTextBox = value;
                OnPropertyChanged();
                AddCost.RaiseCanExecuteChanged();
            }
        }

        public string MaintenanceTypTextBox
        {
            get { return _maintenanceTypTextBox; }
            set
            {
                _maintenanceTypTextBox = value;
                OnPropertyChanged();
                AddCost.RaiseCanExecuteChanged();
            }
        }

        public string SavedOrNotLabel
        {
            get { return _savedOrNotLabel; }
            set
            {
                _savedOrNotLabel = value;
                OnPropertyChanged();
            }
        }

        public VehicleModel Vehicle
        {
            get { return _vehicle; }
            set
            {
                _vehicle = value;
                GetMaintenance();
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

        public OtherCostsModel SelectedCost
        {
            get { return _selecfedCost; }
            set
            {
                _selecfedCost = value;
                if (_selecfedCost != null)
                {
                    MaintenanceTypTextBox = _selecfedCost.MaintenanceType;
                    CostTextBox = _selecfedCost.Cost;
                    GroupBoxLabel = _vehicle.RegNr + " " + _selecfedCost.MaintenanceType;
                }
                OnPropertyChanged();
                Delete.RaiseCanExecuteChanged();

            }
        }

        public List<OtherCostsModel> Costs
        {
            get { return _costs; }
            set
            {
                _costs = value;
                OnPropertyChanged();
            }
        }

        public DelegateCommand Delete { get; set; }
        public DelegateCommand AddCost { get; set; }
        public DelegateCommand Clear { get; set; }
       

        public void GetVehicles()
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Helpers.CredentialHelper.Credential);

                string getVehicles = $"http://moggeapi.azurewebsites.net/api/vehicle/getvehicles";

                var jsonVehicles = client.GetAsync(getVehicles).Result.Content.ReadAsStringAsync().Result;
                var vehicles = JsonConvert.DeserializeObject<List<VehicleModel>>(jsonVehicles);
                Vehicles = vehicles;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                throw;
            }
          
        }

        public void GetMaintenance()
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Helpers.CredentialHelper.Credential);
                string getCosts = $"http://moggeapi.azurewebsites.net/api/maintenance/getmaintenance/{_vehicle.RegNr}";
                var jsonCosts = client.GetAsync(getCosts).Result.Content.ReadAsStringAsync().Result;
                var costs = JsonConvert.DeserializeObject<List<OtherCostsModel>>(jsonCosts);
                Costs = costs;

                MaintenanceSelectedIndex = 0;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                throw;
            }
           
        }

        public bool CanAddMaintenance()
        {
            if (!String.IsNullOrWhiteSpace(MaintenanceTypTextBox) && CostTextBox > 0)
                return true;
            return false;
        }

        public async void AddMaintenance()
        {
            var costToSave = new OtherCostsModel();
            var dialogResult = new MessageBoxResult();

            if (SelectedCost != null)
            {
                costToSave = SelectedCost;
                costToSave.Cost = CostTextBox;
                costToSave.MaintenanceType = MaintenanceTypTextBox;
            }
            else
            {
                costToSave.Cost = CostTextBox;
                costToSave.MaintenanceType = MaintenanceTypTextBox;
                costToSave.ReportDate = DateTime.Now;
                costToSave.Vehicle_Id = Vehicle.RegNr;
                costToSave.Id = null;

            }
            if (SelectedCost == null)
            {
                dialogResult = MessageBox.Show($"You are about to ADD a new Cost, is this right?\n\n Maintenance: \t {MaintenanceTypTextBox} \n Cost: \t\t {CostTextBox}", "Add New Cost?",
                    MessageBoxButton.YesNo);
               
            }
            else
            {
                dialogResult = MessageBox.Show($"You are about to EDIT the selected cost, is this right?\n\n Maintenance:\t {MaintenanceTypTextBox} \n Cost:\t\t {CostTextBox} " , "EDIT cost",
                    MessageBoxButton.YesNo);
            }
            if (dialogResult == MessageBoxResult.Yes)
            {
                try
                {
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Helpers.CredentialHelper.Credential);
                    client.BaseAddress = new Uri("http://moggeapi.azurewebsites.net/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await client.PostAsJsonAsync("api/maintenance/addmaintenance/", costToSave);

                    if (response.IsSuccessStatusCode)
                    {
                        SavedOrNotLabel = "It Worked!!";
                    }
                    else
                    {
                        SavedOrNotLabel = "Oh No. It didn´t work!";
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                    throw;
                }
              
            }

        }

        public bool CanDeleteCost()
        {
            if(_selecfedCost != null)
            return true;
            return false;
        }

        public async void DeleteCost()
        {
            var dialogResult = MessageBox.Show($"You are about to DELETE selected Maintenance, is this right? \n\n Date:\t\t{_selecfedCost.ReportDate} \n Vehicle:\t\t{_selecfedCost.Vehicle_Id}\n Maintenance:\t{_selecfedCost.MaintenanceType}\n Cost:\t\t{_selecfedCost.Cost}", "Delete Maintenance?", MessageBoxButton.YesNo);

            int id = _selecfedCost.Id.Value;

            if (dialogResult == MessageBoxResult.Yes)
            {
                try
                {
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                        Helpers.CredentialHelper.Credential);

                    client.BaseAddress = new Uri("http://moggeapi.azurewebsites.net/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await client.DeleteAsync($"api/maintenance/deletemaintenance/{id}");
                    if (response.IsSuccessStatusCode)
                    {
                        SavedOrNotLabel = "Deleted!";
                    }
                    else
                    {
                        SavedOrNotLabel = "Hmm, Not deleted...";
                    }
                }
                catch (Exception)
                {
                    SavedOrNotLabel = "Oops! Server Error!";
                    throw;
                }
            }

        }

        public void ClearOtherCostModel()
        {
            SelectedCost = null;
            GroupBoxLabel = "Add new cost to " + _vehicle.RegNr;
            CostTextBox = 0;
            MaintenanceTypTextBox = "";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
