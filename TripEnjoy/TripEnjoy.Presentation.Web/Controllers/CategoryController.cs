using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TripEnjoy.Application.Interface.Category;
using TripEnjoy.Domain.Models;

namespace TripEnjoy.Presentation.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _categoryService.GetCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet("{id}", Name = "GetCategoryById")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("AddCategory")]
        public async Task<IActionResult> AddCategory([FromBody] string categoryName)
        {
            if (categoryName == null)
            {
                return BadRequest("Invalid category data!");
            }
            var category = await _categoryService.AddCategoryAsync(categoryName);
            return Ok(category.CategoryId);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("UpdateCategory")]
        public async Task<IActionResult> UpdateCategory([FromBody] Category category)
        {
            if (category == null)
            {
                return BadRequest("Invalid data.");
            }
            await _categoryService.UpdateCatgoryAsync(category);
            return Ok(category.CategoryId);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            Category cate = await _categoryService.GetCategoryByIdAsync(id);
            if (cate == null)
            {
                return NotFound();
            }
            await _categoryService.DeleteCategoryAsync(id);
            return Ok();
        }
    }
}
