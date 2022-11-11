﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_Project_A_2022___Clinic.ViewModel
{
    internal class GenericDataGridViewModel<T>
    {
        public IList<T> Items { get; set; }
        public T SelectedItem { get; set; }
    }
}
