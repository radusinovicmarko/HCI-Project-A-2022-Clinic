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
        private PatientsViewModel patientsViewModel;
        public PatientsPage()
        {
            InitializeComponent();
            patientsViewModel = new PatientsViewModel()
            {
                Patients = new ObservableCollection<Patient>(new MySQLPatientDAO().GetAll())
            };
            DataContext = patientsViewModel;
        }

        private void Search()
        {
            if (string.IsNullOrEmpty(tbFirstName.Text) && string.IsNullOrEmpty(tbLastName.Text) && string.IsNullOrEmpty(tbJmb.Text))
            {
                patientsViewModel = new PatientsViewModel()
                {
                    Patients = new ObservableCollection<Patient>(new MySQLPatientDAO().GetAll())
                };
                DataContext = patientsViewModel;
                return;
            }
            Patient searchPatient = new Patient();
            if (!string.IsNullOrEmpty(tbFirstName.Text))
                searchPatient.FirstName = tbFirstName.Text;
            if (!string.IsNullOrEmpty(tbLastName.Text))
                searchPatient.LastName = tbLastName.Text;
            if (!string.IsNullOrEmpty(tbJmb.Text))
                searchPatient.Jmb = tbJmb.Text;
            patientsViewModel = new PatientsViewModel()
            {
                Patients = new ObservableCollection<Patient>(new MySQLPatientDAO().Get(searchPatient))
            };
            DataContext = patientsViewModel;
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (patientsViewModel.SelectedPatient != null)
                MessageBox.Show(patientsViewModel.SelectedPatient.FirstName + " " + patientsViewModel.SelectedPatient.LastName);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Search();
        }

        private void Tb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Search();
        }
    }
}
