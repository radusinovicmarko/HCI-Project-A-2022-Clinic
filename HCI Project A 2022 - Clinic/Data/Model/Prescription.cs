using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_Project_A_2022___Clinic.Data.Model
{
    internal class Prescription
    {
        public int PrescriptionId { get; set; }
        public DateTime? Date { get; set; } = null;
        public string Instruction { get; set; }
        public Medication Medication { get; set; }
        public Exam Exam { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Prescription prescription &&
                   PrescriptionId == prescription.PrescriptionId;
        }

        public override int GetHashCode()
        {
            return 814419128 + PrescriptionId.GetHashCode();
        }
    }
}
