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
    /// Interaction logic for RecoveryWindow.xaml
    /// </summary>
    public partial class RecoveryWindow : Window
    {
        private readonly Recovery recovery;
        internal RecoveryWindow(Patient patient, Doctor doctor)
        {
            InitializeComponent();
            try
            {
                recovery = new Recovery()
                {
                    Patient = patient,
                    Doctor = doctor
                };
                cbDoctor.ItemsSource = new MySQLDoctorDAO().GetAll();
                cbDoctor.SelectedItem = doctor;
                cbIllness.ItemsSource = new MySQLIllnessDAO().GetAll();
                gridRecovery.DataContext = recovery;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Properties.Resources.ErrorMessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        internal RecoveryWindow(Recovery recovery)
        {
            InitializeComponent();
            this.recovery = recovery;
            try
            {
                cbDoctor.ItemsSource = new MySQLDoctorDAO().GetAll();
                cbDoctor.SelectedItem = recovery.Doctor;
                cbIllness.ItemsSource = new MySQLIllnessDAO().GetAll();
                cbIllness.SelectedItem = recovery.Illness;
                gridRecovery.DataContext = recovery;
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
                    recovery.Date = DateTime.Parse(dpDate.Text);
                    recovery.Illness = cbIllness.SelectedItem as Illness;
                    new MySQLRecoveryDAO().Add(recovery);
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
