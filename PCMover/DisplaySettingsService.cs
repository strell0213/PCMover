using PCMover.Models;
using PCMover.SimpleLogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCMover
{
    public class DisplaySettingsService
    {
        const string NAME_TYPE_DATA = "DisplaySettings";

        public DisplaySettingsService() { }

        public List<IEData> ExportDisplaySettings()
        {
            List<IEData> displayDatas = new List<IEData>();
            
            

            return displayDatas;
        }

        public void GetDisplaySettings(List<IEData> displayDatas)
        {
            IEData data = null;
            try
            {
                
            }
            catch (Exception ex)
            {
                Logger logger = new Logger("GetDisplaySettings",ex.Message,logLevel.errorLog);
                logger.Write();
            }
        }


    }
}
