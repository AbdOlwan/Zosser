using API_OnlineStore.Common;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Cart_F;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Product_F;
using BLL_OnlineStore.Interfaces.ProductBusServices;
using DAL_OnlineStore.Entities.Models.CartModels;
using DAL_OnlineStore.Entities.Models.ProductModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_OnlineStore.Controllers.Product_Controllers
{
    [Route("api/v1/productSpecifications")]
    [ApiController]
    public class ProductSpecificationController : ControllerBase
    {
        private readonly IProductSpecificationServices _service;

        public ProductSpecificationController(IProductSpecificationServices service)
        {
            _service = service;
        }

        // =================================================
        // Get All ProductSpecifications : 
        // =================================================
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ProductSpecificationDTO>>> GetAllProductSpecificationsAsync()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var ProductSpecifications = await _service.GetAllProductSpecifications();
                if (ProductSpecifications == null || ProductSpecifications.Count == 0)
                {
                    return NotFound(new ApiResponse<IEnumerable<ProductSpecificationDTO>>
                    {
                        Success = false,
                        Message = $"No ProductSpecifications found.",
                        Errors = ["Empty result"]
                    });

                }
                return Ok(new ApiResponse<IEnumerable<ProductSpecificationDTO>>
                {
                    Success = true,
                    Message = " retrieved successfully",
                    Data = ProductSpecifications
                });

            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving ProductSpecifications.");

            }

        }


        // =================================================
        // Add New ProductSpecification : 
        // =================================================
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<ProductSpecificationDTO>> AddNewProductSpecificationAsync([FromBody]
                                                               ProductSpecificationDTO ProductSpecification)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            // أولًا: التحقق من أن الكائن نفسه وبياناته الأساسية موجودة
            if (ProductSpecification == null )
            {
                return BadRequest(new ApiResponse<IEnumerable<ProductSpecificationDTO>>
                {
                    Success = false,
                    Message = $"Not Accepted: ProductSpecification data is missing or Name is empty.",
                    Errors = ["Empty result"]
                });
            }
            //var PhoneNum = newDoctor.Phone.Trim().ToString();

            var NewProductSpecification = await _service.AddNewProductSpecification(ProductSpecification);
            if (NewProductSpecification == null)
            {
                return BadRequest(new ApiResponse<IEnumerable<ProductSpecificationDTO>>
                {
                    Success = false,
                    Message = $"ProductSpecification Not Added.",
                    Errors = ["Empty result"]
                });
            }
            return Ok(new ApiResponse<ProductSpecificationDTO>
            {
                Success = true,
                Message = " retrieved successfully",
                Data = NewProductSpecification
            });
        }


    }
}
