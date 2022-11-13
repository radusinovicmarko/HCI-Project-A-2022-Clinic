using HCI_Project_A_2022___Clinic.Data.Model;
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
        internal MainWindow(Employee employee)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("sr");
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
    }
}
