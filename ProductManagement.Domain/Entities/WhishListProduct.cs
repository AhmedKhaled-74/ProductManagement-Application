using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Domain.Entities
{
    public class WhishListProduct
    {

        public Guid WhishListId { get; set; }
        public Guid ProductId { get; set; }

        // navigation props
        public virtual WhishList WhishList { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }

}
