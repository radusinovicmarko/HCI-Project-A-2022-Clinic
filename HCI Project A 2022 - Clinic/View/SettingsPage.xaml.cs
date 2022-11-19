using HCI_Project_A_2022___Clinic.Data.DataAcces.MySQLDataAccess;
using HCI_Project_A_2022___Clinic.Data.Model;
using HCI_Project_A_2022___Clinic.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HCI_Project_A_2022___Clinic.View
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        private readonly Employee employee;
        private readonly SettingsViewModel settings;
        internal SettingsPage(SettingsViewModel settings, Employee employee)
        {
            InitializeComponent();
            this.settings = settings;
            this.employee = employee;
            if (settings.Language == "sr")
                rbSr.IsChecked = true;
            else
                rbEn.IsChecked = true;
            spCredentials.DataContext = employee;
            pbPassword.Password = employee.Password;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                employee.Username = tbUsername.Text;
                employee.Password = pbPassword.Password;
                new MySQLEmployeeDAO().Update(employee.PersonId.Value, employee);
                if (rbSr.IsChecked.Value)
                    settings.Language = "sr";
                else
                    settings.Language = "en";
                File.WriteAllText("settings.json", JsonConvert.SerializeObject(settings));
                MessageBox.Show(Properties.Resources.SuccessMessage, Properties.Resources.SuccessMessageTitle, MessageBoxButton.OK);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Properties.Resources.ErrorMessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
