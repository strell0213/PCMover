using Microsoft.Win32;
using PCMover.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PCMover.Services
{
    public class SystemSettings
    {
        const string NAME_TYPE_DATA = "SystemSettings";
        public SystemSettings() { }
        public List<IEData> ExportSystemSettings()
        {
            List<IEData> systemDatas = new List<IEData>();

            GetRegionSettings(systemDatas); //Настройки региона, языка, часовой пояс
            GetPrivacySettings(systemDatas); //Настройки конфидециальности

            return systemDatas;
        }

        public void GetRegionSettings(List<IEData> systemDatas)
        {
            IEData data = null;

            var region = System.Globalization.RegionInfo.CurrentRegion;
            data = new IEData("Region", region.EnglishName, NAME_TYPE_DATA);
            systemDatas.Add(data);

            var ui = System.Globalization.CultureInfo.CurrentUICulture;
            data = new IEData("Lang", ui.Name, NAME_TYPE_DATA);
            systemDatas.Add(data);

            var tz = TimeZoneInfo.Local;
            data = new IEData("TimeOffset", tz.BaseUtcOffset.Hours.ToString(), NAME_TYPE_DATA);
            systemDatas.Add(data);
        }

        public void GetPrivacySettings(List<IEData> systemDatas)
        {
            IEData data = null;

            int adIdEnabled = (int)(Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\AdvertisingInfo", "Enabled", 0) ?? 0);
            data = new IEData("AdverstingInfo", adIdEnabled.ToString(), NAME_TYPE_DATA);
            systemDatas.Add(data);

            int tailored = (int)(Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Privacy", "TailoredExperiencesWithDiagnosticDataEnabled", 0) ?? 0);
            data = new IEData("TailoredExperiencesWithDiagnosticDataEnabled", tailored.ToString(), NAME_TYPE_DATA);
            systemDatas.Add(data);

            int trackApps = (int)(Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced","Start_TrackProgs",0) ?? 0);
            data = new IEData("Start_TrackProgs", trackApps.ToString(), NAME_TYPE_DATA);
            systemDatas.Add(data);
        }
    }
}
