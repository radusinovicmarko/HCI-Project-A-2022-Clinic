using HCI_Project_A_2022___Clinic.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_Project_A_2022___Clinic.ViewModel
{
    internal class PatientsViewModel
    {
        public IList<Patient> Patients { get; set; }

        public Patient SelectedPatient { get; set; }
    }
}
