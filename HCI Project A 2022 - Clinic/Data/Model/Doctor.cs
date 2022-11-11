using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_Project_A_2022___Clinic.Data.Model
{
    internal class Doctor : Employee
    {
        public string Title { get; set; }

        public override string ToString()
        {
            return LastName + " " + FirstName + ", " + Title;
        }
    }

}
