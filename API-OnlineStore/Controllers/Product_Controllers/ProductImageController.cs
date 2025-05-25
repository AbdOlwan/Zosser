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
    [Route("api/v1/productImages")]
    [ApiController]
    public class ProductImageController : ControllerBase
    {
        private readonly IProductImageServices _service;

        public ProductImageController(IProductImageServices service)
        {
            _service = service;
        }

        // =================================================
        // Get All ProductImages : 
        // =================================================
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ProductImageDTO>>> GetAllProductImagesAsync()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var ProductImages = await _service.GetAllProductImages();
                if (ProductImages == null || ProductImages.Count == 0)
                {
                    return NotFound(new ApiResponse<IEnumerable<ProductImageDTO>>
                    {
                        Success = false,
                        Message = $" No ProductImages found.",
                        Errors = ["Empty result"]
                    });
                }
                return Ok(new ApiResponse<IEnumerable<ProductImageDTO>>
                {
                    Success = true,
                    Message = " retrieved successfully",
                    Data = ProductImages
                });
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving ProductImages.");
            }
        }
        // =================================================
        // Get  ProductImage By Id: 
        // =================================================

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductImageDTO>> GetProductImageByIdAsync(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.GetProductImageById(id);
            if (result == null)
            {
                return BadRequest(new ApiResponse<IEnumerable<ProductImageDTO>>
                {
                    Success = false,
                    Message = $"There is no ProductImage With Id {id}",
                    Errors = ["Empty result"]
                });
            }

            return Ok(new ApiResponse<ProductImageDTO>
            {
                Success = true,
                Message = " retrieved successfully",
                Data = result
            });
        }

        // =================================================
        // Add New ProductImage : 
        // =================================================
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<ProductImageDTO>> AddNewProductImageAsync([FromBody] ProductImageDTO ProductImage)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            // أولًا: التحقق من أن الكائن نفسه وبياناته الأساسية موجودة
            if (ProductImage == null)
            {
                return BadRequest(new ApiResponse<IEnumerable<ProductImageDTO>>
                {
                    Success = false,
                    Message = $"Not Accepted: ProductImage data is missing or Name is empty.",
                    Errors = ["Empty result"]
                });
            }
            //var PhoneNum = newDoctor.Phone.Trim().ToString();

            var NewProductImage = await _service.AddNewProductImage(ProductImage);
            if (NewProductImage == null)
            {
                return BadRequest(new ApiResponse<IEnumerable<ProductImageDTO>>
                {
                    Success = false,
                    Message = $"ProductImage Not Added",
                    Errors = ["Empty result"]
                });
            }
            return Ok(new ApiResponse<ProductImageDTO>
            {
                Success = true,
                Message = " retrieved successfully",
                Data = NewProductImage
            });
        }


        // =================================================
        // Update ProductImage By Id: 
        // =================================================

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> UpdateProductImageByIdAsync(ProductImageDTO ProductImage)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.UpdateProductImageById(ProductImage);
            if (!result)
            {
                return BadRequest(new ApiResponse<IEnumerable<ProductImageDTO>>
                {
                    Success = false,
                    Message = $"Sorry ProductImage Didnot Updated!!",
                    Errors = ["Empty result"]
                });
            }
            return Ok(new ApiResponse<ProductImageDTO>
            {
                Success = true,
                Message = "ProductImage Updated Successfully",
                Data = ProductImage
            });
        }

        // =================================================
        // Delete ProductImage By Id: 
        // =================================================
        //here we use HttpDelete method

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> DeleteProductImageByIdAsync(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.DeleteProductImageById(id);
            if (!result)
                return BadRequest(new ApiResponse<IEnumerable<ProductImageDTO>>
                {
                    Success = false,
                    Message = $"Sorry ProductImage Didnot Deleted! Please Try Again!!",
                    Errors = ["Empty result"]
                });

            return Ok(new ApiResponse<ProductImageDTO>
            {
                Success = true,
                Message = $"ProductImage With Id {id} Deleted Successfully",
                Data = null
            });
        }
    }
}
