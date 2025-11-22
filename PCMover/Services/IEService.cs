using PCMover.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCMover.Services
{
    public class IEService
    {
        private List<IEData> _IEDatas;

        private readonly SystemSettings _systemSettings;
        public IEService() 
        {
            _IEDatas = new List<IEData>();

            _systemSettings = new SystemSettings();
        }

        public void ImportData()
        {

        }

        public void ExportData()
        {
            _IEDatas.Clear();

            _IEDatas.Concat(_systemSettings.ExportSystemSettings()); //Системные настройки
        }

    }
}
