using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Domain.Entities
{
    public class WhishList
    {
        [Key]
        public Guid WhishListId { get; set; }
        public Guid UserId { get; set; }

        // navigation props
        public virtual ICollection<WhishListProduct>? WhishListProducts { get; set; }
    }

}
