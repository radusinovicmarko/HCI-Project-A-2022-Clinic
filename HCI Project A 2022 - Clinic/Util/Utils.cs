using HCI_Project_A_2022___Clinic.Data.Model;
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

        private static readonly string settingsPath = ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + "Resources" + Path.DirectorySeparatorChar + "settings.json";
        public static SettingsViewModel LoadSettings()
        {
            return JsonConvert.DeserializeObject<SettingsViewModel>(File.ReadAllText(settingsPath));
        }

        public static SettingsViewModel LoadSettings(Employee employee)
        {
            Dictionary<int, SettingsViewModel> settings = JsonConvert.DeserializeObject<Dictionary<int, SettingsViewModel>>(File.ReadAllText(settingsPath));
            if (employee != null && settings.ContainsKey((int)employee.PersonId))
                return settings[(int)employee.PersonId];
            else
                return settings[-1];
        }

        public static void SaveSettings(Employee employee, SettingsViewModel settings)
        {
            Dictionary<int, SettingsViewModel> allSettings = JsonConvert.DeserializeObject<Dictionary<int, SettingsViewModel>>(File.ReadAllText(settingsPath));
            if (allSettings.ContainsKey((int)employee.PersonId))
                allSettings[(int)employee.PersonId] = settings;
            else
                allSettings.Add((int)employee.PersonId, settings);
            File.WriteAllText("settings.json", JsonConvert.SerializeObject(allSettings, Formatting.Indented));
        }
    }
}
