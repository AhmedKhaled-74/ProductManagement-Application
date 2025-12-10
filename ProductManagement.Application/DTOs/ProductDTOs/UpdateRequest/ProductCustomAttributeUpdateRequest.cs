using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Application.DTOs.ProductDTOs.UpdateRequest
{
    public class ProductCustomAttributeUpdateRequest
    {
        [Required]
        public Guid ProductCustomAttributeId { get; set; }
        [Required]
        public string Type { get; set; } = null!;
        [Required]
        public Guid ProductId { get; set; }
        [Required]
        public string Attribute { get; set; } = null!;
    }
}
