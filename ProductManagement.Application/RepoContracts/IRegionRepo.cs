using ProductManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.RepoContracts
{
    public interface IRegionRepo
    {
        Task<Region?> GetRegionPropsByNameAsync(string regionName);
        Task<Region?> GetRegionPropsByIdAsync(Guid Id);
        Task<List<Region>> GetAllRegionPropsAsync();
        Task AddRegionPropsAsync(Region region);
        Task UpdateRegionPropsAsync(Region region, Region exsistRegion);
        Task DeleteRegionPropsAsync(Region region);
    }
}
