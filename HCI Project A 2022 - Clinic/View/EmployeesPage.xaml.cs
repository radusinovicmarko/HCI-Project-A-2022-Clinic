using HCI_Project_A_2022___Clinic.Data.DataAcces.MySQLDataAccess;
using HCI_Project_A_2022___Clinic.Data.Model;
using HCI_Project_A_2022___Clinic.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for EmployeesPage.xaml
    /// </summary>
    public partial class EmployeesPage : Page
    {
        private GenericDataGridViewModel<Employee> employeesViewModel;
        private Employee employee;
        internal EmployeesPage(Employee employee)
        {
            InitializeComponent();
            this.employee = employee;
            cbRole.ItemsSource = Enum.GetValues(typeof(EmployeeRole)).Cast<EmployeeRole>();
            employeesViewModel = new GenericDataGridViewModel<Employee>()
            {
                Items = new ObservableCollection<Employee>(new MySQLEmployeeDAO().GetAll())
            };
            DataContext = employeesViewModel;
        }

        private void Search()
        {
            if (string.IsNullOrEmpty(tbFirstName.Text) && string.IsNullOrEmpty(tbLastName.Text) 
                && string.IsNullOrEmpty(tbJmb.Text) && cbRole.SelectedItem == null)
            {
                employeesViewModel = new GenericDataGridViewModel<Employee>()
                {
                    Items = new ObservableCollection<Employee>(new MySQLEmployeeDAO().GetAll())
                };
                DataContext = employeesViewModel;
                return;
            }
            Employee searchEmployee = new Employee();
            if (!string.IsNullOrEmpty(tbFirstName.Text))
                searchEmployee.FirstName = tbFirstName.Text;
            if (!string.IsNullOrEmpty(tbLastName.Text))
                searchEmployee.LastName = tbLastName.Text;
            if (!string.IsNullOrEmpty(tbJmb.Text))
                searchEmployee.Jmb = tbJmb.Text;
            if (cbRole.SelectedItem != null)
                searchEmployee.Role = (EmployeeRole?) Enum.Parse(typeof(EmployeeRole), cbRole.SelectedItem.ToString());
            employeesViewModel = new GenericDataGridViewModel<Employee>()
            {
                Items = new ObservableCollection<Employee>(new MySQLEmployeeDAO().Get(searchEmployee))
            };
            DataContext = employeesViewModel;
        }

        private void Tb_KeyDown(object sender, KeyEventArgs e)
        {
            Search();
        }

        private void BtnAddNewEmployee_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Search();
        }
    }
}
