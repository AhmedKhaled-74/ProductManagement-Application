using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Domain.Entities
{
    public class Region
    {
        [Key]
        public Guid RegionId { get; set; }
        public string RegionName { get; set; } = null!;
        public decimal PoeRegion { get; set; } 
        public decimal ConstTax { get; set; } 
    }

}
