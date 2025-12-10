using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Domain.Entities
{
    public class Cart
    {
        [Key]
        public Guid CartId { get; set; }
        public Guid UserId { get; set; }

        // navigation props
        public virtual ICollection<CartProduct>? CartProducts { get; set; }
    }

}
