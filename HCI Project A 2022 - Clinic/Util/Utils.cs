using HCI_Project_A_2022___Clinic.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_Project_A_2022___Clinic.Util
{
    internal class Utils
    {
        public static SettingsViewModel LoadSettings()
        {
            return JsonConvert.DeserializeObject<SettingsViewModel>(File.ReadAllText("settings.json"));
        }
    }
}
