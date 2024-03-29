﻿using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_Project_A_2022___Clinic.ViewModel
{
    internal class Theme
    {
        public static Theme LightTheme = new Theme()
        {
            Name = "Light",
            Background = "#FFFFFF",
            Surface = "#9EBDF0",
            Primary = "#FFFFFF",
            Secondary = "#FFFFFF",
            OnBackground = "#202020",
            OnSurface = "#2E2E20"
        };

        public static Theme DarkTheme = new Theme()
        {
            Name = "Dark",
            Background = "#202020",
            Surface = "#121212",
            Primary = "#C38FFF",
            Secondary = "#03DAC6",
            OnBackground = "#FFFFFF",
            OnSurface = "#FFFFFF"
        };

        public static Theme MixTheme = new Theme()
        {
            /*Name = "Mix",
            Background = "#6C5B7B",
            Surface = "#355C7D",
            Primary = "#F67280",
            Secondary = "#C06C84",
            OnBackground = "#FFFFFF",
            OnSurface = "#FFFFFF"*/
            Name = "Mix",
            Background = "#48466D",
            Surface = "#3D84A8",
            Primary = "#46CDCF",
            Secondary = "#ABEDD8",
            OnBackground = "#FFFFFF",
            OnSurface = "#1E212D"
        };

        public string Name { get; set; }
        public string Background { get; set; }
        public string Surface { get; set; }
        public string Primary { get; set; }
        public string Secondary { get; set; }
        public string OnBackground { get; set; }
        public string OnSurface { get; set; }

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
