using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCMover.Models
{
    public class IEData
    {
        public string Title { get; set; }
        public string Data { get; set; }
        public string Type { get; set; }

        IEData()
        {

        }

        public IEData(string title, string data, string type)
        {
            Title = title;
            Data = data;
            Type = type;
        }
    }
}
