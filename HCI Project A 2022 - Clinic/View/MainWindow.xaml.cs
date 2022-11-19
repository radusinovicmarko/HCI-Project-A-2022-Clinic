using HCI_Project_A_2022___Clinic.Data.Model;
using HCI_Project_A_2022___Clinic.ViewModel;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace HCI_Project_A_2022___Clinic.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Employee employee;
        private readonly SettingsViewModel settings;
        internal MainWindow(SettingsViewModel settings, Employee employee)
        {
            this.settings = settings;
            this.employee = employee;
            InitializeComponent();
            if (employee.Role != EmployeeRole.ADMIN)
                btnAdministration.Visibility = Visibility.Collapsed;
            frameMain.Content = new PatientsPage(employee);
        }
        private void ClearFrame()
        {
            frameMain.Content = null;
            frameMain.NavigationService.RemoveBackEntry();
        }

        private void BtnPatients_Click(object sender, RoutedEventArgs e)
        {
            if (!(frameMain.Content is PatientsPage))
            {
                ClearFrame();
                frameMain.Content = new PatientsPage(employee);
            }
        }

        private void BtnAppointments_Click(object sender, RoutedEventArgs e)
        {
            if (!(frameMain.Content is AppointmentsPage))
            {
                ClearFrame();
                frameMain.Content = new AppointmentsPage(employee);
            }
        }

        private void BtnExams_Click(object sender, RoutedEventArgs e)
        {
            if (!(frameMain.Content is ExamsPage))
            {
                ClearFrame();
                frameMain.Content = new ExamsPage(employee);
            }
        }

        private void BtnCodebooks_Click(object sender, RoutedEventArgs e)
        {
            if (!(frameMain.Content is CodebooksPage))
            {
                ClearFrame();
                frameMain.Content = new CodebooksPage(settings, employee);
            }
        }

        private void BtnAdministration_Click(object sender, RoutedEventArgs e)
        {
            if (!(frameMain.Content is EmployeesPage))
            {
                ClearFrame();
                frameMain.Content = new EmployeesPage(employee);
            }
        }

        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {
            if (!(frameMain.Content is SettingsPage))
            {
                ClearFrame();
                frameMain.Content = new SettingsPage(settings, employee);
            }
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(Properties.Resources.ConfirmationDialogContent, Properties.Resources.ConfirmationTitle, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                new LoginWindow().Show();
                Close();
            }
        }
    }
}
