
using Book_Store_Core.Dto_s.CategoryDTO_s;
using Book_Store_Core.Interfaces;
using Book_Store_Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Book_Store_API.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet("GetAllCategories")]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                var categories = await _categoryRepository.GetAllCategoriesAsync();
                return Ok(new { success = true, data = categories });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [HttpGet("Details")]
        public async Task<IActionResult> GetCategoryById([FromQuery] int id)
        {
            try
            {
                var category = await _categoryRepository.GetCategoryByIdAsync(id);
                if (category == null)
                {
                    return NotFound(new { success = false, message = "Category not found." });
                }

                return Ok(new { success = true, data = category });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }
        
        [HttpPost("CreateCategory")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDTO categorydto)
        {
            if (categorydto == null)
            {
                return BadRequest(new { success = false, message = "Invalid category data." });
            }

            try
            {
                await _categoryRepository.CreateCategoryAsync(categorydto);
                return StatusCode(200 , new { success = true, data = categorydto });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [HttpPut("UpdateCategory")]
        public async Task<IActionResult> UpdateCategory([FromQuery] int id, [FromBody] UpdateCategoryDTO categoryDTO)
        {
            if (categoryDTO == null)
            {
                return BadRequest(new { success = false, message = "Invalid category data." });
            }

            try
            {
                await _categoryRepository.UpdateCategoryAsync(id, categoryDTO);
                return Ok("Category Update Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [HttpDelete("DeleteCategory")]

        public async Task<IActionResult> DeleteCategory([FromQuery] int id)
        {
            try
            {
                await _categoryRepository.DeleteCategoryAsync(id);
                return Ok("Category Delete Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }
    }
}
