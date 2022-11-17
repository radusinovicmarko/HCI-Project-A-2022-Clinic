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
using System.Windows.Shapes;

namespace HCI_Project_A_2022___Clinic.View
{
    /// <summary>
    /// Interaction logic for PatientWindow.xaml
    /// </summary>
    public partial class PatientWindow : Window
    {
        private Patient patient;
        private Employee employee;
        private bool editMode = false;
        private GenericDataGridViewModel<Exam> examViewModel;
        private GenericDataGridViewModel<Appointment> appointmentViewModel;
        private GenericDataGridViewModel<Recovery> recoveriesViewModel;
        internal PatientWindow(Employee employee)
        {
            InitializeComponent();
            this.employee = employee;
            patient = new Patient();
            tabExams.Visibility = Visibility.Collapsed;
            tabAppointments.Visibility = Visibility.Collapsed;
            tabRecoveries.Visibility = Visibility.Collapsed;
            gridPatientData.DataContext = patient;
            cbCity.ItemsSource = new MySQLCityDAO().GetAll();
        }
        internal PatientWindow(Patient patient, Employee employee)
        {
            InitializeComponent();
            this.patient = patient;
            this.employee = employee;
            editMode = true;
            cbCity.ItemsSource = new MySQLCityDAO().GetAll();
            cbCity.SelectedItem = patient.City;
            examViewModel = new GenericDataGridViewModel<Exam>()
            {
                Items = new ObservableCollection<Exam>(new MySQLExamDAO().Get(new Exam()
                {
                    Patient = patient
                }))
            };
            gridExams.DataContext = examViewModel;
            appointmentViewModel = new GenericDataGridViewModel<Appointment>()
            {
                Items = new ObservableCollection<Appointment>(new MySQLAppointmentDAO().Get(new Appointment()
                {
                    Patient = patient
                }))
            };
            gridAppointments.DataContext = appointmentViewModel;
            recoveriesViewModel = new GenericDataGridViewModel<Recovery>()
            {
                Items = new ObservableCollection<Recovery>(new MySQLRecoveryDAO().Get(new Recovery()
                {
                    Patient = patient
                }))
            };
            gridRecoveries.DataContext = recoveriesViewModel;
            gridPatientData.DataContext = patient;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //TODO: Are you sure?
            var dao = new MySQLPatientDAO();
            if (editMode)
                dao.Update((int)patient.PersonId, patient);
            else
            {
                patient.DateOfBirth = DateTime.Parse(dpDateOfBirth.Text);
                patient.City = cbCity.SelectedItem as City;
                dao.Add(patient);
            }
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (appointmentViewModel.SelectedItem != null)
                    new AppointmentWindow(appointmentViewModel.SelectedItem).ShowDialog();
        }

        private void BtnAddNewAppointment_Click(object sender, RoutedEventArgs e)
        {
            if (new AppointmentWindow(patient).ShowDialog().Value)
            {
                appointmentViewModel = new GenericDataGridViewModel<Appointment>()
                {
                    Items = new ObservableCollection<Appointment>(new MySQLAppointmentDAO().Get(new Appointment()
                    {
                        Patient = patient
                    }))
                };
                gridAppointments.DataContext = appointmentViewModel;
            }
        }

        private void DataGrid_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            if (recoveriesViewModel.SelectedItem != null)
                new RecoveryWindow(recoveriesViewModel.SelectedItem).ShowDialog();
        }

        private void btnAddNewRecovery_Click(object sender, RoutedEventArgs e)
        {
            if (new RecoveryWindow(patient, employee as Doctor).ShowDialog().Value)
            {
                recoveriesViewModel = new GenericDataGridViewModel<Recovery>()
                {
                    Items = new ObservableCollection<Recovery>(new MySQLRecoveryDAO().Get(new Recovery()
                    {
                        Patient = patient
                    }))
                };
                gridRecoveries.DataContext = recoveriesViewModel;
            }
        }

        private void DataGrid_MouseDoubleClick_2(object sender, MouseButtonEventArgs e)
        {
            if (examViewModel.SelectedItem != null)
                new ExamWindow(examViewModel.SelectedItem).ShowDialog();
        }

        private void BtnAddNewExam_Click(object sender, RoutedEventArgs e)
        {
            new ExamWindow(patient, (Doctor)employee).ShowDialog();
        }
    }
}
