using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Domain.Entities
{
    public class Review
    {
        public Guid ReviewId { get; set; }
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
        public int? Likes { get; set; } 
        public int? Dislikes { get; set; } 
        public DateTime? FeedBackCreatedAt { get; set; }
        public string? FeedBack { get; set; }
        public int Rate { get; set; }

        // navigation props
        public virtual Product Product { get; set; } = null!;
    }

    }
