using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using TaxiClient.Annotations;
using TaxiClient.Helpers;

namespace TaxiClient.Models
{
    class AddInfoViewModel : INotifyPropertyChanged
    {
        private string _color;
        private string _fuel;
        private string _manufacturer;
        private string _model;
        private bool _metalic;
        private string _label;
        private bool _hybrid;

        public AddInfoViewModel()
        {
            AddColor = new DelegateCommand(x => SaveColor(), x => CanSaveColor());
            AddFuel = new DelegateCommand(x => SaveFuel(), x => CanSaveFuel());
            AddModel = new DelegateCommand(x => SaveModel(), x => CanSaveModel());
            AddManufacturer = new DelegateCommand(x => SaveManufacturer(), x => CanSaveManufacturer());
            
        }

        public string Label
        {
            get { return _label; }
            set
            {
                _label = value;
                OnPropertyChanged();
            }
        }
        public bool Metalic
        {
            get { return _metalic; }
            set
            {
                _metalic = value;
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
                AddColor.RaiseCanExecuteChanged();
            }
        }
        public string Model
        {
            get { return _model; }
            set
            {
                _model = value;
                OnPropertyChanged();
                AddModel.RaiseCanExecuteChanged();
            }
        }
        public string Manufacturer
        {
            get { return _manufacturer; }
            set
            {
                _manufacturer = value;
                OnPropertyChanged();
                AddManufacturer.RaiseCanExecuteChanged();
            }
        }
        public string Fuel
        {
            get { return _fuel; }
            set
            {
                _fuel = value;
                OnPropertyChanged();
                AddFuel.RaiseCanExecuteChanged();
            }
        }

        public bool Hybrid
        {
            get { return _hybrid; }
            set
            {
                _hybrid = value;
                OnPropertyChanged();
            }
        }

        public DelegateCommand AddColor { get; set; }
        public DelegateCommand AddFuel { get; set; }
        public DelegateCommand AddModel { get; set; }
        public DelegateCommand AddManufacturer { get; set; }

        public bool CanSaveColor()
        {
            return Validators.ValidateTextBox(Color);
        }
        public bool CanSaveFuel()
        {
            return Validators.ValidateTextBox(Fuel);
        }
        public bool CanSaveModel()
        {
            return Validators.ValidateTextBox(Model);
        }
        public bool CanSaveManufacturer()
        {
            return Validators.ValidateTextBox(Manufacturer);
        }

        public async void SaveColor()
        {
            var newcolor = new ColorModel() {ColorName = Color,Metallic = Metalic};
            var ismetalic = "";

            var color = newcolor.ColorName;

            if (newcolor.Metallic)
            {
                ismetalic = "Yes";
                color = color + " Metalic";
                newcolor.ColorName = color;

            }
            else
            {
                ismetalic = "No";
            }


           var result = MessageBox.Show("You are about to save a new color.\n\n\tColor: " + color + "\n\tMetalic: " + ismetalic, "Save Color",
                MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                try
                {
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri("http://moggeapi.azurewebsites.net/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Helpers.CredentialHelper.Credential);

                    HttpResponseMessage response = await client.PostAsJsonAsync("api/vehicle/addcolor", newcolor);

                    if (response.IsSuccessStatusCode)
                    {
                        Label = color + " Saved!";
                        Color = "";
                    }
                    if (response.StatusCode == HttpStatusCode.Conflict)
                    {
                        Label = color + " already saved!";
                    }
                    if (response.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        Label = "Problem connecting to database";
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        public async void SaveFuel()
        {
            var fuel = new FuelModel() {FuelType = Fuel};
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://moggeapi.azurewebsites.net/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Helpers.CredentialHelper.Credential);

                var result = MessageBox.Show("You are about to save a new fueltype.\n\n\tFueltype: " + fuel.FuelType, "Save Fueltype",
                   MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)

                {
                    HttpResponseMessage response = await client.PostAsJsonAsync("api/vehicle/addfuel", fuel);

                    if (response.IsSuccessStatusCode)
                    {
                        Label = "Fueltype: " + fuel.FuelType + " saved!";
                        Fuel = "";
                    }
                    if (response.StatusCode == HttpStatusCode.Conflict)
                    {
                        Label = fuel.FuelType + " already saved!";
                    }
                    if (response.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        Label = "Problem connecting to database";
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                throw;
            }
           
        }
        public async void SaveModel()
        {
            try
            {
                var model = new ModelsModel() { ModelType = Model };

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://moggeapi.azurewebsites.net/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Helpers.CredentialHelper.Credential);

                var result = MessageBox.Show("You are about to save a new model.\n\n\tModel: " + model.ModelType, "Save Model",
                   MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {

                    HttpResponseMessage response = await client.PostAsJsonAsync("api/vehicle/addmodel", model);

                    if (response.IsSuccessStatusCode)
                    {
                        Label = "Model: " + model.ModelType + " saved!";
                        Model = "";
                    }
                    if (response.StatusCode == HttpStatusCode.Conflict)
                    {
                        Label = "Model: " + model.ModelType + " already excists!";
                    }
                    if (response.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        Label = "Problem connecting to database";
                    }
                }
            }
            catch (Exception)
            {
                
                throw;
            }
           
        }
        public async void SaveManufacturer()
        {
            try
            {
                var manufacturer = new ManufacturerModel() { ManufactName = Manufacturer };

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://moggeapi.azurewebsites.net/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Helpers.CredentialHelper.Credential);

                var result =
                    MessageBox.Show(
                        "You are about to save a new Manufacturer.\n\n\tManufacturer: " + manufacturer.ManufactName,
                        "Save manufacturer",
                        MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)

                {


                    HttpResponseMessage response = await client.PostAsJsonAsync("api/vehicle/addmanufacturer", manufacturer);

                    if (response.IsSuccessStatusCode)
                    {
                        Label = "Manufacturer: " + manufacturer.ManufactName + " saved!";
                        Manufacturer = "";
                    }
                    if (response.StatusCode == HttpStatusCode.Conflict)
                    {
                        Label = "Manufacturer: " + manufacturer.ManufactName + " already excists!";
                    }
                    if (response.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        Label = "Problem connecting to database";
                    }
                }
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
