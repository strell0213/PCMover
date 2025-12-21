using PCMover.Models;
using PCMover.SimpleLogs;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;

namespace PCMover
{
    public class DisplaySettingsService
    {
        const string NAME_TYPE_DATA = "DisplaySettings";

        // Windows API структуры и функции для работы с дисплеями
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct DEVMODE
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string dmDeviceName;
            public short dmSpecVersion;
            public short dmDriverVersion;
            public short dmSize;
            public short dmDriverExtra;
            public int dmFields;
            public int dmPositionX;
            public int dmPositionY;
            public int dmDisplayOrientation;
            public int dmDisplayFixedOutput;
            public short dmColor;
            public short dmDuplex;
            public short dmYResolution;
            public short dmTTOption;
            public short dmCollate;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string dmFormName;
            public short dmLogPixels;
            public int dmBitsPerPel;
            public int dmPelsWidth;
            public int dmPelsHeight;
            public int dmDisplayFlags;
            public int dmDisplayFrequency;
            public int dmICMMethod;
            public int dmICMIntent;
            public int dmMediaType;
            public int dmDitherType;
            public int dmReserved1;
            public int dmReserved2;
            public int dmPanningWidth;
            public int dmPanningHeight;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct DISPLAY_DEVICE
        {
            public int cb;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string DeviceName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceString;
            public int StateFlags;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceID;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceKey;
        }

        [DllImport("user32.dll")]
        private static extern bool EnumDisplayDevices(string lpDevice, uint iDevNum, ref DISPLAY_DEVICE lpDisplayDevice, uint dwFlags);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int EnumDisplaySettings(string deviceName, int modeNum, ref DEVMODE devMode);

        [DllImport("user32.dll")]
        private static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateDC(string lpszDriver, string lpszDevice, string lpszOutput, IntPtr lpInitData);

        [DllImport("gdi32.dll")]
        private static extern bool DeleteDC(IntPtr hdc);

        private const int ENUM_CURRENT_SETTINGS = -1;
        private const int ENUM_REGISTRY_SETTINGS = -2;
        private const int DM_DISPLAYFREQUENCY = 0x400000;
        private const int DM_PELSWIDTH = 0x80000;
        private const int DM_PELSHEIGHT = 0x100000;
        private const int DM_DISPLAYORIENTATION = 0x8000000;
        private const int LOGPIXELSX = 88;
        private const int LOGPIXELSY = 90;

        public DisplaySettingsService() { }

        public List<IEData> ExportDisplaySettings()
        {
            List<IEData> displayDatas = new List<IEData>();

            GetDisplaySettings(displayDatas);

            return displayDatas;
        }

        public void GetDisplaySettings(List<IEData> displayDatas)
        {
            IEData data = null;
            try
            {
                // Получаем информацию о всех дисплеях
                List<DisplayInfo> displays = GetDisplayDevices();
                
                // Количество дисплеев
                data = new IEData("DisplayCount", displays.Count.ToString(), NAME_TYPE_DATA);
                displayDatas.Add(data);

                // Информация о каждом дисплее
                for (int i = 0; i < displays.Count; i++)
                {
                    string prefix = $"Display{i + 1}_";
                    
                    // Название монитора
                    data = new IEData($"{prefix}MonitorName", displays[i].MonitorName, NAME_TYPE_DATA);
                    displayDatas.Add(data);

                    // Разрешение
                    data = new IEData($"{prefix}Resolution", $"{displays[i].Width}x{displays[i].Height}", NAME_TYPE_DATA);
                    displayDatas.Add(data);

                    // Частота обновления
                    data = new IEData($"{prefix}RefreshRate", $"{displays[i].RefreshRate}Hz", NAME_TYPE_DATA);
                    displayDatas.Add(data);

                    // Ориентация
                    data = new IEData($"{prefix}Orientation", displays[i].Orientation, NAME_TYPE_DATA);
                    displayDatas.Add(data);

                    // Масштаб (DPI)
                    data = new IEData($"{prefix}Scale", $"{displays[i].ScalePercent}%", NAME_TYPE_DATA);
                    displayDatas.Add(data);

                    // HDR
                    data = new IEData($"{prefix}HDR", displays[i].HDR ? "Включен" : "Выключен", NAME_TYPE_DATA);
                    displayDatas.Add(data);
                }

                // Ночной свет (глобальная настройка)
                bool nightLightEnabled = GetNightLightStatus();
                data = new IEData("NightLight", nightLightEnabled ? "Включен" : "Выключен", NAME_TYPE_DATA);
                displayDatas.Add(data);

                // Масштаб системы (глобальный)
                int systemScale = GetSystemScale();
                data = new IEData("SystemScale", $"{systemScale}%", NAME_TYPE_DATA);
                displayDatas.Add(data);
            }
            catch (Exception ex)
            {
                Logger logger = new Logger("GetDisplaySettings", ex.Message, logLevel.errorLog);
                logger.Write();
            }
        }

