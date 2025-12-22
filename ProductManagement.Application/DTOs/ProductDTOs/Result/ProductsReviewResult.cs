namespace ProductManagement.Application.DTOs.ProductDTOs
{
    public class ProductsReviewResult
    {
        public Guid ReviewId { get; set; }
        public Guid UserId { get; set; }
        public DateTime? FeedBackCreatedAt { get; set; }
        public string? FeedBack { get; set; }
        public int Rate { get; set; }
    }
}
