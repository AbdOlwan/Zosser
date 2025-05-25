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
    [Route("api/v1/specificationValues")]
    [ApiController]
    public class SpecificationValueController : ControllerBase
    {

        private readonly ISpecificationValueServices _service;

        public SpecificationValueController(ISpecificationValueServices service)
        {
            _service = service;
        }

        // =================================================
        // Get All SpecificationValues : 
        // =================================================
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<SpecificationValueDTO>>> GetAllSpecificationValuesAsync()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var SpecificationValues = await _service.GetAllSpecificationValues();
                if (SpecificationValues == null || SpecificationValues.Count == 0)
                {
                    return NotFound(new ApiResponse<IEnumerable<SpecificationValueDTO>>
                    {
                        Success = false,
                        Message = $" No SpecificationValues found.",
                        Errors = ["Empty result"]
                    });
                }
                return Ok(new ApiResponse<IEnumerable<SpecificationValueDTO>>
                {
                    Success = true,
                    Message = "Added successfully",
                    Data = SpecificationValues
                });
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving SpecificationValues.");
            }
        }

        // =================================================
        // Get  SpecificationValue By Id: 
        // =================================================

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SpecificationValueDTO>> GetSpecificationValueByIdAsync(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.GetSpecificationValueById(id);
            if (result == null)
            {
                return BadRequest(new ApiResponse<IEnumerable<SpecificationValueDTO>>
                {
                    Success = false,
                    Message = $" There is no SpecificationValue With Id {id}.",
                    Errors = ["Empty result"]
                });
            }

            return Ok(new ApiResponse<SpecificationValueDTO>
            {
                Success = true,
                Message = "Added successfully",
                Data = result
            });
        }
        // =================================================
        // Add New SpecificationValue : 
        // =================================================
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<SpecificationValueDTO>> AddNewSpecificationValueAsync([FromBody] 
                                                            SpecificationValueDTO SpecificationValue)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            // أولًا: التحقق من أن الكائن نفسه وبياناته الأساسية موجودة
            if (SpecificationValue == null || string.IsNullOrEmpty(SpecificationValue.Value))
            {
                return BadRequest(new ApiResponse<IEnumerable<SpecificationValueDTO>>
                {
                    Success = false,
                    Message = $" Not Accepted: SpecificationValue data is missing or Name is empty.",
                    Errors = ["Empty result"]
                });
            }
            //var PhoneNum = newDoctor.Phone.Trim().ToString();

            var NewSpecificationValue = await _service.AddNewSpecificationValue(SpecificationValue);
            if (NewSpecificationValue == null)
            {
                return BadRequest(new ApiResponse<IEnumerable<SpecificationValueDTO>>
                {
                    Success = false,
                    Message = $" SpecificationValue Not Added",
                    Errors = ["Empty result"]
                });
            }

            return Ok(new ApiResponse<SpecificationValueDTO>
            {
                Success = true,
                Message = "Added successfully",
                Data = NewSpecificationValue
            });
        }

        // =================================================
        // Update SpecificationValue By Id: 
        // =================================================

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> UpdateSpecificationValueByIdAsync(SpecificationValueDTO SpecificationValue)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.UpdateSpecificationValueById(SpecificationValue);
            if (!result)
            {
                return BadRequest(new ApiResponse<IEnumerable<SpecificationValueDTO>>
                {
                    Success = false,
                    Message = $"Sorry SpecificationValue Didnot Updated!!",
                    Errors = ["Empty result"]
                });
            }
            return Ok(new ApiResponse<SpecificationValueDTO>
            {
                Success = true,
                Message = "SpecificationValue Updated Successfully",
                Data = SpecificationValue
            });
        }

        // =================================================
        // Delete SpecificationValue By Id: 
        // =================================================
        //here we use HttpDelete method

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> DeleteSpecificationValueByIdAsync(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.DeleteSpecificationValueById(id);
            if (!result)
                   return BadRequest(new ApiResponse<IEnumerable<SpecificationValueDTO>>
                   {
                       Success = false,
                       Message = $"Sorry SpecificationValue Didnot Deleted! Please Try Again!!",
                       Errors = ["Empty result"]
                   });

            return Ok(new ApiResponse<SpecificationValueDTO>
            {
                Success = true,
                Message = $"SpecificationValue With Id {id} Deleted Successfully",
                Data = null
            });
        }


    }
}
