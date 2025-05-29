using API_OnlineStore.Common;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Cart_F;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Product_F;
using BLL_OnlineStore.Interfaces.ProductBusServices;
using BLL_OnlineStore.Services.ProductBusServices;
using DAL_OnlineStore.Entities.Models.CartModels;
using DAL_OnlineStore.Entities.Models.ProductModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API_OnlineStore.Controllers.Product_Controllers
{
    [Route("api/v1/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryServices _service;

        public CategoryController(ICategoryServices service)
        {
            _service = service;
        }

        // =================================================
        // Get All Categorys : 
        // =================================================

        /// <summary>
        /// Get all Categorys with culture-specific translations
        /// </summary>
        /// <param name="culture">Culture code (default: ar)</param>
        /// <returns>List of Categorys</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetAllCategorys([FromQuery] string culture = "ar")
        {

            try
            {
                var categories = await _service.GetAllCategorys(culture);
                return Ok(new ApiResponse(200, categories));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"An error occurred while retrieving categories: {ex.Message}"));
            }
        }

        // =================================================
        // Get  Category By Id: 
        // =================================================


        /// <summary>
        /// Get a category by ID with culture-specific translations
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CategoryDTO>> GetCategoryById(int id, [FromQuery] string culture = "ar")
        {
            try
            {
                var category = await _service.GetCategoryByIdAsync(id, culture);

                if (category == null)
                    return NotFound(new ApiResponse(404, $"Category with ID {id} not found"));

                return Ok(new ApiResponse(200, category));

            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"An error occurred while retrieving the category: {ex.Message}"));
            }
        }



        // =================================================
        // Add New Category : 
        // =================================================
        /// <summary>
        /// Create a new category with translations
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<CategoryDTO>> CreateCategory([FromBody] CreateCategoryDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, ModelState));

            try
            {
                if (string.IsNullOrWhiteSpace(dto.ArCategoryName))
                    return BadRequest(new ApiResponse(400, "Arabic Category Name is required"));

                if (string.IsNullOrWhiteSpace(dto.EnCategoryName))
                    return BadRequest(new ApiResponse(400, "English Category Name is required"));

                var createdCategory = await _service.CreateCategoryAsync(dto);

                return CreatedAtAction(nameof(GetCategoryById), new { id = createdCategory?.CategoryId }, createdCategory);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"An error occurred while creating the category: {ex.Message}"));
            }
        }


        // =================================================
        // Update Category By Id: 
        // =================================================

        /// <summary>
        /// Update an existing category with translations
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryDTO dto)
        {
            if (id != dto.CategoryId)
                return BadRequest(new ApiResponse(400, "ID in the URL does not match the ID in the request body"));

            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, ModelState));

            try
            {
                if (!await _service.CategoryExistsAsync(id))
                    return NotFound(new ApiResponse(404, $"Category with ID {id} not found"));

                await _service.UpdateCategoryAsync(dto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponse(404, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"An error occurred while updating the category: {ex.Message}"));
            }
        }


        // =================================================
        // Delete Category By Id: 
        // =================================================
        //here we use HttpDelete method

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> DeleteCategoryByIdAsync(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.DeleteCategoryById(id);
            if (!result)
                return BadRequest(new ApiResponse<IEnumerable<CategoryDTO>>
                {
                    Success = false,
                    Message = $"Sorry Category Didnot Deleted! Please Try Again",
                    Errors = ["Empty result"]
                });

            return Ok(new ApiResponse<CategoryDTO>
            {
                Success = true,
                Message = $"Category With Id {id} Deleted Successfully",
                Data = null
            });
        }

    }
}
