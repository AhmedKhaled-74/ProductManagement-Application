namespace ProductManagement.Application.DTOs.ProductDTOs.Result
{
    public class ProductDetailedResult
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string ProductDescription { get; set; } = null!;
        public decimal ProductMass { get; set; }
        public decimal ProductVolume { get; set; }
        public Guid VendorId { get; set; }
        public string VendorName { get; set; } = null!;
        public Guid? BrandId { get; set; }
        public string? BrandName { get; set; }
        public Guid ProductCategoryId { get; set; }
        public string ProductCategoryName { get; set; } = null!;
        public Guid? ProductSubCategoryId { get; set; }
        public string? ProductSubCategoryName { get; set; } = null!;
        public int NumberOfRemainingProducts { get; set; }
        public int SoldTimes { get; set; }
        public decimal Discount { get; set; }
        // changed props
        public decimal ActualProductPrice { get; set; }
        public decimal Rate { get; set; }
        public int TotalRatedUsers { get; set; }
        public string IconUrl { get; set; } = null!;
        public List<ProductMediaResult> ProductMediaResults { get; set; } = [];
        public List<ProductsCustomAttributeResult>? ProductsCustomAttributesResults { get; set; }
        public List<ProductsReviewResult>? ProductsReviewsResults { get; set; }


    }

}
