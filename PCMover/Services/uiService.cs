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
using System.Windows;

namespace PCMover.Services
{
    public class UIService
    {
        const string NAME_TYPE_DATA = "UISettings";
        public UIService() { }
        public List<IEData> ExportUI()
        {
            List<IEData> personalizationDatas = new List<IEData>();
            GetPrivacyPersonalizationSettings(personalizationDatas);
            GetPersonalize(personalizationDatas);
            GetAdPersonalizationStart(personalizationDatas);
            GetNotificationStart(personalizationDatas);

            return personalizationDatas;
        }
        public void GetPrivacyPersonalizationSettings(List<IEData> personalizationDatas)
        {
            IEData personalization = null;

            try
            {
                int adWallpapers = (int)(Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Wallpapers", "BackgroundHistoryPath0", 0) ?? 0);
                personalization = new IEData("SlideshowSourceDirectoriesSet", adWallpapers.ToString(), NAME_TYPE_DATA);
                personalizationDatas.Add(personalization);

                //int adColor = (int)(Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "Enabled", 0) ?? 0);
                //personalization = new IEData("BackgroundHistoryPath0", adColor.ToString(), NAME_TYPE_DATA);
                //personalizationDatas.Add(personalization);
                //---------------------------GetPersonalize(List<IEData> personalizationDatas)----------------------------

                int adTheme = (int)(Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes", "Enabled", 0) ?? 0);
                personalization = new IEData("WallpaperSetFromTheme", adTheme.ToString(), NAME_TYPE_DATA);
                personalizationDatas.Add(personalization);

                int adTask = (int)(Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Taskband", "Enabled", 0) ?? 0);
                personalization = new IEData("FavoritesVersion", adTask.ToString(), NAME_TYPE_DATA);
                personalizationDatas.Add(personalization);
            }
            catch (Exception ex)
            {
                Logger log = new Logger("Error Privacy Settings", ex.Message, (logLevel)3);
                log.Write();
            }
        }

        public void GetPersonalize(List<IEData> personalizationDatas)
        {
            IEData personalization = null;

            try
            {
                int appsUseLightTheme = (int)(Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "AppsUseLightTheme", 0) ?? 0);
                personalization = new IEData("AppsUseLightTheme", appsUseLightTheme.ToString(), NAME_TYPE_DATA);
                personalizationDatas.Add(personalization);

                int colorPrevalence = (int)(Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "ColorPrevalence", 0) ?? 0);
                personalization = new IEData("ColorPrevalence", colorPrevalence.ToString(), NAME_TYPE_DATA);
                personalizationDatas.Add(personalization);

                int enableTransparency = (int)(Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "EnableTransparency", 0) ?? 0);
                personalization = new IEData("EnableTransparency", enableTransparency.ToString(), NAME_TYPE_DATA);
                personalizationDatas.Add(personalization);

                int systemUsesLightTheme = (int)(Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "SystemUsesLightTheme", 0) ?? 0);
                personalization = new IEData("SystemUsesLightTheme", systemUsesLightTheme.ToString(), NAME_TYPE_DATA);
                personalizationDatas.Add(personalization);
            }
            catch (Exception ex)
            {
                Logger log = new Logger("Error Privacy Settings", ex.Message, (logLevel)3);
                log.Write();
            }
        }

