using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Domain.Entities
{
    public class CartProduct
    {

        public Guid CartId { get; set; }
        public Guid ProductId { get; set; }

        public int Quantity { get; set; }

        // navigation props
        public virtual Cart Cart { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }

}
