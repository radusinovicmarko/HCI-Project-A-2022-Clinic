using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_Project_A_2022___Clinic.Data.Model
{
    internal class City
    {
        public int CityId { get; set; }
        public string CityName { get; set; }
        public string PostCode { get; set; }

        public override bool Equals(object obj)
        {
            return obj is City city &&
                   CityId == city.CityId;
        }

        public override int GetHashCode()
        {
            return -528980569 + CityId.GetHashCode();
        }

        public override string ToString()
        {
            return CityName;
        }
    }
}
