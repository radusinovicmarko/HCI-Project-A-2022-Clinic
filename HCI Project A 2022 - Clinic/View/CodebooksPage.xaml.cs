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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HCI_Project_A_2022___Clinic.View
{
    /// <summary>
    /// Interaction logic for CodebooksPage.xaml
    /// </summary>
    public partial class CodebooksPage : Page
    {
        private GenericViewModelEntity<City> citiesViewModel;
        private GenericViewModelEntity<ExamType> examTypesViewModel;
        private GenericViewModelEntity<Illness> illnessesViewModel;
        private GenericViewModelEntity<Medication> medicationsViewModel;
        private readonly Employee employee;
        private readonly SettingsViewModel settings;
        internal CodebooksPage(SettingsViewModel settings, Employee employee)
        {
            InitializeComponent();
            this.employee = employee;
            this.settings = settings;
            if (employee.Role != EmployeeRole.ADMIN)
            {
                spAddCity.IsEnabled = false;
                spEditCity.IsEnabled = false;
                spAddExamType.IsEnabled = false;
                spEditExamType.IsEnabled = false;
                spAddIllness.IsEnabled = false;
                spEditIllness.IsEnabled = false;
                spAddMedication.IsEnabled = false;
                spEditMedication.IsEnabled = false;
            }
            DataContext = settings;
            UpdateCitiesDG();
            spAddCity.DataContext = new GenericViewModelEntity<City>
            {
                SelectedItem = new City(),
                Theme = settings.Theme
            };
            UpdateExamTypesDG();
            spAddExamType.DataContext = new GenericViewModelEntity<ExamType>
            {
                SelectedItem = new ExamType(),
                Theme = settings.Theme
            };
            UpdateIllnessesDG();
            spAddIllness.DataContext = new GenericViewModelEntity<Illness>
            {
                SelectedItem = new Illness(),
                Theme = settings.Theme
            };
            UpdateMedicationsDG();
            spAddMedication.DataContext = new GenericViewModelEntity<Medication>
            {
                SelectedItem = new Medication(),
                Theme = settings.Theme
            };
        }

        private void UpdateCitiesDG()
        {
            try
            {
                citiesViewModel = new GenericViewModelEntity<City>()
                {
                    Items = new MySQLCityDAO().GetAll(),
                    Theme = settings.Theme
                };
                gridCities.DataContext = citiesViewModel;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Properties.Resources.ErrorMessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void UpdateExamTypesDG()
        {
            try
            {
                examTypesViewModel = new GenericViewModelEntity<ExamType>()
                {
                    Items = new MySQLExamTypeDAO().GetAll(),
                    Theme = settings.Theme
                };
                gridExamTypes.DataContext = examTypesViewModel;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Properties.Resources.ErrorMessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void UpdateIllnessesDG()
        {
            try
            {
                illnessesViewModel = new GenericViewModelEntity<Illness>()
                {
                    Items = new MySQLIllnessDAO().GetAll(),
                    Theme = settings.Theme
                };
                gridIllnesses.DataContext = illnessesViewModel;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Properties.Resources.ErrorMessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateMedicationsDG()
        {
            try
            {
                medicationsViewModel = new GenericViewModelEntity<Medication>()
                {
                    Items = new MySQLMedicationDAO().GetAll(),
                    Theme = settings.Theme
                };
                gridMedications.DataContext = medicationsViewModel;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Properties.Resources.ErrorMessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DgExamTypes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (examTypesViewModel.SelectedItem != null && employee.Role == EmployeeRole.ADMIN)
            {
                spEditExamType.IsEnabled = true;
                spEditExamType.DataContext = examTypesViewModel;
            }
        }

        private void DgCities_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (citiesViewModel.SelectedItem != null && employee.Role == EmployeeRole.ADMIN)
            {
                spEditCity.IsEnabled = true;
                spEditCity.DataContext = citiesViewModel;
            }
        }

        private void DgIllnesses_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (illnessesViewModel.SelectedItem != null && employee.Role == EmployeeRole.ADMIN)
            {
                spEditIllness.IsEnabled = true;
                spEditIllness.DataContext = illnessesViewModel;
            }
        }

        private void DgMedications_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (medicationsViewModel.SelectedItem != null && employee.Role == EmployeeRole.ADMIN)
            {
                spEditMedication.IsEnabled = true;
                spEditMedication.DataContext = medicationsViewModel;
            }
        }

        private void BtnAddCity_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(Properties.Resources.ConfirmationDialogContent, Properties.Resources.ConfirmationTitle, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    City city = (spAddCity.DataContext as GenericViewModelEntity<City>).SelectedItem;
                    new MySQLCityDAO().Add(city);
                    UpdateCitiesDG();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Properties.Resources.ErrorMessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BtnEditCity_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(Properties.Resources.ConfirmationDialogContent, Properties.Resources.ConfirmationTitle, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    City city = (spEditCity.DataContext as GenericViewModelEntity<City>).SelectedItem;
                    new MySQLCityDAO().Update(city.CityId, city);
                    UpdateCitiesDG();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Properties.Resources.ErrorMessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BtnEditExamType_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(Properties.Resources.ConfirmationDialogContent, Properties.Resources.ConfirmationTitle, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    ExamType examType = (spEditExamType.DataContext as GenericViewModelEntity<ExamType>).SelectedItem;
                    new MySQLExamTypeDAO().Update(examType.ExamTypeId, examType);
                    UpdateExamTypesDG();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Properties.Resources.ErrorMessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BtnAddExamType_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(Properties.Resources.ConfirmationDialogContent, Properties.Resources.ConfirmationTitle, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    ExamType examType = (spAddExamType.DataContext as GenericViewModelEntity<ExamType>).SelectedItem;
                    new MySQLExamTypeDAO().Add(examType);
                    UpdateExamTypesDG();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Properties.Resources.ErrorMessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BtnAddllness_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(Properties.Resources.ConfirmationDialogContent, Properties.Resources.ConfirmationTitle, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    Illness illness = (spAddIllness.DataContext as GenericViewModelEntity<Illness>).SelectedItem;
                    new MySQLIllnessDAO().Add(illness);
                    UpdateIllnessesDG();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Properties.Resources.ErrorMessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BtnEditllness_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(Properties.Resources.ConfirmationDialogContent, Properties.Resources.ConfirmationTitle, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    Illness illness = (spEditIllness.DataContext as GenericViewModelEntity<Illness>).SelectedItem;
                    new MySQLIllnessDAO().Update(illness.IllnessId, illness);
                    UpdateIllnessesDG();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Properties.Resources.ErrorMessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BtnAddMedication_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(Properties.Resources.ConfirmationDialogContent, Properties.Resources.ConfirmationTitle, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    Medication medication = (spAddMedication.DataContext as GenericViewModelEntity<Medication>).SelectedItem;
                    new MySQLMedicationDAO().Add(medication);
                    UpdateMedicationsDG();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Properties.Resources.ErrorMessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BtnEditMedication_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(Properties.Resources.ConfirmationDialogContent, Properties.Resources.ConfirmationTitle, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    Medication medication = (spEditMedication.DataContext as GenericViewModelEntity<Medication>).SelectedItem;
                    new MySQLMedicationDAO().Update(medication.MedicationId, medication);
                    UpdateMedicationsDG();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Properties.Resources.ErrorMessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
