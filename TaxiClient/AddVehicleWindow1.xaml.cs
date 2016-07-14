using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using TaxiClient.Models;

namespace TaxiClient
{
    /// <summary>
    /// Interaction logic for AddVehicleWindow1.xaml
    /// </summary>
    /// 
    /// 
    
    public partial class AddVehicleWindow1 : Window
    {
        public AddVehicleWindow1()
        {
            InitializeComponent();
            this.DataContext = new AddVehicleInfoViewModel(new VehicleModel());
        }
    }
}
