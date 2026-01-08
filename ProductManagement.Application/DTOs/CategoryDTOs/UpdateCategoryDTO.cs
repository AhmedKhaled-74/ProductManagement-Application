using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.DTOs.CategoryDTOs
{
    public class UpdateCategoryDTO
    {
        [Required]
        public Guid CategoryId { get; set; }
        [Required]
        public string CategoryName { get; set; } = null!;
        [Required]
        public string CategoryDescription { get; set; } = null!;
        [Required]
        public string CategoryImageUrl { get; set; } = null!;
    }
}
