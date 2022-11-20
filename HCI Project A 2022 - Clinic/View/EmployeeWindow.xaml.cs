using HCI_Project_A_2022___Clinic.Data.DataAcces.MySQLDataAccess;
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
    /// Interaction logic for EmployeeWindow.xaml
    /// </summary>
    public partial class EmployeeWindow : Window
    {
        private readonly Employee employee;
        private readonly SettingsViewModel settings;
        private readonly bool editMode = false;
        internal EmployeeWindow(SettingsViewModel settings, EmployeeRole role)
        {
            InitializeComponent();
            this.settings = settings;
            try
            {
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
                DataContext = new GenericDataGridViewModel<Employee>()
                {
                    SelectedItem = employee,
                    Theme = settings.Theme
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Properties.Resources.ErrorMessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        internal EmployeeWindow(SettingsViewModel settings, Employee employee)
        {
            InitializeComponent();
            this.settings = settings;
            try
            {
                this.employee = employee;
                editMode = true;
                tbPassword.Visibility = Visibility.Collapsed;
                tbUsername.IsEnabled = false;
                cbRole.ItemsSource = Enum.GetValues(typeof(EmployeeRole)).Cast<EmployeeRole>();
                cbRole.SelectedItem = Enum.Parse(typeof(EmployeeRole), employee.Role.ToString());
                cbCity.ItemsSource = new MySQLCityDAO().GetAll();
                cbCity.SelectedItem = employee.City;
                if (employee.Role == EmployeeRole.LJEKAR)
                    DataContext = new GenericDataGridViewModel<Doctor>()
                    {
                        SelectedItem = new MySQLDoctorDAO().Get(new Doctor() { PersonId = employee.PersonId })[0],
                        Theme = settings.Theme
                    };
                else
                    DataContext = new GenericDataGridViewModel<Employee>()
                    {
                        SelectedItem = employee,
                        Theme = settings.Theme
                    };
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Properties.Resources.ErrorMessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(Properties.Resources.ConfirmationDialogContent, Properties.Resources.ConfirmationTitle, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
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
                    MessageBox.Show(Properties.Resources.SuccessMessage, Properties.Resources.SuccessMessageTitle, MessageBoxButton.OK);
                    DialogResult = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Properties.Resources.ErrorMessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                }
                Close();
            }
        }
    }
}
