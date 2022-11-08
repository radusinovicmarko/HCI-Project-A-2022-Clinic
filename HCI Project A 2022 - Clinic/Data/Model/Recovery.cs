using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_Project_A_2022___Clinic.Data.Model
{
    internal class Recovery
    {
        public int RecoveryId { get; set; }
        public DateTime Date { get; set; }
        public Illness Illness { get; set; }
        public Patient Patient { get; set; }
        public Doctor Doctor { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Recovery recovery &&
                   RecoveryId == recovery.RecoveryId;
        }

        public override int GetHashCode()
        {
            return -1554434349 + RecoveryId.GetHashCode();
        }
    }
}
