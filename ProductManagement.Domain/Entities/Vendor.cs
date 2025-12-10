namespace ProductManagement.Domain.Entities
{
    public class Vendor
    {
        public Guid VendorId { get; set; }
        public string VendorName { get; set; } = null!;
        public virtual ICollection<Product>? Products { get; set; }
        public virtual ICollection<Brand>? Brands { get; set; }

    }

}
