using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Consummer_api.Models
{
    public class CustomCurrency
    {
        public string code { get; set; }
        public string symbol { get; set; }
        public string rate { get; set; }
        public string description { get; set; }
        public float rate_float { get; set; }
    }
}
