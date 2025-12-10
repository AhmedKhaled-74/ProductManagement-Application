using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Application.DTOs.ProductDTOs.UpdateRequest
{
    public class ProductUpdateRequest
    {
        [Required]
        public Guid ProductId { get; set; } 

        [Required]
        public string ProductName { get; set; } = null!;
        [Required]
        public string ProductDescription { get; set; } = null!;
        [Required]
        public decimal ProductMass { get; set; }
        [Required]
        public decimal ProductVolume { get; set; }
        [Required]
        public decimal ProductPrice { get; set; }
        public decimal? Discount { get; set; } = 0;
        [Required]
        public Guid VendorId { get; set; }
        public Guid? BrandId { get; set; }
        [Required]
        public Guid ProductCategoryId { get; set; }
        public Guid? ProductSubCategoryId { get; set; }
    }
}
