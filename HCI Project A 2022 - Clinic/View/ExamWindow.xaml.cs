using HCI_Project_A_2022___Clinic.Data.DataAcces.MySQLDataAccess;
using HCI_Project_A_2022___Clinic.Data.Model;
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
        private Exam exam;
        private Prescription prescription;
        private Referral referral;
        internal ExamWindow(Doctor doctor)
        {
            InitializeComponent();
            exam = new Exam()
            {
                Doctor = doctor
            };
            Load();
            cbDoctor.SelectedItem = doctor;
        }

        internal ExamWindow(Patient patient, Doctor doctor)
        {
            InitializeComponent();
            exam = new Exam()
            {
                Doctor = doctor,
                Patient = patient
            };
            Load();
            cbDoctor.SelectedItem = doctor;
        }
        internal ExamWindow(Exam exam)
        {
            InitializeComponent();
            this.exam = exam;
            Load();
            cbDoctor.SelectedItem = exam.Doctor;
            cbExamType.SelectedItem = exam.ExamType;
            var prescriptions = new MySQLPrescriptionDAO().Get(new Prescription() { Exam = new Exam() { ExamId = exam.ExamId } });
            if (prescriptions.Count > 0)
            {
                gridPrescription.DataContext = prescriptions[0];
                cbMedication.SelectedItem = prescriptions[0].Medication;
                cbPrescription.IsChecked = true;
            }
            var referrals = new MySQLReferralDAO().Get(new Referral() { Exam = new Exam() { ExamId = exam.ExamId } });
            if (referrals.Count > 0)
            {
                gridReferral.DataContext = referrals[0];
                cbRefferal.IsChecked = true;
            }
            IsEnabled = false;
        }
        private void Load()
        {
            prescription = new Prescription();
            referral = new Referral();
            cbDoctor.ItemsSource = new MySQLDoctorDAO().GetAll();
            cbExamType.ItemsSource = new MySQLExamTypeDAO().GetAll();
            cbMedication.ItemsSource = new MySQLMedicationDAO().GetAll();
            gridExam.DataContext = exam;
            gridPrescription.DataContext = prescription;
            gridReferral.DataContext = referral;
        }

        private void CbPrescription_Checked(object sender, RoutedEventArgs e)
        {
            gridPrescription.IsEnabled = true;
        }

        private void CbPrescription_Unchecked(object sender, RoutedEventArgs e)
        {
            gridPrescription.IsEnabled = false;
        }

        private void CbRefferal_Checked(object sender, RoutedEventArgs e)
        {
            gridReferral.IsEnabled = true;
        }

        private void CbRefferal_Unchecked(object sender, RoutedEventArgs e)
        {
            gridReferral.IsEnabled = false;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (exam.Doctor == null)
                exam.Doctor = cbDoctor.SelectedItem as Doctor;
            exam.ExamType = cbExamType.SelectedItem as ExamType;
            exam.DateTime = DateTime.Now;
            new MySQLExamDAO().Add(exam);
            if (cbPrescription.IsChecked.Value)
            {
                prescription.Exam = exam;
                prescription.Medication = cbMedication.SelectedItem as Medication;
                prescription.Date = DateTime.Now;
                new MySQLPrescriptionDAO().Add(prescription);
            }
            if (cbRefferal.IsChecked.Value)
            {
                referral.Exam = exam;
                new MySQLReferralDAO().Add(referral);
            }
        }
    }
}
