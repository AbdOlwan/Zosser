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
    [Route("api/v1/specifications")]
    [ApiController]
    public class SpecificationController : ControllerBase
    {
        private readonly ISpecificationServices _service;

        public SpecificationController(ISpecificationServices service)
        {
            _service = service;
        }

        // =================================================
        // Get All Specifications : 
        // =================================================
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<SpecificationDTO>>> GetAllSpecificationsAsync()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var Specifications = await _service.GetAllSpecifications();
                if (Specifications == null || Specifications.Count == 0)
                {
                    return NotFound(new ApiResponse<IEnumerable<SpecificationDTO>>
                    {
                        Success = false,
                        Message = $"No Specifications found.",
                        Errors = ["Empty result"]
                    });
                }
                return Ok(new ApiResponse<IEnumerable<SpecificationDTO>>
                {
                    Success = true,
                    Message = " retrieved successfully",
                    Data = Specifications
                });
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving Specifications.");
            }
        }
        // =================================================
        // Get  Specification By Id: 
        // =================================================

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SpecificationDTO>> GetSpecificationByIdAsync(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.GetSpecificationById(id);
            if (result == null)
            {
                return BadRequest(new ApiResponse<IEnumerable<SpecificationDTO>>
                {
                    Success = false,
                    Message = $"There is no Specification With Id {id}",
                    Errors = ["Empty result"]
                });
            }

            return Ok(new ApiResponse<SpecificationDTO>
            {
                Success = true,
                Message = " retrieved successfully",
                Data = result
            });
        }



        // =================================================
        // Add New Specification : 
        // =================================================
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<SpecificationDTO>> AddNewSpecificationAsync([FromBody] SpecificationDTO Specification)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            // أولًا: التحقق من أن الكائن نفسه وبياناته الأساسية موجودة
            if (Specification == null || string.IsNullOrEmpty(Specification.Specification_Name))
            {
                return BadRequest(new ApiResponse<IEnumerable<SpecificationDTO>>
                {
                    Success = false,
                    Message = $"Not Accepted: Specification data is missing or Name is empty.",
                    Errors = ["Empty result"]
                });
            }
            //var PhoneNum = newDoctor.Phone.Trim().ToString();

            var NewSpecification = await _service.AddNewSpecification(Specification);
            if (NewSpecification == null)
            {
                return BadRequest(new ApiResponse<IEnumerable<SpecificationDTO>>
                {
                    Success = false,
                    Message = $"Specification Not Added",
                    Errors = ["Empty result"]
                });
            }
            return Ok(new ApiResponse<SpecificationDTO>
            {
                Success = true,
                Message = " retrieved successfully",
                Data = NewSpecification
            });
        }



        // =================================================
        // Update Specification By Id: 
        // =================================================

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> UpdateSpecificationByIdAsync(SpecificationDTO Specification)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.UpdateSpecificationById(Specification);
            if (!result)
            {
                return BadRequest(new ApiResponse<IEnumerable<SpecificationDTO>>
                {
                    Success = false,
                    Message = $"Sorry Specification Didnot Updated!!",
                    Errors = ["Empty result"]
                });
            }
            return Ok(new ApiResponse<SpecificationDTO>
            {
                Success = true,
                Message = " Specification Updated Successfully",
                Data = Specification
            });
        }


        // =================================================
        // Delete Specification By Id: 
        // =================================================
        //here we use HttpDelete method

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> DeleteSpecificationByIdAsync(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.DeleteSpecificationById(id);
            if (!result)
                return BadRequest(new ApiResponse<IEnumerable<SpecificationDTO>>
                {
                    Success = false,
                    Message = $"Sorry Specification Didnot Deleted! Please Try Again!!",
                    Errors = ["Empty result"]
                });

            return Ok(new ApiResponse<SpecificationDTO>
            {
                Success = true,
                Message = $"Specification With Id {id} Deleted Successfully",
                Data = null
            });
        }

    }
}
