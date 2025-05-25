using API_OnlineStore.Common;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Cart_F;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Product_F;
using BLL_OnlineStore.Interfaces.ProductBusServices;
using DAL_OnlineStore.Entities.Models.CartModels;
using DAL_OnlineStore.Entities.Models.ProductModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API_OnlineStore.Controllers.Product_Controllers
{
    [Route("api/v1/categorys")]
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
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetAllCategorysAsync()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var Categorys = await _service.GetAllCategorys();
                if (Categorys == null || Categorys.Count == 0)
                {
                    return NotFound(new ApiResponse<IEnumerable<CategoryDTO>>
                    {
                        Success = false,
                        Message = $" No Categorys found.",
                        Errors = ["Empty result"]
                    });

                }
                return Ok(new ApiResponse<IEnumerable<CategoryDTO>>
                {
                    Success = true,
                    Message = " retrieved successfully",
                    Data = Categorys
                });

            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving Categorys.");

            }
        }
        // =================================================
        // Get  Category By Id: 
        // =================================================

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoryDTO>> GetCategoryByIdAsync(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.GetCategoryById(id);
            if (result == null)
            {
                return BadRequest(new ApiResponse<IEnumerable<CategoryDTO>>
                {
                    Success = false,
                    Message = $" There is no Category With Id {id}",
                    Errors = ["Empty result"]
                });
            }

            return Ok(new ApiResponse<CategoryDTO>
            {
                Success = true,
                Message = " retrieved successfully",
                Data = result
            });
        }



        // =================================================
        // Add New Category : 
        // =================================================
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<CategoryDTO>> AddNewCategoryAsync([FromBody] CategoryDTO Category)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            // أولًا: التحقق من أن الكائن نفسه وبياناته الأساسية موجودة
            if (Category == null || string.IsNullOrEmpty(Category.Category_Name))
            {
                return BadRequest(new ApiResponse<IEnumerable<CategoryDTO>>
                {
                    Success = false,
                    Message = $" Not Accepted: Category data is missing or Name is empty.",
                    Errors = ["Empty result"]
                });
            }
            //var PhoneNum = newDoctor.Phone.Trim().ToString();

            var NewCategory = await _service.AddNewCategory(Category);
            if (NewCategory == null)
            {
                return BadRequest(new ApiResponse<IEnumerable<CategoryDTO>>
                {
                    Success = false,
                    Message = $" Category Not Added",
                    Errors = ["Empty result"]
                });
            }
            return Ok(new ApiResponse<CategoryDTO>
            {
                Success = true,
                Message = " retrieved successfully",
                Data = NewCategory
            });
        }



        // =================================================
        // Update Category By Id: 
        // =================================================

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> UpdateCategoryByIdAsync(CategoryDTO Category)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.UpdateCategoryById(Category);
            if (!result)
            {
                return BadRequest(new ApiResponse<IEnumerable<CategoryDTO>>
                {
                    Success = false,
                    Message = $"Sorry Category Didnot Updated!!",
                    Errors = ["Empty result"]
                });
            }
            return Ok(new ApiResponse<CategoryDTO>
            {
                Success = true,
                Message = " Category Updated Successfully",
                Data = Category
            });
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
