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

namespace TaxiClient.Models
{

    class GetCVehicleViewModel : INotifyPropertyChanged
    {
        private VehicleModel _vehicle;

        public string RegNr { get; set; }

        public GetCVehicleViewModel()
        {
            Get = new DelegateCommand(x => GetVehicle());
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

        public DelegateCommand Get { get; set; }

        public void GetVehicle()
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Helpers.CredentialHelper.Credential);
                var jsonString = client.GetAsync("http://moggeapi.azurewebsites.net/api/Vehicle/Getvehicle/" + RegNr).Result.Content.ReadAsStringAsync().Result;
                Vehicle = JsonConvert.DeserializeObject<VehicleModel>(jsonString);
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
