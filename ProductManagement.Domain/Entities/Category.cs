using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Domain.Entities
{
    public class Category
    {
        [Key]
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;

        // navigation props
        public virtual ICollection<SubCategory>? SubCategories { get; set; }
        public virtual ICollection<Product>? Products { get; set; }

    }

}
