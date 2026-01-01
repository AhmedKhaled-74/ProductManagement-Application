using ProductManagement.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Presentation.Models
{
    public class AddToCartRequest
    {
        public List<Guid>? CustomAttIds { get; set; }
        public PriceConstsSetup? ConstsSetup { get; set; }
    }
}
