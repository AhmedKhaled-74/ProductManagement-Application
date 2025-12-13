namespace ProductManagement.Application.DTOs.ReviewDTOs
{
    public class AddReviewReactionRequest
    {
        public Guid ReviewId { get; set; }
        public bool IsLike { get; set; }
    }
}