        private List<DisplayInfo> GetDisplayDevices()
        {
            List<DisplayInfo> displays = new List<DisplayInfo>();
            DISPLAY_DEVICE displayDevice = new DISPLAY_DEVICE();
            displayDevice.cb = Marshal.SizeOf(displayDevice);

            uint deviceIndex = 0;
            while (EnumDisplayDevices(null, deviceIndex, ref displayDevice, 0))
            {
                if ((displayDevice.StateFlags & 0x1) != 0) // DISPLAY_DEVICE_ATTACHED_TO_DESKTOP
                {
                    DisplayInfo info = new DisplayInfo
                    {
                        DeviceName = displayDevice.DeviceName,
                        MonitorName = displayDevice.DeviceString
                    };

                    // Получаем настройки дисплея
                    DEVMODE devMode = new DEVMODE();
                    devMode.dmSize = (short)Marshal.SizeOf(typeof(DEVMODE));

                    if (EnumDisplaySettings(displayDevice.DeviceName, ENUM_CURRENT_SETTINGS, ref devMode) != 0)
                    {
                        info.Width = devMode.dmPelsWidth;
                        info.Height = devMode.dmPelsHeight;
                        info.RefreshRate = devMode.dmDisplayFrequency;
                        info.Orientation = GetOrientationString(devMode.dmDisplayOrientation);
                    }

                    // Получаем масштаб (DPI)
                    info.ScalePercent = GetDisplayScale(displayDevice.DeviceName);

                    // Получаем HDR статус
                    info.HDR = GetHDRStatus(displayDevice.DeviceName);

                    displays.Add(info);
                }

                deviceIndex++;
                displayDevice.cb = Marshal.SizeOf(displayDevice);
            }

            return displays;
        }

        private string GetOrientationString(int orientation)
        {
            switch (orientation)
            {
                case 0: return "Альбомная";
                case 1: return "Портретная";
                case 2: return "Альбомная (перевернутая)";
                case 3: return "Портретная (перевернутая)";
                default: return "Неизвестно";
            }
        }

        private int GetDisplayScale(string deviceName)
        {
            try
            {
                // Способ 1: Через Windows API GetDeviceCaps
                IntPtr hdc = CreateDC(null, deviceName, null, IntPtr.Zero);
                if (hdc != IntPtr.Zero)
                {
                    int dpiX = GetDeviceCaps(hdc, LOGPIXELSX);
                    DeleteDC(hdc);
                    
                    // Базовый DPI Windows = 96, масштаб = (DPI / 96) * 100
                    int scale = (int)Math.Round((dpiX / 96.0) * 100);
                    if (scale > 0) return scale;
                }
            }
            catch { }

            try
            {
                // Способ 2: Через реестр для конкретного дисплея
                // Windows хранит настройки масштаба в реестре
                RegistryKey displayKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\DisplaySettings");
                if (displayKey != null)
                {
                    // Ищем настройки для конкретного устройства
                    string[] valueNames = displayKey.GetValueNames();
                    foreach (string valueName in valueNames)
                    {
                        if (valueName.Contains(deviceName) || deviceName.Contains(valueName))
                        {
                            object dpiValue = displayKey.GetValue(valueName);
                            if (dpiValue != null)
                            {
                                int dpi = Convert.ToInt32(dpiValue);
                                displayKey.Close();
                                return (int)Math.Round((dpi / 96.0) * 100);
                            }
                        }
                    }
                    displayKey.Close();
                }
            }
            catch { }

            // Способ 3: Глобальный масштаб системы
            return GetSystemScale();
        }

        private int GetSystemScale()
        {
            try
            {
                // Пробуем получить из реестра
                object value = Registry.GetValue(@"HKEY_CURRENT_USER\Control Panel\Desktop", "LogPixels", null);
                if (value != null)
                {
                    int dpi = Convert.ToInt32(value);
                    return (int)Math.Round((dpi / 96.0) * 100);
                }
            }
            catch { }

            // Значение по умолчанию
            return 100;
        }

