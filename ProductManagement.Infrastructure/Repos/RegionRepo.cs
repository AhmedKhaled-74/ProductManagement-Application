using Microsoft.EntityFrameworkCore;
using ProductManagement.Application.Errors;
using ProductManagement.Application.RepoContracts;
using ProductManagement.Domain.Entities;
using ProductManagement.Infrastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Infrastructure.Repos
{
    public class RegionRepo : IRegionRepo
    {
        private readonly AppDbContext _dbContext;

        public RegionRepo(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddRegionPropsAsync(Region region)
        {
            await _dbContext.Regions.AddAsync(region);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteRegionPropsAsync(Region region)
        {
            _dbContext.Regions.Remove(region);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Region>> GetAllRegionPropsAsync()
        {
          return await _dbContext.Regions.ToListAsync();
        }

        public async Task<Region?> GetRegionPropsByNameAsync(string regionName)
        {
           return await _dbContext.Regions
                .FirstOrDefaultAsync(r => r.RegionName.ToLower() == regionName.ToLower());

        }

        public async Task<Region?> GetRegionPropsByIdAsync(Guid Id)
        {
            return await _dbContext.Regions
                 .FirstOrDefaultAsync(r => r.RegionId == Id);
        }
        public async Task UpdateRegionPropsAsync(Region region ,Region exsistRegion)
        {

            region.RegionName = exsistRegion.RegionName;
            region.PoeRegion = exsistRegion.PoeRegion;
            region.ConstTax = exsistRegion.ConstTax;

            _dbContext.Regions.Update(region);
            await _dbContext.SaveChangesAsync();
        }
    }
}
