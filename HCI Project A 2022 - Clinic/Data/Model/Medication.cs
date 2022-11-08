using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_Project_A_2022___Clinic.Data.Model
{
    internal class Medication
    {
        public int MedicationId { get; set; }
        public string GenericName { get; set; }
        public string FactoryName { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Medication medication &&
                   MedicationId == medication.MedicationId;
        }

        public override int GetHashCode()
        {
            return 2027573207 + MedicationId.GetHashCode();
        }
    }
}
