using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.RightsManagement;
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
using TaxiClient.Helpers;
using TaxiClient.Models;

namespace TaxiClient
{
    /// <summary>
    /// Interaction logic for AddEmployeeWindow.xaml
    /// </summary>
    public partial class AddEmployeeWindow : Window
    {
        public AddEmployeeWindow()
        {
            InitializeComponent();
            DataContext = new AddUserViewModel(new EmployeeModel());
        }

        private async void btnAddEmployee_Click(object sender, RoutedEventArgs e)
        {

            if (pwbPassword.Password != pwbConfirmPword.Password)
            {
                MessageBox.Show("Confirm Password doesn´t match your password!");
                pwbConfirmPword.BorderBrush = Brushes.Red;
            }
            else
            {
                var employee = new EmployeeModel();
                employee.FirstName = txtFirstName.Text;
                employee.LastName = txtLastName.Text;
                employee.UserName = txtUserName.Text;
                employee.Password = pwbPassword.Password;
                employee.IsAdmin = ckbAdmin.IsChecked.Value.Equals(true);

                try
                {
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Helpers.CredentialHelper.Credential);
                    client.BaseAddress = new Uri("http://moggeapi.azurewebsites.net/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                  
                        var respone = await client.PostAsJsonAsync("api/user/adduser", employee);

                        if (respone.IsSuccessStatusCode)
                        {
                            lblStatus.Content = "User successfully added!.";
                        }
                        else
                        {
                            lblStatus.Content = "Something went wrong.";
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
              
               
            }
        }

        private void PwbPassword_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
           
            var b = new AddUserViewModel(new EmployeeModel());
            b.Password = pwbPassword.Password;
        }
    }
}
