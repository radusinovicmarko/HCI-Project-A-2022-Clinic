using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_Project_A_2022___Clinic.ViewModel
{
    internal class Theme
    {
        public static Theme LightTheme = new Theme();

        public static Theme DarkTheme = new Theme();

        public static Theme MixTheme = new Theme();

        private Theme()
        {

        }
    }
    internal class SettingsViewModel
    {
        public string Language { get; set; }
        public Theme Theme { get; set; }
    }
}