        private bool GetNightLightStatus()
        {
            try
            {
                // Способ 1: Через реестр Windows Settings (Windows 10/11)
                object value = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Settings\System", "NightLightEnabled", null);
                if (value != null)
                {
                    return Convert.ToInt32(value) == 1;
                }

                // Способ 2: Через CloudStore (более надежный для Windows 10)
                RegistryKey cloudStoreKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\CloudStore\Store\Cache\DefaultAccount");
                if (cloudStoreKey != null)
                {
                    string[] subKeys = cloudStoreKey.GetSubKeyNames();
                    foreach (string subKey in subKeys)
                    {
                        if (subKey.Contains("bluelightreduction") || subKey.Contains("BlueLightReduction"))
                        {
                            RegistryKey blueLightKey = cloudStoreKey.OpenSubKey(subKey);
                            if (blueLightKey != null)
                            {
                                string[] dataKeys = blueLightKey.GetSubKeyNames();
                                foreach (string dataKey in dataKeys)
                                {
                                    RegistryKey dataSubKey = blueLightKey.OpenSubKey(dataKey);
                                    if (dataSubKey != null)
                                    {
                                        object dataValue = dataSubKey.GetValue("Data");
                                        if (dataValue != null && dataValue is byte[])
                                        {
                                            byte[] data = (byte[])dataValue;
                                            // В бинарных данных ночного света обычно есть флаг включения
                                            // Ищем паттерн, который указывает на включенное состояние
                                            if (data.Length > 20)
                                            {
                                                // Упрощенная проверка: ищем определенные байты
                                                // В реальности структура сложнее, но это работает для большинства случаев
                                                for (int i = 0; i < Math.Min(data.Length - 1, 50); i++)
                                                {
                                                    if (data[i] == 0x01 && i + 1 < data.Length && data[i + 1] == 0x01)
                                                    {
                                                        dataSubKey.Close();
                                                        blueLightKey.Close();
                                                        cloudStoreKey.Close();
                                                        return true;
                                                    }
                                                }
                                            }
                                        }
                                        dataSubKey.Close();
                                    }
                                }
                                blueLightKey.Close();
                            }
                        }
                    }
                    cloudStoreKey.Close();
                }

                // Способ 3: Через Windows Registry напрямую
                value = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CloudStore\Store\Cache\DefaultAccount\$windows.data.bluelightreduction.settings\Windows.Data.BlueLightReduction.Settings", "Data", null);
                if (value != null && value is byte[])
                {
                    byte[] data = (byte[])value;
                    if (data.Length > 10)
                    {
                        // Проверяем наличие флага включения
                        return data[data.Length - 1] == 0x01 || data[0] == 0x01;
                    }
                }
            }
            catch { }

            return false;
        }

        private bool GetHDRStatus(string deviceName)
        {
            try
            {
                // Способ 1: Проверяем через реестр для конкретного устройства
                string keyPath = @"SYSTEM\CurrentControlSet\Enum\DISPLAY";
                RegistryKey baseKey = Registry.LocalMachine.OpenSubKey(keyPath);
                
                if (baseKey != null)
                {
                    foreach (string subKeyName in baseKey.GetSubKeyNames())
                    {
                        RegistryKey subKey = baseKey.OpenSubKey(subKeyName);
                        if (subKey != null)
                        {
                            foreach (string deviceSubKey in subKey.GetSubKeyNames())
                            {
                                string deviceParamsPath = deviceSubKey + "\\Device Parameters";
                                RegistryKey deviceKey = subKey.OpenSubKey(deviceParamsPath);
                                if (deviceKey != null)
                                {
                                    // Проверяем поддержку HDR
                                    object hdrSupport = deviceKey.GetValue("HDRSupport");
                                    if (hdrSupport != null && Convert.ToInt32(hdrSupport) == 1)
                                    {
                                        deviceKey.Close();
                                        subKey.Close();
                                        baseKey.Close();
                                        return true;
                                    }

                                    // Альтернативная проверка через EDID
                                    object edid = deviceKey.GetValue("EDID");
                                    if (edid != null && edid is byte[])
                                    {
                                        byte[] edidData = (byte[])edid;
                                        // В EDID есть информация о поддержке HDR10
                                        // Проверяем определенные байты в EDID структуре
                                        if (edidData.Length > 250)
                                        {
                                            // Проверяем блок расширений EDID для HDR метаданных
                                            for (int i = 128; i < edidData.Length - 1; i += 128)
                                            {
                                                if (edidData[i] == 0x02 && edidData[i + 1] == 0x03) // CEA Extension Block
                                                {
                                                    // Ищем HDR метаданные в CEA блоке
                                                    if (i + 3 < edidData.Length && (edidData[i + 3] & 0x20) != 0)
                                                    {
                                                        deviceKey.Close();
                                                        subKey.Close();
                                                        baseKey.Close();
                                                        return true;
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    deviceKey.Close();
                                }
                            }
                            subKey.Close();
                        }
                    }
                    baseKey.Close();
                }

                // Способ 2: Проверяем через настройки Windows (Windows 10/11)
                RegistryKey displaySettings = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Settings\System");
                if (displaySettings != null)
                {
                    object hdrEnabled = displaySettings.GetValue("HDREnabled");
                    if (hdrEnabled != null)
                    {
                        bool result = Convert.ToInt32(hdrEnabled) == 1;
                        displaySettings.Close();
                        return result;
                    }
                    displaySettings.Close();
                }

                // Способ 3: Проверяем через Windows Advanced Color Settings
                RegistryKey advancedColor = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Settings\System");
                if (advancedColor != null)
                {
                    object advancedColorEnabled = advancedColor.GetValue("AdvancedColorEnabled");
                    if (advancedColorEnabled != null)
                    {
                        bool result = Convert.ToInt32(advancedColorEnabled) == 1;
                        advancedColor.Close();
                        return result;
                    }
                    advancedColor.Close();
                }
            }
            catch { }

            return false;
        }

        private class DisplayInfo
        {
            public string DeviceName { get; set; }
            public string MonitorName { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
            public int RefreshRate { get; set; }
            public string Orientation { get; set; }
            public int ScalePercent { get; set; }
            public bool HDR { get; set; }
        }
    }
}
