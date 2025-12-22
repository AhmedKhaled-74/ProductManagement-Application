using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.DTOs.ProductDTOs.Result
{
    public class ProductVendorResult
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string ProductDescription { get; set; } = null!;
        public Guid? BrandId { get; set; }
        public string? BrandName { get; set; }
        public Guid ProductCategoryId { get; set; }
        public string ProductCategoryName { get; set; } = null!;
        public Guid? ProductSubCategoryId { get; set; }
        public string? ProductSubCategoryName { get; set; } = null!;
        public int NumberOfRemainingProducts { get; set; }
        public int SoldTimes { get; set; }
        public decimal Discount { get; set; }
        public bool IsApproved { get; set; }
        // changed props
        public decimal ProductPrice { get; set; }
        public decimal Rate { get; set; }
        public int TotalRatedUsers { get; set; }
        public string IconUrl { get; set; } = null!;
    }
}
