using ProductManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.RepoContracts
{
    public interface ICategoryRepo
    {
        public Task CreateCategory(Category category);
        public Task<IEnumerable<Category>> GetAllCategories();
        Task<bool> DoesCategoryExistAsync(Guid categoryId);
        Task<bool> DoesSubCategoryExistAsync(Guid subCategoryId);
    }
}
