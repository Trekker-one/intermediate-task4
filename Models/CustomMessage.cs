using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Consummer_api.Models
{
    public class CustomMessage
    {
        public CustomTime time { get; set; }
        public string disclaimer { get; set; }
        public string chartName { get; set; }
        public CustomBpi bpi { get; set; }
    }
}
