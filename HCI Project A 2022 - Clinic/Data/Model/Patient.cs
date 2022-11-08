using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_Project_A_2022___Clinic.Data.Model
{
    internal class Patient : Person
    {
        public string MedicalCardNumber { get; set; }
        public string BloodType { get; set; }
        public string MarriageStatus { get; set; }
        public string PhoneNumber { get; set; }
    }
}
