using PCMover.Models;
using System;
using System.Collections.Generic;
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
        public List<IEData> ImportSystemSettings()
        {
            List<IEData> systemDatas = new List<IEData>();
            IEData data = null;

            var region = System.Globalization.RegionInfo.CurrentRegion;
            data = new IEData("Region", region.EnglishName, NAME_TYPE_DATA);
            systemDatas.Add(data);

            var ui = System.Globalization.CultureInfo.CurrentUICulture;
            data = new IEData("Lang", ui.Name, NAME_TYPE_DATA);
            systemDatas.Add(data);

            return systemDatas;
        }
    }
}
