using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Newtonsoft.Json;
using TaxiClient.Models;
using TaxiClient.ViewModels;

namespace TaxiClient
{
    /// <summary>
    /// Interaction logic for LoginCBWindow.xaml
    /// </summary>
    public partial class LoginCBWindow : Window
    {
        public LoginCBWindow()
        {
            InitializeComponent();
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
           //var loginModel = new LoginModel() {Username = txtUserName.Text, Password = pwbPassword.Password};

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://moggeapi.azurewebsites.net/");
            //client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            Helpers.CredentialHelper.Credential = Convert.ToBase64String(Encoding.UTF8.GetBytes(txtUserName.Text + ":" + pwbPassword.Password));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",Helpers.CredentialHelper.Credential);

            var response =  await client.GetAsync("api/user/getaccesslevel");

            if (response.IsSuccessStatusCode)
            {
                var user = JsonConvert.DeserializeObject<UserModel>(await response.Content.ReadAsStringAsync());

                if (user.IsAdmin)
                {
                    var window = new AdminStartWindow();
                    window.DataContext = new AdminStartViewModel(user);
                    window.Show();
                    this.Close();
                }
                else
                {
                    var window = new DriverWindow();
                    window.DataContext = new DriverViewModel(user);
                    window.Show();
                    this.Close();
                }
            }
        }
    }
}
