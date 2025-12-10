using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Domain.Entities
{
    public class ProductMedia
    {
        [Key]
        public Guid ProductMediaId { get; set; }
        public string Type { get; set; } = null!;
        public Guid ProductId { get; set; }
        [Url]
        [Required]
        [MaxLength(500)]
        public string ProductMediaURL { get; set; } = null!;

        // navigation props
        public virtual Product Product { get; set; } = null!;
    }

    }