        public void GetNotificationStart(List<IEData> personalizationDatas)
        {
            IEData personalization = null;

            try
            {
                int adToastEnabled = (int)(Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PushNotifications", "Enabled", 0) ?? 0);
                personalization = new IEData("ToastEnabled", adToastEnabled.ToString(), NAME_TYPE_DATA);
                personalizationDatas.Add(personalization);

                int adNOC_GLOBAL_SETTING_ALLOW_TOASTS_ABOVE_LOCK = (int)(Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PushNotifications", "Enabled", 0) ?? 0);
                personalization = new IEData("NOC_GLOBAL_SETTING_ALLOW_TOASTS_ABOVE_LOCK", adNOC_GLOBAL_SETTING_ALLOW_TOASTS_ABOVE_LOCK.ToString(), NAME_TYPE_DATA);
                personalizationDatas.Add(personalization);

                int adNOC_GLOBAL_SETTING_ALLOW_TOASTS_SOUND = (int)(Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PushNotifications", "Enabled", 0) ?? 0);
                personalization = new IEData("NOC_GLOBAL_SETTING_ALLOW_TOASTS_SOUND", adNOC_GLOBAL_SETTING_ALLOW_TOASTS_SOUND.ToString(), NAME_TYPE_DATA);
                personalizationDatas.Add(personalization);

                int adSubscribedContentEnabled = (int)(Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "Enabled", 0) ?? 0);
                personalization = new IEData("SubscribedContent-338388Enabled", adSubscribedContentEnabled.ToString(), NAME_TYPE_DATA);
                personalizationDatas.Add(personalization);

                int adSubscribedCon = (int)(Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "Enabled", 0) ?? 0);
                personalization = new IEData("SoftLandingEnabled", adSubscribedCon.ToString(), NAME_TYPE_DATA);
                personalizationDatas.Add(personalization);

                int adSystemPaneSuggestionsEnabled = (int)(Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "Enabled", 0) ?? 0);
                personalization = new IEData("SystemPaneSuggestionsEnabled", adSystemPaneSuggestionsEnabled.ToString(), NAME_TYPE_DATA);
                personalizationDatas.Add(personalization);
            }
            catch (Exception ex)
            {
                Logger log = new Logger("Error Privacy Settings", ex.Message, (logLevel)3);
                log.Write();
            }
        }
        public void GetAdPersonalizationStart(List<IEData> personalizationDatas)
        {
            IEData personalization = null;

            try
            {
                int adStart_Expanded = (int)(Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Enabled", 0) ?? 0);
                personalization = new IEData("Start_Grid_Expanded", adStart_Expanded.ToString(), NAME_TYPE_DATA);
                personalizationDatas.Add(personalization);

                int adStart_ShowAppsList = (int)(Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Enabled", 0) ?? 0);
                personalization = new IEData("Start_ShowAppsList", adStart_ShowAppsList.ToString(), NAME_TYPE_DATA);
                personalizationDatas.Add(personalization);

                int adStart_TrackAppInstall = (int)(Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Enabled", 0) ?? 0);
                personalization = new IEData("Start_TrackAppInstall", adStart_TrackAppInstall.ToString(), NAME_TYPE_DATA);
                personalizationDatas.Add(personalization);

                int adStart_TrackUserAppUse = (int)(Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Enabled", 0) ?? 0);
                personalization = new IEData("Start_TrackUserAppUse", adStart_TrackUserAppUse.ToString(), NAME_TYPE_DATA);
                personalizationDatas.Add(personalization);

                int adStart_ShowSuggestions = (int)(Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Enabled", 0) ?? 0);
                personalization = new IEData("Start_ShowSuggestions", adStart_ShowSuggestions.ToString(), NAME_TYPE_DATA);
                personalizationDatas.Add(personalization);

                int adStart_Layout = (int)(Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Enabled", 0) ?? 0);
                personalization = new IEData("adStart_Layout", adStart_Layout.ToString(), NAME_TYPE_DATA);
                personalizationDatas.Add(personalization);

                int adStart_RecentDocsHistory = (int)(Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Enabled", 0) ?? 0);
                personalization = new IEData("adStart_RecentDocsHistory", adStart_RecentDocsHistory.ToString(), NAME_TYPE_DATA);
                personalizationDatas.Add(personalization);

                int adStart_AccountSettingsNotifications = (int)(Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Enabled", 0) ?? 0);
                personalization = new IEData("Start_AccountSettingsNotifications", adStart_AccountSettingsNotifications.ToString(), NAME_TYPE_DATA);
                personalizationDatas.Add(personalization);
            }
            catch (Exception ex)
            {
                Logger log = new Logger("Error Privacy Settings", ex.Message, (logLevel)3);
                log.Write();
            }
        }
    }
}
