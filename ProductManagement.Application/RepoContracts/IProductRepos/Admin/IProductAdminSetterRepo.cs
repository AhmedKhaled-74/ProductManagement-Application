using ProductManagement.Domain.Entities;
namespace ProductManagement.Application.RepoContracts.IProductRepos.Admin
{
    public interface IProductAdminSetterRepo
    {
        Task ApproveProductAsync(Guid productId);
    }
}
