using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.DTOs
{
    public class RegionDTO
    {
        public Guid RegionId { get; set; }
        public string RegionName { get; set; } = null!;
        public decimal PoeRegion { get; set; } 
        public decimal ConstTax { get; set; } 
    }
    public class RegionAddDTO
    {
        public string RegionName { get; set; } = null!;
        public decimal PoeRegion { get; set; } 
        public decimal ConstTax { get; set; } 
    }
    public class RegionUpdateDTO
    {
        public Guid RegionId { get; set; }
        public string RegionName { get; set; } = null!;
        public decimal PoeRegion { get; set; } 
        public decimal ConstTax { get; set; } 
    }
}
