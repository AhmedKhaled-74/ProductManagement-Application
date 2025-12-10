namespace ProductManagement.Application.DTOs.ProductDTOs.Result
{
    public class ProductsCustomAttributeResult
    {
        public Guid ProductCustomAttributeId { get; set; }
        public string Type { get; set; } = null!;
        public string Attribute { get; set; } = null!;
    }
}
