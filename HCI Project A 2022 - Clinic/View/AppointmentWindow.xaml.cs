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
    /// Interaction logic for AppointmentWindow.xaml
    /// </summary>
    public partial class AppointmentWindow : Window
    {
        private Appointment appointment;
        public AppointmentWindow()
        {
            InitializeComponent();
            appointment = new Appointment();
            cbDoctor.ItemsSource = new MySQLDoctorDAO().GetAll();
            gridAppointment.DataContext = appointment;
        }
        internal AppointmentWindow(Patient patient)
        {
            InitializeComponent();
            appointment = new Appointment()
            {
                Patient = patient
            };
            cbDoctor.ItemsSource = new MySQLDoctorDAO().GetAll();
            gridAppointment.DataContext = appointment;
        }
        internal AppointmentWindow(Appointment appointment)
        {
            InitializeComponent();
            this.appointment = appointment;
            cbDoctor.ItemsSource = new MySQLDoctorDAO().GetAll();
            cbDoctor.SelectedItem = appointment.Doctor;
            gridAppointment.DataContext = appointment;
            btnSave.Visibility = Visibility.Collapsed;
            btnNewExam.Visibility = Visibility.Visible;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            appointment.Doctor = cbDoctor.SelectedItem as Doctor;
            appointment.DateTime = DateTime.Parse(dpDate.Text + " " + tpTime.Text);
            new MySQLAppointmentDAO().Add(appointment);
            DialogResult = true;
            Close();
        }

        private void BtnNewExam_Click(object sender, RoutedEventArgs e)
        {
            new ExamWindow(appointment.Patient, appointment.Doctor).ShowDialog();
        }
    }
}
