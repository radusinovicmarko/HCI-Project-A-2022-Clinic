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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HCI_Project_A_2022___Clinic.View
{
    /// <summary>
    /// Interaction logic for ExamsPage.xaml
    /// </summary>
    public partial class ExamsPage : Page
    {
        private GenericDataGridViewModel<Exam> examsViewModel;
        private readonly Employee employee;
        internal ExamsPage(Employee employee)
        {
            InitializeComponent();
            this.employee = employee;
            if (employee.Role != EmployeeRole.LJEKAR)
                btnAddNewExam.Visibility = Visibility.Collapsed;
            try
            {
                cbDoctor.ItemsSource = new MySQLDoctorDAO().GetAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Properties.Resources.ErrorMessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            UpdateDG();
        }
        private void UpdateDG()
        {
            try
            {
                examsViewModel = new GenericDataGridViewModel<Exam>()
                {
                    Items = new ObservableCollection<Exam>(new MySQLExamDAO().GetAll())
                };
                DataContext = examsViewModel;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Properties.Resources.ErrorMessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Search()
        {
            if (string.IsNullOrEmpty(dpDate.Text) && cbDoctor.SelectedItem == null)
            {
                UpdateDG();
                return;
            }
            try
            {
                Exam searchExam = new Exam();
                if (!string.IsNullOrEmpty(dpDate.Text))
                    searchExam.DateTime = DateTime.Parse(dpDate.Text);
                if (cbDoctor.SelectedItem != null)
                    searchExam.Doctor = cbDoctor.SelectedItem as Doctor;
                examsViewModel = new GenericDataGridViewModel<Exam>()
                {
                    Items = new ObservableCollection<Exam>(new MySQLExamDAO().Get(searchExam))
                };
                DataContext = examsViewModel;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Properties.Resources.ErrorMessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnAddNewExam_Click(object sender, RoutedEventArgs e)
        {
            if (new ExamWindow((Doctor)employee).ShowDialog().Value)
                UpdateDG();
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            Search();
        }

        private void DgExams_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (examsViewModel.SelectedItem != null)
                new ExamWindow(examsViewModel.SelectedItem).ShowDialog();
        }
    }
}
