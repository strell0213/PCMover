using Microsoft.Win32;
using PCMover.Models;
using PCMover.SimpleLogs;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PCMover.Services
{
    public class SystemSettingsService
    {
        const string NAME_TYPE_DATA = "SystemSettings";
        public SystemSettingsService() { }
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

            try
            {
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
            catch(Exception ex)
            {
                Logger log = new Logger("Error Region Settings", ex.Message, (logLevel)3);
                log.Write();
            }
        }

        public void GetPrivacySettings(List<IEData> systemDatas)
        {
            IEData data = null;

            try
            {
                int adIdEnabled = (int)(Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\AdvertisingInfo", "Enabled", 0) ?? 0);
                data = new IEData("AdverstingInfo", adIdEnabled.ToString(), NAME_TYPE_DATA);
                systemDatas.Add(data);

                int tailored = (int)(Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Privacy", "TailoredExperiencesWithDiagnosticDataEnabled", 0) ?? 0);
                data = new IEData("TailoredExperiencesWithDiagnosticDataEnabled", tailored.ToString(), NAME_TYPE_DATA);
                systemDatas.Add(data);

                int trackApps = (int)(Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_TrackProgs", 0) ?? 0);
                data = new IEData("Start_TrackProgs", trackApps.ToString(), NAME_TYPE_DATA);
                systemDatas.Add(data);
            }
            catch (Exception ex)
            {
                Logger log = new Logger("Error Privacy Settings", ex.Message, (logLevel)3);
                log.Write();
            }
        }
    }
}
