using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_Project_A_2022___Clinic.Data.DataAcces
{
    internal interface IGenericSearchDAO<T> : IGenericDAO<T>
    {
        List<T> Get(T item);
    }
}
