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
        private Employee employee;
        internal AppointmentsPage(Employee employee)
        {
            InitializeComponent();
            this.employee = employee;
            cbDoctor.ItemsSource = new MySQLDoctorDAO().GetAll();
            appointmentsViewModel = new GenericDataGridViewModel<Appointment>()
            {
                Items = new ObservableCollection<Appointment>(new MySQLAppointmentDAO().GetAll())
            };
            DataContext = appointmentsViewModel;
        }

        private void Search()
        {
            if (string.IsNullOrEmpty(dpDate.Text) && cbDoctor.SelectedItem == null)
            {
                appointmentsViewModel = new GenericDataGridViewModel<Appointment>()
                {
                    Items = new ObservableCollection<Appointment>(new MySQLAppointmentDAO().GetAll())
                };
                DataContext = appointmentsViewModel;
                return;
            }
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

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (appointmentsViewModel.SelectedItem != null)
                new AppointmentWindow(appointmentsViewModel.SelectedItem).ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Search();
        }

        private void btnAddNewAppointment_Click(object sender, RoutedEventArgs e)
        {
            new AppointmentWindow().ShowDialog();
        }
    }
}
