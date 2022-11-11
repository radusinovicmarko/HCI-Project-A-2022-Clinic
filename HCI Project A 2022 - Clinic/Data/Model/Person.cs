using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_Project_A_2022___Clinic.Data.Model
{
    internal abstract class Person
    {
        public int? PersonId { get; set; } = null;
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Jmb { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public City City { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Person person &&
                   PersonId == person.PersonId;
        }

        public override int GetHashCode()
        {
            return -1255590651 + PersonId.GetHashCode();
        }
    }
}
