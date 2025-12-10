using ProductManagement.Application.DTOs.ProductDTOs.Result;

namespace ProductManagement.Application.DTOs.CartDTOs
{
    public class CartProductDTO
    {
        public Guid CartId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string ProductName { get; set; } = null!;
        public string ProductDescription { get; set; } = null!;
        public string VendorName { get; set; } = null!;
        public string? BrandName { get; set; }
        public string ProductCategoryName { get; set; } = null!;
        public string? ProductSubCategoryName { get; set; } = null!;
        // changed props
        public decimal ActualProductPrice { get; set; }
        public decimal Rate { get; set; }
        public int TotalRatedUsers { get; set; }
        public string IconUrl { get; set; } = null!;
        public List<ProductsCustomAttributeResult>? ProductCustomAttributes { get; set; }

    }
}
