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
        private readonly uiService _uiService;
        public IEService() 
        {
            _IEDatas = new List<IEData>();

            _systemSettings = new SystemSettings();
            _uiService = new uiService();
        }

        public void ImportData()
        {

        }

        public void ExportData()
        {
            _IEDatas.Clear();

            _IEDatas.Concat(_systemSettings.ExportSystemSettings()); //Системные настройки
            _IEDatas.Concat(_uiService.ExportUI());
        }

    }
}
