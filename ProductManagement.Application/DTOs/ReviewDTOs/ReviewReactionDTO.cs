namespace ProductManagement.Application.DTOs.ReviewDTOs
{
    public class ReviewReactionDTO
    {
        public Guid ReviewId { get; set; }
        public int LikesCount { get; set; }
        public int DislikesCount { get; set; }
        public bool? UserReaction { get; set; } // null = no reaction, true = like, false = dislike
    }
}
