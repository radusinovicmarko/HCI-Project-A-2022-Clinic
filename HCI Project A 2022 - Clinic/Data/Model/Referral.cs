using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_Project_A_2022___Clinic.Data.Model
{
    internal class Referral
    {
        public int ReferralId { get; set; }
        public string InstitutionName { get; set; }
        public string InstitutionCode { get; set; }
        public string Type { get; set; }
        public Exam Exam { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Referral referral &&
                   ReferralId == referral.ReferralId;
        }

        public override int GetHashCode()
        {
            return -1377503995 + ReferralId.GetHashCode();
        }
    }
}
