using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.Helpers
{
    public class PriceConstsSetup
    {
        public decimal PoeRegion {  get; set; }
        public decimal CMass { get; set; } = 1.2m;
        public decimal CVol { get; set; } = 0.5m;
        public decimal CTax { get; set; }
    }
}
