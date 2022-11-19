﻿using HCI_Project_A_2022___Clinic.Data.DataAcces.MySQLDataAccess;
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
        private readonly Appointment appointment;
        public AppointmentWindow()
        {
            InitializeComponent();
            appointment = new Appointment();
            try
            {
                cbDoctor.ItemsSource = new MySQLDoctorDAO().GetAll();
                gridAppointment.DataContext = appointment;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Properties.Resources.ErrorMessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        internal AppointmentWindow(Patient patient)
        {
            InitializeComponent();
            appointment = new Appointment() { Patient = patient };
            try
            {
                cbDoctor.ItemsSource = new MySQLDoctorDAO().GetAll();
                gridAppointment.DataContext = appointment;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Properties.Resources.ErrorMessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        internal AppointmentWindow(Employee employee, Appointment appointment)
        {
            InitializeComponent();
            this.appointment = appointment;
            try
            {
                cbDoctor.ItemsSource = new MySQLDoctorDAO().GetAll();
                cbDoctor.SelectedItem = appointment.Doctor;
                gridAppointment.DataContext = appointment;
                btnSave.Visibility = Visibility.Collapsed;
                if (employee.Role == EmployeeRole.LJEKAR)
                    btnNewExam.Visibility = Visibility.Visible;
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
                    appointment.DateTime = DateTime.Parse(dpDate.Text + " " + tpTime.Text);
                    if (appointment.Doctor == null)
                        appointment.Doctor = cbDoctor.SelectedItem as Doctor;
                    new MySQLAppointmentDAO().Add(appointment);
                    DialogResult = true;
                    MessageBox.Show(Properties.Resources.SuccessMessage, Properties.Resources.SuccessMessageTitle, MessageBoxButton.OK);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Properties.Resources.ErrorMessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                }
                Close();
            }
        }

        private void BtnNewExam_Click(object sender, RoutedEventArgs e)
        {
            new ExamWindow(appointment.Patient, appointment.Doctor).ShowDialog();
        }
    }
}
