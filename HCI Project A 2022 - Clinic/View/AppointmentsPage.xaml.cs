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
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for AppointmentsPage.xaml
    /// </summary>
    public partial class AppointmentsPage : Page
    {
        private GenericDataGridViewModel<Appointment> appointmentsViewModel;
        private readonly Employee employee;
        internal AppointmentsPage(Employee employee)
        {
            InitializeComponent();
            this.employee = employee;
            if (employee.Role != EmployeeRole.TEHNICAR)
                btnAddNewAppointment.Visibility = Visibility.Collapsed;
            try
            {
                cbDoctor.ItemsSource = new MySQLDoctorDAO().GetAll();
                UpdateDG();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Properties.Resources.ErrorMessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void UpdateDG()
        {
            try
            {
                appointmentsViewModel = new GenericDataGridViewModel<Appointment>()
                {
                    Items = new ObservableCollection<Appointment>(new MySQLAppointmentDAO().GetAll())
                };
                DataContext = appointmentsViewModel;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Properties.Resources.ErrorMessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Search()
        {
            if (string.IsNullOrEmpty(dpDate.Text) && cbDoctor.SelectedItem == null)
            {
                UpdateDG();
                return;
            }
            try
            {
                Appointment searchAppointment = new Appointment();
                if (!string.IsNullOrEmpty(dpDate.Text))
                    searchAppointment.DateTime = DateTime.Parse(dpDate.Text);
                if (cbDoctor.SelectedItem != null)
                    searchAppointment.Doctor = cbDoctor.SelectedItem as Doctor;
                appointmentsViewModel = new GenericDataGridViewModel<Appointment>()
                {
                    Items = new ObservableCollection<Appointment>(new MySQLAppointmentDAO().Get(searchAppointment))
                };
                DataContext = appointmentsViewModel;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Properties.Resources.ErrorMessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (appointmentsViewModel.SelectedItem != null)
                new AppointmentWindow(employee, appointmentsViewModel.SelectedItem).ShowDialog();
        }

        private void BtnAddNewAppointment_Click(object sender, RoutedEventArgs e)
        {
            if (new AppointmentWindow().ShowDialog().Value)
                UpdateDG();
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            Search();
        }
    }
}
