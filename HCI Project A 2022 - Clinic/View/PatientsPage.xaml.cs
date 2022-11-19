using HCI_Project_A_2022___Clinic.Data.DataAcces.MySQLDataAccess;
using HCI_Project_A_2022___Clinic.Data.Model;
using HCI_Project_A_2022___Clinic.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for PatientsPage.xaml
    /// </summary>
    public partial class PatientsPage : Page
    {
        private GenericDataGridViewModel<Patient> patientsViewModel;
        private readonly Employee employee;
        private readonly SettingsViewModel settings;
        internal PatientsPage(SettingsViewModel settings, Employee employee)
        {
            InitializeComponent();
            this.employee = employee;
            this.settings = settings;
            if (employee.Role != EmployeeRole.TEHNICAR)
                btnAddNewPatient.Visibility = Visibility.Collapsed;
            UpdateDG();
        }

        private void UpdateDG()
        {
            try
            {
                patientsViewModel = new GenericDataGridViewModel<Patient>()
                {
                    Items = new ObservableCollection<Patient>(new MySQLPatientDAO().GetAll()),
                    Theme = settings.Theme
                };
                DataContext = patientsViewModel;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Properties.Resources.ErrorMessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private void Search()
        {
            if (string.IsNullOrEmpty(tbFirstName.Text) && string.IsNullOrEmpty(tbLastName.Text) && string.IsNullOrEmpty(tbJmb.Text))
            {
                UpdateDG();
                return;
            }
            Patient searchPatient = new Patient();
            if (!string.IsNullOrEmpty(tbFirstName.Text))
                searchPatient.FirstName = tbFirstName.Text;
            if (!string.IsNullOrEmpty(tbLastName.Text))
                searchPatient.LastName = tbLastName.Text;
            if (!string.IsNullOrEmpty(tbJmb.Text))
                searchPatient.Jmb = tbJmb.Text;
            try
            {
                patientsViewModel = new GenericDataGridViewModel<Patient>()
                {
                    Items = new ObservableCollection<Patient>(new MySQLPatientDAO().Get(searchPatient)),
                    Theme = settings.Theme
                };
                DataContext = patientsViewModel;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Properties.Resources.ErrorMessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private void Tb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Search();
        }

        private void BtnAddNewPatient_Click(object sender, RoutedEventArgs e)
        {
            new PatientWindow(employee).ShowDialog();
            UpdateDG();
        }

        private void DgPatients_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (patientsViewModel.SelectedItem != null)
                new PatientWindow(patientsViewModel.SelectedItem, employee).ShowDialog();
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            Search();
        }
    }
}
