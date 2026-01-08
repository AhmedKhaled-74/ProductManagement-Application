using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.DTOs.CategoryDTOs
{
    public class AddCategoryDTO
    {
        [Required(ErrorMessage = "Category name is required.")]
        public string CategoryName { get; set; } = null!;
        [Required(ErrorMessage = "Description is required.")]
        public string CategoryDescription { get; set; } = null!;
        [Required(ErrorMessage = "Category image URL is required.")]
        public string CategoryImageUrl { get; set; } = null!;
    }
}
