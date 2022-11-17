using System;

namespace HCI_Project_A_2022___Clinic.Data.Model
{
    internal enum EmployeeRole
    {
        ADMIN, LJEKAR, TEHNICAR
    }

    internal class Employee : Person
    {
        public decimal Salary { get; set; }
        public EmployeeRole? Role { get; set; } = null;
        public DateTime? HireDate { get; set; } = null;
        public string Qualification { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool Employed { get; set; }
    }
}
