using ProductManagement.Application.DTOs.CartDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.DTOs
{
    public class WishListDTO
    {
        public Guid WhishListId { get; set; }
        public Guid UserId { get; set; }
        public List<WishListProductsDTO>? WishListProducts { get; set; }
    }
    public class WishListProductsDTO
    {
        public Guid ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public decimal ProductPrice { get; set; }
        public string? ProductImageUrl { get; set; }
    }
}
