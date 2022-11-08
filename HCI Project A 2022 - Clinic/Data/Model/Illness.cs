using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_Project_A_2022___Clinic.Data.Model
{
    internal class Illness
    {
        public int IllnessId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Illness illness &&
                   IllnessId == illness.IllnessId;
        }

        public override int GetHashCode()
        {
            return 43588570 + IllnessId.GetHashCode();
        }
    }
}
