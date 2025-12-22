using ErrorOr;
using MediatR;
using ProductManagement.Application.HttpClients;
using ProductManagement.Domain.Entities;
using System.Threading.Tasks;

namespace ProductManagement.Application.Helpers
{
    public static class VendorAuthorizationHelper
    {
        public static async Task<ErrorOr<Unit>?> ValidateVendorAsync(UserMicroClient userClient, Guid vendorId, Guid requestUserId)
        {
            var vendor = await userClient.GetUserById(vendorId);
            if (vendor == null)
                return Errors.Errors.VendorErrors.VendorNotFound;
            if (vendor.RoleName != "Vendor")
                return Errors.Errors.VendorErrors.UserIsNotVendor;
            if (vendor.UserId != requestUserId)
                return Errors.Errors.VendorErrors.VendorDoesntHaveProductAuthorize;
            return null;
        }
    }
}
