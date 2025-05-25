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
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<BrandDTO>>> GetAllBrandsAsync()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var Brands = await _service.GetAllBrands();
                if (Brands == null || Brands.Count == 0)
                {
                    return NotFound(new ApiResponse<IEnumerable<BrandDTO>>
                    {
                        Success = false,
                        Message = $"No Brands found.",
                        Errors = ["Empty result"]
                    });


                }

                return Ok(new ApiResponse<IEnumerable<BrandDTO>>
                {
                    Success = true,
                    Message = " retrieved successfully",
                    Data = Brands
                });
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving Brands.");
            }
        }
        // =================================================
        // Get  Brand By Id: 
        // =================================================

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BrandDTO>> GetBrandByIdAsync(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.GetBrandById(id);
            if (result == null)
            {
                return NotFound(new ApiResponse<IEnumerable<BrandDTO>>
                {
                    Success = false,
                    Message = $"There is no Brand With Id {id}",
                    Errors = ["Empty result"]
                });
            }

            return Ok(new ApiResponse<BrandDTO>
            {
                Success = true,
                Message = " retrieved successfully",
                Data = result
            });
        }
        // =================================================
        // Add New Brand : 
        // =================================================
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<BrandDTO>> AddNewBrandAsync([FromBody] BrandDTO Brand)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            // أولًا: التحقق من أن الكائن نفسه وبياناته الأساسية موجودة
            if (Brand == null || string.IsNullOrEmpty(Brand.Brand_Name))
            {
                return BadRequest(new ApiResponse<IEnumerable<BrandDTO>>
                {
                    Success = false,
                    Message = $"Not Accepted: Brand data is missing or Name is empty.",
                    Errors = ["Empty result"]
                });
            }
            //var PhoneNum = newDoctor.Phone.Trim().ToString();

            var NewBrand = await _service.AddNewBrand(Brand);
            if (NewBrand == null)
            {
                return BadRequest(new ApiResponse<IEnumerable<BrandDTO>>
                {
                    Success = false,
                    Message = $"Brand Not Added",
                    Errors = ["Empty result"]
                });
            }
            return Ok(new ApiResponse<BrandDTO>
            {
                Success = true,
                Message = " retrieved successfully",
                Data = NewBrand
            });
        }



        // =================================================
        // Update Brand By Id: 
        // =================================================

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> UpdateBrandByIdAsync(BrandDTO Brand)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.UpdateBrandById(Brand);
            if (!result)
            {
                return BadRequest(new ApiResponse<IEnumerable<BrandDTO>>
                {
                    Success = false,
                    Message = $"Sorry Brand Didnot Updated!!",
                    Errors = ["Empty result"]
                });
            }
            return Ok(new ApiResponse<BrandDTO>
            {
                Success = true,
                Message = "Brand Updated Successfully",
                Data = Brand
            });
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
