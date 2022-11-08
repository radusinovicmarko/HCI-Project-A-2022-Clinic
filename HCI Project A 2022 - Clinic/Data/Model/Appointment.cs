using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_Project_A_2022___Clinic.Data.Model
{
    internal class Appointment
    {
        public int AppointmentId { get; set; }
        public DateTime DateTime { get; set; }
        public string Reason { get; set; }
        public Patient Patient { get; set; }
        public Doctor Doctor { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Appointment appointment &&
                   AppointmentId == appointment.AppointmentId;
        }

        public override int GetHashCode()
        {
            return -282582001 + AppointmentId.GetHashCode();
        }
    }
}
