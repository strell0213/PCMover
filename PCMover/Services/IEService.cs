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

        private readonly SystemSettingsService _systemSettings;
        private readonly UIService _uiService;
        public IEService() 
        {
            _IEDatas = new List<IEData>();

            _systemSettings = new SystemSettingsService();
            _uiService = new UIService();
        }

        public void ImportData()
        {

        }

        public void ExportData()
        {
            _IEDatas.Clear();

            _IEDatas.Concat(_systemSettings.ExportSystemSettings()); //Системные настройки
            _IEDatas.Concat(_uiService.ExportUI()); //Настройки UI


        }

        public void SaveDataListInFile()
        {
            //Пользователь выбирает путь для сохранения настроек

            foreach (var data in _IEDatas)
            {
                //data - одна настройка
                //Сохранение настройки в файл ПОСТРОЧНО
            }
        }

        public void LoadDataListFrom()
        {

        }
    }
}
