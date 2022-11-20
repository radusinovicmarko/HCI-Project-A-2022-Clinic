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
        private readonly Patient patient;
        private readonly Employee employee;
        private readonly bool editMode = false;
        private readonly GenericDataGridViewModel<Exam> examViewModel;
        private GenericDataGridViewModel<Appointment> appointmentViewModel;
        private GenericDataGridViewModel<Recovery> recoveriesViewModel;
        private readonly SettingsViewModel settings;
        internal PatientWindow(SettingsViewModel settings, Employee employee)
        {
            InitializeComponent();
            this.settings = settings;
            this.employee = employee;
            patient = new Patient();
            DataContext = new { settings.Theme };
            tabExams.Visibility = Visibility.Collapsed;
            tabAppointments.Visibility = Visibility.Collapsed;
            tabRecoveries.Visibility = Visibility.Collapsed;
            gridPatientData.DataContext = new GenericDataGridViewModel<Patient>()
            {
                SelectedItem = patient,
                Theme = settings.Theme
            };
            try
            {
                cbCity.ItemsSource = new MySQLCityDAO().GetAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Properties.Resources.ErrorMessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        internal PatientWindow(SettingsViewModel settings, Patient patient, Employee employee)
        {
            InitializeComponent();
            this.settings = settings;
            this.patient = patient;
            this.employee = employee;
            editMode = true;
            DataContext = new { settings.Theme };
            if (employee.Role != EmployeeRole.TEHNICAR)
            {
                btnSave.Visibility = Visibility.Collapsed;
                btnCancel.Visibility = Visibility.Collapsed;
                btnAddNewAppointment.Visibility = Visibility.Collapsed;
            }
            if (employee.Role != EmployeeRole.LJEKAR)
            {
                btnAddNewExam.Visibility = Visibility.Collapsed;
                btnAddNewRecovery.Visibility = Visibility.Collapsed;
            }
            try
            {
                cbCity.ItemsSource = new MySQLCityDAO().GetAll();
                cbCity.SelectedItem = patient.City;
                examViewModel = new GenericDataGridViewModel<Exam>()
                {
                    Items = new ObservableCollection<Exam>(new MySQLExamDAO().Get(new Exam()
                    {
                        Patient = patient
                    })),
                    Theme = settings.Theme
                };
                gridExams.DataContext = examViewModel;
                appointmentViewModel = new GenericDataGridViewModel<Appointment>()
                {
                    Items = new ObservableCollection<Appointment>(new MySQLAppointmentDAO().Get(new Appointment()
                    {
                        Patient = patient
                    })),
                    Theme = settings.Theme
                };
                gridAppointments.DataContext = appointmentViewModel;
                recoveriesViewModel = new GenericDataGridViewModel<Recovery>()
                {
                    Items = new ObservableCollection<Recovery>(new MySQLRecoveryDAO().Get(new Recovery()
                    {
                        Patient = patient
                    })),
                    Theme = settings.Theme
                };
                gridRecoveries.DataContext = recoveriesViewModel;
                gridPatientData.DataContext = new GenericDataGridViewModel<Patient>()
                {
                    SelectedItem = patient,
                    Theme = settings.Theme
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Properties.Resources.ErrorMessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnAddNewAppointment_Click(object sender, RoutedEventArgs e)
        {
            if (new AppointmentWindow(settings, patient).ShowDialog().Value)
            {
                try
                {
                    appointmentViewModel = new GenericDataGridViewModel<Appointment>()
                    {
                        Items = new ObservableCollection<Appointment>(new MySQLAppointmentDAO().Get(new Appointment()
                        {
                            Patient = patient
                        })),
                        Theme = settings.Theme
                    };
                    gridAppointments.DataContext = appointmentViewModel;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Properties.Resources.ErrorMessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BtnAddNewRecovery_Click(object sender, RoutedEventArgs e)
        {
            if (new RecoveryWindow(settings, patient, employee as Doctor).ShowDialog().Value)
            {
                try
                {
                    recoveriesViewModel = new GenericDataGridViewModel<Recovery>()
                    {
                        Items = new ObservableCollection<Recovery>(new MySQLRecoveryDAO().Get(new Recovery()
                        {
                            Patient = patient
                        })),
                        Theme = settings.Theme
                    };
                    gridRecoveries.DataContext = recoveriesViewModel;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Properties.Resources.ErrorMessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BtnAddNewExam_Click(object sender, RoutedEventArgs e)
        {
            new ExamWindow(settings, patient, (Doctor)employee).ShowDialog();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(Properties.Resources.ConfirmationDialogContent, Properties.Resources.ConfirmationTitle, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    var dao = new MySQLPatientDAO();
                    if (editMode)
                        dao.Update((int)patient.PersonId, patient);
                    else
                    {
                        patient.DateOfBirth = DateTime.Parse(dpDateOfBirth.Text);
                        patient.City = cbCity.SelectedItem as City;
                        dao.Add(patient);
                    }
                    MessageBox.Show(Properties.Resources.SuccessMessage, Properties.Resources.SuccessMessageTitle, MessageBoxButton.OK);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Properties.Resources.ErrorMessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void DgExams_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (examViewModel.SelectedItem != null)
                new ExamWindow(settings, examViewModel.SelectedItem).ShowDialog();
        }

        private void DgAppointments_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (appointmentViewModel.SelectedItem != null)
                new AppointmentWindow(settings, employee, appointmentViewModel.SelectedItem).ShowDialog();
        }

        private void DgRecoveries_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (recoveriesViewModel.SelectedItem != null)
                new RecoveryWindow(settings, recoveriesViewModel.SelectedItem).ShowDialog();
        }
    }
}
