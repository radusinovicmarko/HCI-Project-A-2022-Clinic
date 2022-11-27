using HCI_Project_A_2022___Clinic.Data.DataAcces.MySQLDataAccess;
using HCI_Project_A_2022___Clinic.Data.Model;
using HCI_Project_A_2022___Clinic.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
    /// Interaction logic for ExamWindow.xaml
    /// </summary>
    public partial class ExamWindow : Window
    {
        private readonly Exam exam;
        private readonly SettingsViewModel settings;
        private Prescription prescription;
        private Referral referral;
        internal ExamWindow(SettingsViewModel settings, Doctor doctor)
        {
            InitializeComponent();
            this.settings = settings;
            DataContext = settings;
            exam = new Exam() { Doctor = doctor };
            Load();
            cbDoctor.SelectedItem = doctor;
        }

        internal ExamWindow(SettingsViewModel settings, Patient patient, Doctor doctor)
        {
            InitializeComponent();
            this.settings = settings;
            DataContext = settings;
            exam = new Exam()
            {
                Doctor = doctor,
                Patient = patient
            };
            Load();
            cbDoctor.SelectedItem = doctor;
        }
        internal ExamWindow(SettingsViewModel settings, Exam exam)
        {
            InitializeComponent();
            this.settings = settings;
            this.exam = exam;
            DataContext = settings;
            Load();
            cbDoctor.SelectedItem = exam.Doctor;
            cbExamType.SelectedItem = exam.ExamType;
            try
            {
                var prescriptions = new MySQLPrescriptionDAO().Get(new Prescription() { Exam = new Exam() { ExamId = exam.ExamId } });
                if (prescriptions.Count > 0)
                {
                    gridPrescription.DataContext = new GenericDataGridViewModel<Prescription>()
                    {
                        SelectedItem = prescriptions[0],
                        Theme = settings.Theme
                    };
                    cbMedication.SelectedItem = prescriptions[0].Medication;
                    cbPrescription.IsChecked = true;
                }
                var referrals = new MySQLReferralDAO().Get(new Referral() { Exam = new Exam() { ExamId = exam.ExamId } });
                if (referrals.Count > 0)
                {
                    gridReferral.DataContext = new GenericDataGridViewModel<Referral>()
                    {
                        SelectedItem = referrals[0],
                        Theme = settings.Theme
                    };
                    cbReferral.IsChecked = true;
                }
                ConfigureEditMode();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Properties.Resources.ErrorMessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ConfigureEditMode()
        {
            tbDiagnosisCode.IsEnabled = false;
            cbExamType.IsEnabled = false;
            tbReport.IsEnabled = false;
            cbPrescription.IsEnabled = false;
            cbMedication.IsEnabled = false;
            tbInstruction.IsEnabled = false;
            tbJmb.IsEnabled = false;
            cbDoctor.IsEnabled = false;
            cbReferral.IsEnabled = false;
            tbInstitutionName.IsEnabled = false;
            tbInstitutionCode.IsEnabled = false;
            tbReferralType.IsEnabled = false;
            btnSave.Visibility = Visibility.Collapsed;
        }

        private void Load()
        {
            try
            {
                prescription = new Prescription();
                referral = new Referral();
                cbDoctor.ItemsSource = new MySQLDoctorDAO().GetAll();
                cbExamType.ItemsSource = new MySQLExamTypeDAO().GetAll();
                cbMedication.ItemsSource = new MySQLMedicationDAO().GetAll();
                gridExam.DataContext = new GenericDataGridViewModel<Exam>()
                {
                    SelectedItem = exam,
                    Theme = settings.Theme
                };
                gridPrescription.DataContext = new GenericDataGridViewModel<Prescription>()
                {
                    SelectedItem = prescription,
                    Theme = settings.Theme
                };
                gridReferral.DataContext = new GenericDataGridViewModel<Referral>()
                {
                    SelectedItem = referral,
                    Theme = settings.Theme
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Properties.Resources.ErrorMessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CbPrescription_Checked(object sender, RoutedEventArgs e)
        {
            gridPrescription.IsEnabled = true;
        }

        private void CbPrescription_Unchecked(object sender, RoutedEventArgs e)
        {
            gridPrescription.IsEnabled = false;
        }

        private void CbReferral_Checked(object sender, RoutedEventArgs e)
        {
            gridReferral.IsEnabled = true;
        }

        private void CbReferral_Unchecked(object sender, RoutedEventArgs e)
        {
            gridReferral.IsEnabled = false;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (exam.Doctor == null)
                exam.Doctor = cbDoctor.SelectedItem as Doctor;
            exam.ExamType = cbExamType.SelectedItem as ExamType;
            exam.DateTime = DateTime.Now;
            MessageBoxResult result = MessageBox.Show(Properties.Resources.ConfirmationDialogContent, Properties.Resources.ConfirmationTitle, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    new MySQLExamDAO().Add(exam);
                    if (cbPrescription.IsChecked.Value)
                    {
                        prescription.Exam = exam;
                        prescription.Medication = cbMedication.SelectedItem as Medication;
                        prescription.Date = DateTime.Now;
                        new MySQLPrescriptionDAO().Add(prescription);
                    }
                    if (cbReferral.IsChecked.Value)
                    {
                        referral.Exam = exam;
                        new MySQLReferralDAO().Add(referral);
                    }
                    MessageBox.Show(Properties.Resources.SuccessMessage, Properties.Resources.SuccessMessageTitle, MessageBoxButton.OK);
                    DialogResult = true;
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Properties.Resources.ErrorMessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
