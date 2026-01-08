namespace ProductManagement.Application.DTOs.CategoryDTOs
{
    public class SubCategoryResultDTO
    {
        public Guid SubCategoryId { get; set; }
        public Guid CategoryId { get; set; }
        public string SubCategoryName { get; set; } = null!;
    }
    public class AddSubCategoryDTO
    {
        public Guid CategoryId { get; set; }
        public string SubCategoryName { get; set; } = null!;
    }
    public class UpdateSubCategoryDTO
    {
        public Guid SubCategoryId { get; set; }
        public Guid CategoryId { get; set; }
        public string SubCategoryName { get; set; } = null!;
    }

}