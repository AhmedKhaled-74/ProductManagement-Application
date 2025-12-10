using ProductManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.DTOs.ProductDTOs.AddRequest
{
    public class ProductMediaAddRequest
    {
        [Required]
        public string Type { get; set; } = null!;
        
        public Guid ProductId { get; set; }
        [Url]
        [Required]
        [MaxLength(500)]
        public string ProductMediaURL { get; set; } = null!;
    }
}
