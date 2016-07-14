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
using System.Windows.Documents;
using System.Windows.Input;
using Newtonsoft.Json;
using TaxiClient.Helpers;

namespace TaxiClient.Models
{

    class AddVehicleInfoViewModel : INotifyPropertyChanged
    {
        
        private readonly VehicleModel _vehicleModel;

        private VehicleModel _vehicle;

        private List<ColorModel> _colors;
        private List<ModelsModel> _models;
        private List<FuelModel> _fuels;
        private List<ManufacturerModel> _manufacturers;
        private List<FuelModel> _hybridFuels;

        private string _regNr;
        private int _yearModel;
        private int _tripMeter;
        private ColorModel _selectedColor;
        private ManufacturerModel _selectedManufacturer;
        private FuelModel _selectedFuel;
        private ModelsModel _selectedModel;
        private FuelModel _selectedHybridFuel;
        private bool _isHybrid;
        private string _isVehicleSaved;


        public ColorModel SelectedColor
        {
            get { return _selectedColor; }
            set
            {
                _selectedColor = value;
                OnPropertyChanged();
                AddVehicle.RaiseCanExecuteChanged();
            }
        }

        public ManufacturerModel SelectedManufacturer

        {
            get { return _selectedManufacturer; }
            set
            {
                _selectedManufacturer = value;
                OnPropertyChanged();
                AddVehicle.RaiseCanExecuteChanged();
            }
        }

        public FuelModel SelectedFuel
        {
            get { return _selectedFuel; }
            set
            {
                _selectedFuel = value;
                OnPropertyChanged();
                AddVehicle.RaiseCanExecuteChanged();
            }
        }

        public FuelModel SelectedHybridFuel
        {
            get { return _selectedHybridFuel; }
            set
            {
                _selectedHybridFuel = value;
                OnPropertyChanged();
                AddVehicle.RaiseCanExecuteChanged();
            }
        }

        public ModelsModel SelectedModel
        {
            get { return _selectedModel; }
            set
            {
                _selectedModel = value;
                OnPropertyChanged();
                AddVehicle.RaiseCanExecuteChanged();
            }
        }

        public bool IsHybrid
        {
            get { return _isHybrid; }
            set
            {
                _isHybrid = value;
                OnPropertyChanged();
                AddVehicle.RaiseCanExecuteChanged();
            }
        }

        public string RegNr
        {
            get { return _regNr; }
            set
            {
                _regNr = value;
                OnPropertyChanged();
                AddVehicle.RaiseCanExecuteChanged();
            }
        }

        public int TripMeter
        {
            get { return _tripMeter; }
            set
            {
                _tripMeter = value;
                OnPropertyChanged();
                AddVehicle.RaiseCanExecuteChanged();
            }
        }

        public int YearModel
        {
            get { return _yearModel; }
            set
            {
                _yearModel = value;
                OnPropertyChanged();
                AddVehicle.RaiseCanExecuteChanged();
            }
        }

        public string IsVehicleSaved
        {
            get { return _isVehicleSaved; }
            set
            {
                _isVehicleSaved = value;
                OnPropertyChanged();
            }
        }

        public AddVehicleInfoViewModel(VehicleModel vehicleModel)
        {
            if (vehicleModel != null) GetInfo();

            _vehicleModel = vehicleModel;

            //GetAllInfo = new DelegateCommand(x => GetInfo());
            AddVehicle = new DelegateCommand(x => SaveVehicle(), x => CanSaveVehicle());
            AddInfo = new DelegateCommand(x => OpenAddInfoWindow());

            RegNr = vehicleModel.RegNr;
            YearModel = vehicleModel.YearModel;
            TripMeter = vehicleModel.TripMeter;


        }
        public VehicleModel Vehicle
        {
            get { return _vehicle; }
            set
            {
                _vehicle = value;
                OnPropertyChanged();
            }
        }

        public List<ColorModel> Colors
        {
            get { return _colors; }
            set
            {
                _colors = value;
                OnPropertyChanged();
            }

        }

        public List<ModelsModel> Models
        {
            get { return _models; }
            set
            {
                _models = value;
                OnPropertyChanged();
            }
        }

        public List<FuelModel> HybridFuels
        {
            get { return _hybridFuels; }
            set
            {
                _hybridFuels = value;
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

        public List<ManufacturerModel> Manufacturers
        {
            get { return _manufacturers; }
            set
            {
                _manufacturers = value;
                OnPropertyChanged();
            }
        }

        public DelegateCommand AddInfo { get; set; }
        public DelegateCommand AddVehicle { get; set; }

        //public DelegateCommand GetAllInfo { get; set; }

        public void OpenAddInfoWindow()
        {
            var window = new  AddInfoWindow();
            window.DataContext = new AddInfoViewModel();
            window.Show();
        }



        public void GetInfo()
        {
            try
            {
                HttpClient client = new HttpClient();
                var jsonString = client.GetAsync("http://moggeapi.azurewebsites.net/api/vehicle/getvehicleinfo/").Result.Content.ReadAsStringAsync().Result;

                var addVehicleInfoModel = JsonConvert.DeserializeObject<AddVehicleInfoModel>(jsonString);

                Colors = addVehicleInfoModel.Colors;
                Models = addVehicleInfoModel.Models;
                Fuels = addVehicleInfoModel.Fuels;
                Manufacturers = addVehicleInfoModel.Manufacturers;
                HybridFuels = addVehicleInfoModel.Fuels;


            }
            catch (Exception e)
            {

                throw;
            }
        }


        public bool CanSaveVehicle()
        {

            return Validators.ValidateRegNr(RegNr) &&
                   Validators.ValidateYear(YearModel) &&
                   Validators.ValidateTripMeter(TripMeter) &&
                   Validators.ValidateColor(_selectedColor) &&
                   Validators.ValidateFuel(_selectedFuel) &&
                   Validators.ValidateManufacturer(_selectedManufacturer) &&
                   Validators.ValidateModel(_selectedModel)&&
                   Validators.ValidateHybrid(IsHybrid,SelectedHybridFuel,SelectedFuel);

            
        }
       

        public async void SaveVehicle()
        {
            var vehicle = new VehicleModel();
            vehicle.RegNr = _regNr;
            vehicle.YearModel = _yearModel;
            vehicle.TripMeter = _tripMeter;
            vehicle.Color_ID = _selectedColor.Id;
            vehicle.PrimaryFuel_ID = _selectedFuel.Id;
            vehicle.Manufacturer_ID = _selectedManufacturer.Id;
            vehicle.Model_ID = _selectedModel.Id;


            if (IsHybrid)
            {
                vehicle.SecondaryFuel_ID = _selectedHybridFuel.Id;
            }
            else
            {
                vehicle.SecondaryFuel_ID = null;
            }

            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://moggeapi.azurewebsites.net/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Helpers.CredentialHelper.Credential);

                HttpResponseMessage response = await client.PostAsJsonAsync("api/vehicle/addvehicle", vehicle);

                if (response.IsSuccessStatusCode)
                {
                    IsVehicleSaved = "Saved successfully!";
                }
                else
                {
                    IsVehicleSaved = "Something went wrong.";
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                throw;
            }
           


        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


        }
    }
}

