using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Domain.Entities
{
    public class SubCategory
    {
        [Key]
        public Guid SubCategoryId { get; set; }
        public string SubCategoryName { get; set; } = null!;
        public Guid CategoryId { get; set; }
        // navigation props
        public virtual Category Category { get; set; } = null!;
        public virtual ICollection<Product>? Products { get; set; }

    }

}
