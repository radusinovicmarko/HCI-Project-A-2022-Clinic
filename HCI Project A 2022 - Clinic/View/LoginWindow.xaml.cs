using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
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
using HCI_Project_A_2022___Clinic.Data.DataAcces.MySQLDataAccess;
using HCI_Project_A_2022___Clinic.Data.Model;

namespace HCI_Project_A_2022___Clinic.View
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en");
            InitializeComponent();
            Patient p = new Patient()
            {
                PhoneNumber = "x",
                Jmb = "asfasf",
                FirstName = "Jovana",
                LastName = "Miljkovic",
                DateOfBirth = new DateTime(1998, 12, 12),
                Email = "x",
                Address = "x",
                City = new City()
                {
                    CityId = 1,
                    CityName = "Banja Luka",
                    PostCode = "78000"
                }
            };
            //var list = new MySQLPatientDAO().Add(p);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string username = tbUsername.Text;
            string password = pbPassword.Password;
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show(Properties.Resources.CredentialsMissing);
                return;
            }
            if (MySQLUtil.Login(username, password))
            {
                new MainWindow().Show();
                this.Close();
            }
            else
                MessageBox.Show(Properties.Resources.LoginError);
        }
    }
}
