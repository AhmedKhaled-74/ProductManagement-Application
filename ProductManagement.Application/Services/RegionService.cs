using ErrorOr;
using MediatR;
using ProductManagement.Application.DTOs;
using ProductManagement.Application.Errors;
using ProductManagement.Application.IServices;
using ProductManagement.Application.Mappers;
using ProductManagement.Application.RepoContracts;
using ProductManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ProductManagement.Application.Errors.Errors;

namespace ProductManagement.Application.Services
{
    public class RegionService : IRegionService
    {
        private readonly IRegionRepo _regionRepo;

        public RegionService(IRegionRepo regionRepo)
        {
            _regionRepo = regionRepo;
        }
        public async Task<ErrorOr<Unit>> AddRegionPropsAsync(RegionAddDTO? region)
        {
            if (region == null) return RegionErrors.RegionObjectRequired;

            if ( await _regionRepo.GetRegionPropsByNameAsync(region.RegionName) is not null) 
            {
                return RegionErrors.DuplicatedRegion;
            }

            await _regionRepo.AddRegionPropsAsync(region.ToRegionEntity());
            return Unit.Value;

        }

        public async Task<ErrorOr<Unit>> DeleteRegionPropsByIdAsync(Guid? Id)
        {
            if (!Id.HasValue || Id == null) return RegionErrors.RegionIdRequired;

            if (await _regionRepo.GetRegionPropsByIdAsync(Id.Value) is Region region and not null)
            {
                await _regionRepo.DeleteRegionPropsAsync(region);
                return Unit.Value;
            }

            return RegionErrors.RegionNotFound;
        }

        public async Task<List<RegionDTO>> GetAllRegionPropsAsync()
        {

            var regions = await _regionRepo.GetAllRegionPropsAsync();
            return regions.Select(r=>r.ToRegionResult()).ToList();
        }

        public async Task<ErrorOr<RegionDTO>> GetRegionPropsByIdAsync(Guid? Id)
        {
            if (!Id.HasValue || Id == null) return RegionErrors.RegionIdRequired;

            if (await _regionRepo.GetRegionPropsByIdAsync(Id.Value) is Region region and not null)
            {
                return region.ToRegionResult();
            }
            return RegionErrors.RegionNotFound;
            
        }

        public async Task<ErrorOr<RegionDTO>> GetRegionPropsByNameAsync(string? regionName)
        {
            if (regionName == null) return RegionErrors.RegionObjectRequired;

            if (await _regionRepo.GetRegionPropsByNameAsync(regionName) is Region region and not null)
            {
                return region.ToRegionResult();
            }
            return RegionErrors.RegionNotFound;

        }

        public async Task<ErrorOr<Unit>> UpdateRegionPropsAsync(RegionUpdateDTO? region)
        {
            if (region == null) return RegionErrors.RegionObjectRequired;
            if (await _regionRepo.GetRegionPropsByIdAsync(region.RegionId) is Region regionExsist and not null)
            {
                await _regionRepo.UpdateRegionPropsAsync(region.ToRegionEntityFromUpdate() , regionExsist);
                return Unit.Value;
            }
            return RegionErrors.RegionNotFound;
        }
    }
}
