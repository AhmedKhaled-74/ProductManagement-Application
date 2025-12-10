using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Domain.Entities
{
    public class Product
    {
        [Key]
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string ProductDescription { get; set; } = null!;
        public decimal ProductMass { get; set; }
        public decimal ProductVolume { get; set; }
        public decimal ProductPrice { get; set; }
        public Guid VendorId { get; set; }
        public Guid? BrandId { get; set; }
        public Guid ProductCategoryId { get; set; }
        public Guid? ProductSubCategoryId { get; set; }
        public int NumberOfRemainingProducts { get; set; }
        public int SoldTimes { get; set; }
        public decimal Discount { get; set; }
        public int TotalRatedUsers { get; set; }
        public int TotalStars { get; set; }


        // navigation props
        public virtual ICollection<ProductMedia> ProductMedias { get; set; } = [];
        public virtual ICollection<ProductCustomAttribute>? ProductCustomAttributes { get; set; }
        public virtual Category ProductCategory { get; set; } = null!;
        public virtual Vendor Vendor { get; set; } = null!;
        public virtual SubCategory? ProductSubCategory { get; set; }
        public virtual Brand? ProductBrand { get; set; }
        public virtual ICollection<Review>? Reviews { get; set; }
        public virtual ICollection<WhishListProduct>? WhishListProducts { get; set; }
        public virtual ICollection<CartProduct>? CartProducts { get; set; }

    }

}
