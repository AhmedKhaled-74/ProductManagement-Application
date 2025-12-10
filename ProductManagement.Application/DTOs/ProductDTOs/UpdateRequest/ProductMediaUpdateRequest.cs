using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Application.DTOs.ProductDTOs.UpdateRequest
{
    public class ProductMediaUpdateRequest
    {
        [Required]
        public Guid ProductMediaId { get; set; }
        [Required]
        public Guid ProductId { get; set; }
        [Required]
        public string Type { get; set; } = null!;
        [Url]
        [Required]
        [MaxLength(500)]
        public string ProductMediaURL { get; set; } = null!;
    }
}
