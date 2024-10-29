using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripEnjoy.Application.Interface.Category;
using TripEnjoy.Domain.Models;
using TripEnjoy.Infrastructure.Entities;

namespace TripEnjoy.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Retrieve all categories
        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        // Retrieve a single category by ID
        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        // Add new a category
        public async Task<Category> AddCategoryAsync(string categoryName)
        {
           var category = new Category
           {
               CategoryName = categoryName,
               CategoryStatus = true
           };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category; 
        }

        // Delete a category by ID
        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return false;
            }
            category.CategoryStatus = false;
            await _context.SaveChangesAsync();
            return true;
        }

        // Update an existing category
        public async Task<Category> UpdateCatgoryAsync(Category category)
        {
            var categoryExist = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == category.CategoryId);

            if (categoryExist == null)
            {
                throw new Exception("The category does not exist.");
            }

        
            categoryExist.CategoryName = category.CategoryName;
            categoryExist.CategoryStatus = category.CategoryStatus;

            _context.Categories.Update(categoryExist);
            await _context.SaveChangesAsync();

            return categoryExist; // Trả về danh mục sau khi cập nhật
        }
    }
}
