﻿using HCI_Project_A_2022___Clinic.Data.Model;
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
    /// Interaction logic for RoleWindow.xaml
    /// </summary>
    public partial class RoleWindow : Window
    {
        public RoleWindow()
        {
            InitializeComponent();
            cbRole.ItemsSource = Enum.GetValues(typeof(EmployeeRole)).Cast<EmployeeRole>();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (cbRole.SelectedItem != null)
            {
                new EmployeeWindow((EmployeeRole)Enum.Parse(typeof(EmployeeRole), cbRole.SelectedItem.ToString())).ShowDialog();
                Close();
            }
        }
    }
}