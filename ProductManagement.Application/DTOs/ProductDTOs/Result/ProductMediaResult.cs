namespace ProductManagement.Application.DTOs.ProductDTOs.Result
{
    public class ProductMediaResult
    {
        public Guid ProductMediaId { get; set; }
        public string Type { get; set; } = null!;
        public string ProductMediaURL { get; set; } = null!;

    }

}
