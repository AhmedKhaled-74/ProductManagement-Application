using ProductManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.DTOs.CartDTOs
{
    public class CartDTO
    {
        public Guid CartId { get; set; }
        public Guid UserId { get; set; }
        public  List<CartProductDTO>? CartProducts { get; set; }
    }
}
