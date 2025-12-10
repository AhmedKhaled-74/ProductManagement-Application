using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Application.DTOs.ProductDTOs.UpdateRequest
{
    public class ProductReviewUpdateRequest
    {
        [Required]
        public Guid ReviewId { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public Guid ProductId { get; set; }
        public DateTime? FeedBackCreatedAt { get; set; }
        public string? FeedBack { get; set; }
        [Required]
        public int Rate { get; set; }
    }
}
