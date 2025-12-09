using Microsoft.Win32;
using PCMover.Models;
using PCMover.SimpleLogs;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PCMover.Services
{
    public class uiService
    {
        const string NAME_TYPE_DATA = "UISettings";
        public uiService() { }
        public List<IEData> ExportUI()
        {
            List<IEData> personalizationDatas = new List<IEData>();
            personalizationSettings(personalizationDatas);
            //GetRegionSettings(systemDatas); //Настройки региона, языка, часовой пояс
            //GetPrivacySettings(systemDatas); //Настройки конфидециальности

            return personalizationDatas;
        }
        public void GetPrivacyPersonalizationSettings(List<IEData> personalizationDatas)
        {
            IEData personalization = null;

            int adWallpapers = (int)(Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Wallpapers", "BackgroundHistoryPath0", 0) ?? 0);
            personalization = new IEData("SlideshowSourceDirectoriesSet", adWallpapers.ToString(), NAME_TYPE_DATA);
            personalizationDatas.Add(personalization);

            int adColor = (int)(Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "Enabled", 0) ?? 0);
            personalization = new IEData("BackgroundHistoryPath0", adColor.ToString(), NAME_TYPE_DATA);
            personalizationDatas.Add(personalization);

            //int adLockScreen  = (int)(Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PersonalizationCSP\LockScreenImagePath", "Enabled", 0) ?? 0);
            //personalization = new IEData("AdverstingInfo", adLockScreen.ToString(), NAME_TYPE_DATA);
            //personalizationDatas.Add(personalization);

            int adTheme = (int)(Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes", "Enabled", 0) ?? 0);
            personalization = new IEData("WallpaperSetFromTheme", adTheme.ToString(), NAME_TYPE_DATA);
            personalizationDatas.Add(personalization);

            //int adFonts = (int)(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Fonts", "Enabled", 0) ?? 0);
            //personalization = new IEData("AdverstingInfo", adFonts.ToString(), NAME_TYPE_DATA);
            //personalizationDatas.Add(personalization);

            //int adStart = (int)(Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CloudStore\Store\Cache\DefaultAccount", "Enabled", 0) ?? 0);
            //personalization = new IEData("AdverstingInfo", adStart.ToString(), NAME_TYPE_DATA);
            //personalizationDatas.Add(personalization);

            int adTask = (int)(Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Taskband", "Enabled", 0) ?? 0);
            personalization = new IEData("FavoritesVersion", adTask.ToString(), NAME_TYPE_DATA);
            personalizationDatas.Add(personalization);
        }
        public void personalizationSettings(List<IEData> personalizationDatas)
        {
            IEData personalization = null;

            var background = System.Registry.GetValue();
            personalization = new IEData("background", background.Background, NAME_TYPE_DATA);
            personalizationDatas.Add(personalization);

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
    }
}
