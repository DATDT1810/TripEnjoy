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

        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] Category category)
        {
            if (category == null)
            {
                return BadRequest("Invalid category data!");
            }
            await _categoryService.AddCategoryAsync(category);
            return CreatedAtRoute("GetCategoryById", new { id = category.CategoryId }, category);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] Category category)
        {
            if (category == null)
            {
                return BadRequest("Invalid data.");
            }

            Category obj = await _categoryService.GetCategoryByIdAsync(id);
            if (obj == null)
            {
                return NotFound();
            }
            await _categoryService.UpdateCatgoryAsync(category);
            return CreatedAtRoute("GetCategoryById", new { id = category.CategoryId }, category);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            Category cate = await _categoryService.GetCategoryByIdAsync(id);
            if (cate == null)
            {
                return NotFound();
            }
            await _categoryService.DeleteCategoryAsync(id);
            return NoContent();
        }
    }
}
