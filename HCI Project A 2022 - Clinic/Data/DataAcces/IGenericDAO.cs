using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_Project_A_2022___Clinic.Data.DataAcces
{
    internal interface IGenericDAO<T>
    {
        List<T> GetAll();
        int Add(T item);
        void Delete(int id);
        void Update(int id, T item);
    }
}
