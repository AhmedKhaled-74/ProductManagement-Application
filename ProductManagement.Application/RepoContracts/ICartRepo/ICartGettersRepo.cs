using ProductManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.RepoContracts.ICartRepo
{
    public interface ICartGettersRepo
    {
        // Define methods for getting cart data here
        Task<Cart?> GetCartByIdsAsync(Guid userId , Guid cartId);
        Task<Cart?> GetCartByUserIdAsync(Guid userId);

    }
}
