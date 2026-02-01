using ProductManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.DTOs.ProductDTOs.AddRequest
{
    public class ProductAddRequest
    {
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
        [Range(0, 100)]
        public decimal? Discount { get; set; } = 0;
        [Required]
        public Guid VendorId { get; set; }
        public Guid? BrandId { get; set; }
        [Required]
        public Guid ProductCategoryId { get; set; }
        public Guid? ProductSubCategoryId { get; set; }
        [Required]
        public int NumberOfAddingProducts { get; set; }
        [Required]
        public List<ProductMediaAddRequestByAddProduct> ProductMediaAddRequests { get; set; } = new List<ProductMediaAddRequestByAddProduct>();
        public List<ProductCustomAttributeAddRequest>? ProductCustomAttributeAddRequests { get; set; }
    }
}
