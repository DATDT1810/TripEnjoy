using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripEnjoy.Application.Interface.Category;
using TripEnjoy.Domain.Models;
namespace TripEnjoy.Application.Services.Category
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<TripEnjoy.Domain.Models.Category>> GetCategoriesAsync()
        {
            return await _categoryRepository.GetCategoriesAsync();
        }

        public async Task<TripEnjoy.Domain.Models.Category> GetCategoryByIdAsync(int id)
        {
            return await _categoryRepository.GetCategoryByIdAsync(id);
        }

        public async Task<TripEnjoy.Domain.Models.Category> AddCategoryAsync(string categoryName)
        {
            return await _categoryRepository.AddCategoryAsync(categoryName);
        }

        public async Task<TripEnjoy.Domain.Models.Category> UpdateCatgoryAsync(TripEnjoy.Domain.Models.Category category)
        {
            return await _categoryRepository.UpdateCatgoryAsync(category);
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            return await _categoryRepository.DeleteCategoryAsync(id);
        }
    }
}
