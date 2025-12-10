using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Domain.Entities
{
    public class Brand
    {
        [Key]
        public Guid BrandId { get; set; }
        public string BrandName { get; set; } = null!;
        public virtual ICollection<Product>? Products { get; set; }
        public virtual ICollection<Vendor>? Vendors { get; set; }


    }

}
