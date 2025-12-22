using ProductManagement.Domain.Entities;
namespace ProductManagement.Application.RepoContracts.IProductRepos.User
{
    public interface IProductUserSetterRepo
    {
        Task AddProductReviewAsync(Review reviewRequest);
        Task UpdateProductReviewAsync(Guid reviewId, Review reviewRequest);
    }
}
