using API_OnlineStore.Common;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Cart_F;
using BLL_OnlineStore.DTOs.EntitiesDTOs.People_F;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Product_F;
using BLL_OnlineStore.Interfaces.ProductBusServices;
using DAL_OnlineStore.Entities.Models.CartModels;
using DAL_OnlineStore.Entities.Models.ProductModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API_OnlineStore.Controllers.Product_Controllers
{
    [Route("api/v1/brands")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBrandServices _service;

        public BrandController(IBrandServices service)
        {
            _service = service;
        }

        // =================================================
        // Get All Brands : 
        // =================================================

        /// <summary>
        /// Get all Brands with culture-specific translations
        /// </summary>
        /// <param name="culture">Culture code (default: ar)</param>
        /// <returns>List of Brands</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<BrandDTO>>> GetAllBrands([FromQuery] string culture = "ar")
        {
            try
            {
                var Brands = await _service.GetAllBrands(culture);
                if (Brands == null)
                {
                    return NotFound(new ApiResponse(400, "No Brands Found"));
                }
                return Ok(new ApiResponse(200, Brands));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"An error occurred while retrieving Brands: {ex.Message}"));
            }
        }

        // =================================================
        // Get  Brand By Id: 
        // =================================================

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BrandDTO>> GetBrandByIdAsync(int id, [FromQuery] string culture = "ar")
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, ModelState));

            try
            {
                var result = await _service.GetBrandByIdAsync(id, culture);
                if (result == null)
                    return NotFound(new ApiResponse(404, $"Brand with ID {id} not found"));

                return Ok(new ApiResponse(200, result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"An error occurred while retrieving the Brand: {ex.Message}"));
            }
        }

        // =================================================
        // Add New Brand : 
        // =================================================

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<BrandDTO>> AddNewBrandAsync([FromBody] CreateBrandDTO createBrandDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, ModelState));

            if (createBrandDto == null || string.IsNullOrWhiteSpace(createBrandDto.ArBrandName) || string.IsNullOrWhiteSpace(createBrandDto.EnBrandName))
            {
                return BadRequest(new ApiResponse(400, "Invalid input data"));

            }

            var newBrand = await _service.CreateBrandAsync(createBrandDto);
            if (newBrand == null)
            {
                return BadRequest(new ApiResponse(400, "Operation failed"));
;
            }

            return CreatedAtAction(nameof(GetBrandByIdAsync), new { id = newBrand.Brand_ID }, new ApiResponse<BrandDTO>
            {
                Success = true,
                Message = "Brand created successfully",
                Data = newBrand
            });
        }


        // =================================================
        // Update Brand By Id: 
        // =================================================


        /// <summary>
        /// Update an existing brand with translations
        /// </summary>
        /// <param name="id">Brand ID</param>
        /// <param name="updateBrandDTO">Brand update data</param>
        /// <returns>No content if successful</returns>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateBrand(int id, [FromBody] UpdateBrandDTO updateBrandDTO)
        {
            if (id != updateBrandDTO.Brand_ID)
                return BadRequest(new ApiResponse(400, "ID in the URL does not match the ID in the request body"));

            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, ModelState));

            try
            {
                // تحقق من وجود البراند
                if (!await _service.BrandExistsAsync(id))
                    return NotFound(new ApiResponse(404, $"Brand with ID {id} not found"));

                // تحديث البراند
                await _service.UpdateBrandAsync(updateBrandDTO);

                return NoContent(); // 204 No Content
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponse(404, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"An error occurred while updating the brand: {ex.Message}"));
            }
        }












        // =================================================
        // Delete Brand By Id: 
        // =================================================
        //here we use HttpDelete method

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> DeleteBrandByIdAsync(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.DeleteBrandById(id);
            if (!result)
                     return BadRequest(new ApiResponse<IEnumerable<BrandDTO>>
                     {
                         Success = false,
                         Message = $"Sorry Brand Didnot Deleted! Please Try Again!!",
                         Errors = ["Empty result"]
                     });

                  return Ok(new ApiResponse<BrandDTO>
                  {
                      Success = true,
                      Message = $"Brand With Id {id} Deleted Successfully",
                      Data = null
                  });
        }

    }
}







//[HttpPost]
//[ProducesResponseType(StatusCodes.Status400BadRequest)]
//[ProducesResponseType(StatusCodes.Status201Created)]
//public async Task<ActionResult<BrandDTO>> AddNewBrandAsync([FromBody] BrandDTO Brand)
//{
//    if (!ModelState.IsValid)
//        return BadRequest(ModelState);
//    // أولًا: التحقق من أن الكائن نفسه وبياناته الأساسية موجودة
//    if (Brand == null || string.IsNullOrEmpty(Brand.Brand_Name))
//    {
//        return BadRequest(new ApiResponse<IEnumerable<BrandDTO>>
//        {
//            Success = false,
//            Message = $"Not Accepted: Brand data is missing or Name is empty.",
//            Errors = ["Empty result"]
//        });
//    }
//    //var PhoneNum = newDoctor.Phone.Trim().ToString();

//    var NewBrand = await _service.AddNewBrand(Brand);
//    if (NewBrand == null)
//    {
//        return BadRequest(new ApiResponse<IEnumerable<BrandDTO>>
//        {
//            Success = false,
//            Message = $"Brand Not Added",
//            Errors = ["Empty result"]
//        });
//    }
//    return Ok(new ApiResponse<BrandDTO>
//    {
//        Success = true,
//        Message = " retrieved successfully",
//        Data = NewBrand
//    });
//}






//[HttpPut]
//[ProducesResponseType(StatusCodes.Status200OK)]
//[ProducesResponseType(StatusCodes.Status400BadRequest)]
//[ProducesResponseType(StatusCodes.Status404NotFound)]
//public async Task<ActionResult<string>> UpdateBrandByIdAsync1(UpdateBrandDTO dto)
//{
//    if (!ModelState.IsValid)
//        return BadRequest(ModelState);

//    var result = await _service.UpdateBrandAsync(dto);
//    if (!result)
//    {
//        return BadRequest(new ApiResponse<IEnumerable<BrandDTO>>
//        {
//            Success = false,
//            Message = $"Sorry Brand Didnot Updated!!",
//            Errors = ["Empty result"]
//        });
//    }
//    return Ok(new ApiResponse<BrandDTO>
//    {
//        Success = true,
//        Message = "Brand Updated Successfully",
//        Data = Brand
//    });
//}
