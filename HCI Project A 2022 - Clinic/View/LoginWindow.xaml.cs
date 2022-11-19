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
using HCI_Project_A_2022___Clinic.Util;
using HCI_Project_A_2022___Clinic.ViewModel;

namespace HCI_Project_A_2022___Clinic.View
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly SettingsViewModel settings;
        public LoginWindow()
        {
            settings = Utils.LoadSettings();
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(settings.Language);
            InitializeComponent();
        }

        private void Login()
        {
            string username = tbUsername.Text;
            string password = pbPassword.Password;
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show(Properties.Resources.CredentialsMissing, Properties.Resources.ErrorMessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                var loginInfo = MySQLUtil.Login(username, password);
                if (loginInfo.Item1)
                {
                    Employee employee = new MySQLEmployeeDAO().Get(new Employee() { PersonId = loginInfo.Item2 })[0];
                    if (employee.Role == EmployeeRole.LJEKAR)
                        employee = new MySQLDoctorDAO().Get(new Doctor() { PersonId = loginInfo.Item2 })[0];
                    new MainWindow(settings, employee).Show();
                    Close();
                }
                else
                    MessageBox.Show(Properties.Resources.LoginError, Properties.Resources.ErrorMessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Properties.Resources.ErrorMessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TbUsername_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Login();
        }

        private void PbPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Login();
        }

        private void BtnUsername_Click(object sender, RoutedEventArgs e)
        {
            Login();
        }
    }
}
