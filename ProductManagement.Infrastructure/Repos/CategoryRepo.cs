using Microsoft.EntityFrameworkCore;
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
    public class CategoryRepo : ICategoryRepo
    {
        private readonly AppDbContext _dbContext;

        public CategoryRepo(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task CreateCategory(Category category)
        {
            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            return await _dbContext.Categories
                .Include(c=>c.SubCategories).ToListAsync();
        }
        public async Task<bool> DoesCategoryExistAsync(Guid categoryId)
        {
            return await _dbContext.Categories
                .AnyAsync(c => c.CategoryId == categoryId);
        }
        public async Task<Category?> GetCategoryById(Guid categoryId)
        {
            return await _dbContext.Categories
                .Include(c => c.SubCategories)
                .FirstOrDefaultAsync(c => c.CategoryId == categoryId);
        }
        public async Task DeleteCategory(Guid categoryId)
        {
            var category = await _dbContext.Categories
                .FirstOrDefaultAsync(c => c.CategoryId == categoryId);
            if (category != null)
            {
                _dbContext.Categories.Remove(category);
                await _dbContext.SaveChangesAsync();
            }
        }
        public async Task<bool> DoesSubCategoryExistAsync(Guid subCategoryId)
        {
            return await _dbContext.SubCategories
                .AnyAsync(c => c.SubCategoryId == subCategoryId);
        }
    }
}
