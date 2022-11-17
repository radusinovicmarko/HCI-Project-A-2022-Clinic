using HCI_Project_A_2022___Clinic.Data.DataAcces.MySQLDataAccess;
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
    /// Interaction logic for EmployeeWindow.xaml
    /// </summary>
    public partial class EmployeeWindow : Window
    {
        private Employee employee;
        private bool editMode = false;
        internal EmployeeWindow(EmployeeRole role)
        {
            InitializeComponent();
            cbRole.ItemsSource = Enum.GetValues(typeof(EmployeeRole)).Cast<EmployeeRole>();
            cbRole.SelectedItem = role;
            cbCity.ItemsSource = new MySQLCityDAO().GetAll();
            if (role == EmployeeRole.LJEKAR)
                employee = new Doctor { Role = role };
            else
            {
                employee = new Employee { Role = role };
                tbTitle.Visibility = Visibility.Collapsed;
            }
            DataContext = employee;
        }
        internal EmployeeWindow(Employee employee)
        {
            InitializeComponent();
            this.employee = employee;
            this.editMode = true;
            cbRole.ItemsSource = Enum.GetValues(typeof(EmployeeRole)).Cast<EmployeeRole>();
            cbRole.SelectedItem = Enum.Parse(typeof(EmployeeRole), employee.Role.ToString());
            cbCity.ItemsSource = new MySQLCityDAO().GetAll();
            cbCity.SelectedItem = employee.City;
            if (employee.Role == EmployeeRole.LJEKAR)
                DataContext = new MySQLDoctorDAO().Get(new Doctor() { PersonId = employee.PersonId })[0];
            else
                DataContext = employee;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (editMode)
            {
                if (employee is Doctor doctor)
                    new MySQLDoctorDAO().Update((int)employee.PersonId, doctor);
                else
                    new MySQLEmployeeDAO().Update((int)employee.PersonId, employee);
            }
            else
            {
                employee.DateOfBirth = DateTime.Parse(dpDateOfBirth.Text);
                employee.HireDate = DateTime.Parse(dpHireDate.Text);
                employee.City = cbCity.SelectedItem as City;
                if (employee is Doctor doctor)
                    new MySQLDoctorDAO().Add(doctor);
                else
                    new MySQLEmployeeDAO().Add(employee);
            }
        }
    }
}
