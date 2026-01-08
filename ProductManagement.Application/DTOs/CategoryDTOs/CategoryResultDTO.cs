using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.DTOs.CategoryDTOs
{
    public class CategoryResultDTO
    {
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        public string CategoryDescription { get; set; } = null!;
        public string CategoryImageUrl { get; set; } = null!;

    }
    public class CategoryDetailedResultDTO
    {
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        public string CategoryDescription { get; set; } = null!;
        public string CategoryImageUrl { get; set; } = null!;
        public List<SubCategoryResultDTO>? SubCategories { get; set; }

    }
}
