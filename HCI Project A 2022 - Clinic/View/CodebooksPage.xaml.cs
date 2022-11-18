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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HCI_Project_A_2022___Clinic.View
{
    /// <summary>
    /// Interaction logic for CodebooksPage.xaml
    /// </summary>
    public partial class CodebooksPage : Page
    {
        private SettingsViewModel settings;
        private Employee employee;
        private GenericDataGridViewModel<City> citiesViewModel;
        private GenericDataGridViewModel<ExamType> examTypesViewModel;
        private GenericDataGridViewModel<Illness> illnessesViewModel;
        private GenericDataGridViewModel<Medication> medicationsViewModel;

        internal CodebooksPage(SettingsViewModel settings, Employee employee)
        {
            InitializeComponent();
            this.settings = settings;
            this.employee = employee;
            UpdateCitiesDG();
            spAddCity.DataContext = new City();
            UpdateExamTypesDG();
            spAddExamType.DataContext = new ExamType();
            UpdateIllnessesDG();
            spAddIllness.DataContext = new Illness();
            UpdateMedicationsDG();
            spAddMedication.DataContext = new Medication();
        }

        private void UpdateCitiesDG()
        {
            citiesViewModel = new GenericDataGridViewModel<City>()
            {
                Items = new MySQLCityDAO().GetAll()
            };
            gridCities.DataContext = citiesViewModel;
        }
        private void UpdateExamTypesDG()
        {
            examTypesViewModel = new GenericDataGridViewModel<ExamType>()
            {
                Items = new MySQLExamTypeDAO().GetAll()
            };
            gridExamTypes.DataContext = examTypesViewModel;
        }
        private void UpdateIllnessesDG()
        {
            illnessesViewModel = new GenericDataGridViewModel<Illness>()
            {
                Items = new MySQLIllnessDAO().GetAll()
            };
            gridIllnesses.DataContext = illnessesViewModel;
        }
        
        private void UpdateMedicationsDG()
        {
            medicationsViewModel = new GenericDataGridViewModel<Medication>()
            {
                Items = new MySQLMedicationDAO().GetAll()
            };
            gridMedications.DataContext = medicationsViewModel;
        }

        private void DgExamTypes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (examTypesViewModel.SelectedItem != null)
            {
                spEditExamType.IsEnabled = true;
                spEditExamType.DataContext = examTypesViewModel.SelectedItem;
            }
        }

        private void DgCities_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (citiesViewModel.SelectedItem != null)
            {
                spEditCity.IsEnabled = true;
                spEditCity.DataContext = citiesViewModel.SelectedItem;
            }
        }

        private void DgIllnesses_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (illnessesViewModel.SelectedItem != null)
            {
                spEditIllness.IsEnabled = true;
                spEditIllness.DataContext = illnessesViewModel.SelectedItem;
            }
        }

        private void DgMedications_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (medicationsViewModel.SelectedItem != null)
            {
                spEditMedication.IsEnabled = true;
                spEditMedication.DataContext = medicationsViewModel.SelectedItem;
            }
        }

        private void BtnAddCity_Click(object sender, RoutedEventArgs e)
        {
            City city = spAddCity.DataContext as City;
            new MySQLCityDAO().Add(city);
            UpdateCitiesDG();
        }

        private void BtnEditCity_Click(object sender, RoutedEventArgs e)
        {
            City city = spEditCity.DataContext as City;
            new MySQLCityDAO().Update(city.CityId, city);
            UpdateCitiesDG();
        }

        private void BtnEditExamType_Click(object sender, RoutedEventArgs e)
        {
            ExamType examType = spEditExamType.DataContext as ExamType;
            new MySQLExamTypeDAO().Update(examType.ExamTypeId, examType);
            UpdateExamTypesDG();
        }

        private void BtnAddExamType_Click(object sender, RoutedEventArgs e)
        {
            ExamType examType = spEditExamType.DataContext as ExamType;
            new MySQLExamTypeDAO().Add(examType);
            UpdateExamTypesDG();
        }

        private void BtnAddllness_Click(object sender, RoutedEventArgs e)
        {
            Illness illness = spAddIllness.DataContext as Illness;
            new MySQLIllnessDAO().Add(illness);
            UpdateIllnessesDG();
        }

        private void BtnEditllness_Click(object sender, RoutedEventArgs e)
        {
            Illness illness = spEditCity.DataContext as Illness;
            new MySQLIllnessDAO().Update(illness.IllnessId, illness);
            UpdateIllnessesDG();
        }

        private void BtnAddMedication_Click(object sender, RoutedEventArgs e)
        {
            Medication medication = spAddMedication.DataContext as Medication;
            new MySQLMedicationDAO().Add(medication);
            UpdateMedicationsDG();
        }

        private void BtnEditMedication_Click(object sender, RoutedEventArgs e)
        {
            Medication medication = spEditMedication.DataContext as Medication;
            new MySQLMedicationDAO().Update(medication.MedicationId, medication);
            UpdateMedicationsDG();
        }
    }
}
