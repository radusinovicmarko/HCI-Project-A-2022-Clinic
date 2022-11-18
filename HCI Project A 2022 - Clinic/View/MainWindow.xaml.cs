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
        private Employee employee;
        private SettingsViewModel settings;
        internal MainWindow(SettingsViewModel settings, Employee employee)
        {
            this.settings = settings;
            InitializeComponent();
            this.employee = employee;
            frameMain.Content = new PatientsPage(employee);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Are you sure?
            new LoginWindow().Show();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (!(frameMain.Content is PatientsPage))
            {
                frameMain.Content = null;
                frameMain.NavigationService.RemoveBackEntry();
                frameMain.Content = new PatientsPage(employee);
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (!(frameMain.Content is AppointmentsPage))
            {
                frameMain.Content = null;
                frameMain.NavigationService.RemoveBackEntry();
                frameMain.Content = new AppointmentsPage(employee);
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (!(frameMain.Content is EmployeesPage))
            {
                frameMain.Content = null;
                frameMain.NavigationService.RemoveBackEntry();
                frameMain.Content = new EmployeesPage(employee);
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (!(frameMain.Content is ExamsPage))
            {
                frameMain.Content = null;
                frameMain.NavigationService.RemoveBackEntry();
                frameMain.Content = new ExamsPage(employee);
            }
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            if (!(frameMain.Content is SettingsPage))
            {
                frameMain.Content = null;
                frameMain.NavigationService.RemoveBackEntry();
                frameMain.Content = new SettingsPage(settings, employee);
            }
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            if (!(frameMain.Content is CodebooksPage))
            {
                frameMain.Content = null;
                frameMain.NavigationService.RemoveBackEntry();
                frameMain.Content = new CodebooksPage(settings, employee);
            }
        }
    }
}
