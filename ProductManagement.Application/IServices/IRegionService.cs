using ErrorOr;
using MediatR;
using ProductManagement.Application.DTOs;
using ProductManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.IServices
{
    public interface IRegionService
    {

        Task<ErrorOr<RegionDTO>> GetRegionPropsByIdAsync(Guid? Id);
        Task<ErrorOr<RegionDTO>> GetRegionPropsByNameAsync(string? regionName);
        Task<List<RegionDTO>> GetAllRegionPropsAsync();
        Task<ErrorOr<Unit>> AddRegionPropsAsync(RegionAddDTO? region);
        Task<ErrorOr<Unit>> UpdateRegionPropsAsync(RegionUpdateDTO? region);
        Task<ErrorOr<Unit>> DeleteRegionPropsByIdAsync(Guid? Id);
    }
}
