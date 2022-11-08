using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_Project_A_2022___Clinic.Data.Model
{
    internal class Exam
    {
        public int ExamId { get; set; }
        public DateTime DateTime { get; set; }
        public string DiagnosisCode { get; set; }
        public string Report { get; set; }
        public ExamType ExamType { get; set; }
        public Patient Patient { get; set; }
        public Doctor Doctor { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Exam exam &&
                   ExamId == exam.ExamId;
        }

        public override int GetHashCode()
        {
            return 97181431 + ExamId.GetHashCode();
        }
    }
}
