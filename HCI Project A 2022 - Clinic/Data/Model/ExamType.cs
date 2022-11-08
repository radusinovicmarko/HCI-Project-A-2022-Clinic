using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_Project_A_2022___Clinic.Data.Model
{
    internal class ExamType
    {
        public int ExamTypeId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        public override bool Equals(object obj)
        {
            return obj is ExamType type &&
                   ExamTypeId == type.ExamTypeId;
        }

        public override int GetHashCode()
        {
            return 710036551 + ExamTypeId.GetHashCode();
        }
    }
}
