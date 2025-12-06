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
    public class UIService
    {
        const string NAME_TYPE_DATA = "UISettings";
        public UIService() {}
        public List<IEData> ExportUI()
        {
            List<IEData> UISettings = new List<IEData>();
            //-------Тут методы пишешь
            return UISettings;
        }

    }
}
