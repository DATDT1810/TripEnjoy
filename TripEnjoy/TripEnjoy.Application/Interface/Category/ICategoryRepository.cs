using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripEnjoy.Application.Interface.Category
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<TripEnjoy.Domain.Models.Category>> GetCategoriesAsync();
        Task<TripEnjoy.Domain.Models.Category> GetCategoryByIdAsync(int id);
        Task<TripEnjoy.Domain.Models.Category> AddCategoryAsync(TripEnjoy.Domain.Models.Category category);
        Task<TripEnjoy.Domain.Models.Category> UpdateCatgoryAsync(TripEnjoy.Domain.Models.Category category);
        Task<bool> DeleteCategoryAsync(int id);
    }
}
