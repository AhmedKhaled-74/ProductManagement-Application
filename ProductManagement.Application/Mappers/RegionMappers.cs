using Mapster;
using ProductManagement.Application.DTOs;
using ProductManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.Mappers
{
    public static class RegionMappers
    {
        public static RegionDTO ToRegionResult(this Region region)
        {
            return region.Adapt<RegionDTO>();
        }
        public static Region ToRegionEntity(this RegionAddDTO region)
        {
            return region.Adapt<Region>();
        }
        public static Region ToRegionEntityFromUpdate(this RegionUpdateDTO region)
        {
            return region.Adapt<Region>();
        }

        
    }
}
