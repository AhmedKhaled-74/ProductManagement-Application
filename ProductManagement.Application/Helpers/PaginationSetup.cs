using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.Helpers
{
        public class PaginationSetup
        {
        public int? UserPageSize { get; set; } = 10;
        public int? AdminPageSize { get; set; } = 100;
        }
    
}
