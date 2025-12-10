using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Application.DTOs.ProductDTOs.AddRequest
{
    public class ProductCustomAttributeAddRequest
    {
        [Required]
        public string Type { get; set; } = null!;

        public Guid ProductId { get; set; }
        [Required]
        public string Attribute { get; set; } = null!;
    }
}
