using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Domain.Entities
{
    public class ProductCustomAttribute
    {
        [Key]
        public Guid ProductCustomAttributeId { get; set; }
        public string Type { get; set; } = null!;
        public Guid ProductId { get; set; }
        public string Attribute { get; set; } = null!;

        // navigation props
        public virtual Product Product { get; set; } = null!;
    }

}
