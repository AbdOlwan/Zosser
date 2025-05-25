using API_OnlineStore.Common;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Cart_F;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Product_F;
using BLL_OnlineStore.Interfaces.ProductBusServices;
using DAL_OnlineStore.Entities.Models.CartModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Drawing2D;

namespace API_OnlineStore.Controllers.Product_Controllers
{
    [Route("api/v1/types")]
    [ApiController]
    public class TypeController : ControllerBase
    {
        private readonly ITypeServices _service;

        public TypeController(ITypeServices service)
        {
            _service = service;
        }

        // =================================================
        // Get All Types : 
        // =================================================
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<TypeDTO>>> GetAllTypesAsync()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var Types = await _service.GetAllTypes();
                if (Types == null || Types.Count == 0)
                {
                    return NotFound(new ApiResponse<IEnumerable<TypeDTO>>
                    {
                        Success = false,
                        Message = $"No Types found.",
                        Errors = ["Empty result"]
                    });
                }
                return Ok(new ApiResponse<IEnumerable<TypeDTO>>
                {
                    Success = true,
                    Message = " retrieved successfully",
                    Data = Types
                });
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving Types.");
            }
        }

        // =================================================
        // Get  Type By Id: 
        // =================================================

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TypeDTO>> GetTypeByIdAsync(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.GetTypeById(id);
            if (result == null)
            {
                return BadRequest(new ApiResponse<IEnumerable<TypeDTO>>
                {
                    Success = false,
                    Message = $"There is no Type With Id {id}",
                    Errors = ["Empty result"]
                });
            }

            return Ok(new ApiResponse<TypeDTO>
            {
                Success = true,
                Message = "retrieved successfully",
                Data = result
            });
        }
        // =================================================
        // Add New Type : 
        // =================================================
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<TypeDTO>> AddNewTypeAsync([FromBody] TypeDTO Type)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            // أولًا: التحقق من أن الكائن نفسه وبياناته الأساسية موجودة
            if (Type == null || string.IsNullOrEmpty(Type.TypeName))
            {
                return BadRequest(new ApiResponse<IEnumerable<TypeDTO>>
                {
                    Success = false,
                    Message = $"Not Accepted: Type data is missing or Name is empty.",
                    Errors = ["Empty result"]
                });
            }
            //var PhoneNum = newDoctor.Phone.Trim().ToString();

            var NewType = await _service.AddNewType(Type);
            if (NewType == null)
            {
                return BadRequest(new ApiResponse<IEnumerable<TypeDTO>>
                {
                    Success = false,
                    Message = $"Type Not Added.",
                    Errors = ["Empty result"]
                });
            }
            return Ok(new ApiResponse<TypeDTO>
            {
                Success = true,
                Message = "retrieved successfully",
                Data = NewType
            });
        }



        // =================================================
        // Update Type By Id: 
        // =================================================

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> UpdateTypeByIdAsync(TypeDTO Type)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.UpdateTypeById(Type);
            if (!result)
            {
                return BadRequest(new ApiResponse<IEnumerable<TypeDTO>>
                {
                    Success = false,
                    Message = $"Sorry Type Didnot Updated!!.",
                    Errors = ["Empty result"]
                });
            }
            return Ok(new ApiResponse<TypeDTO>
            {
                Success = true,
                Message = "Type Updated Successfully",
                Data = Type
            });
        }


        // =================================================
        // Delete Type By Id: 
        // =================================================
        //here we use HttpDelete method

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> DeleteTypeByIdAsync(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.DeleteTypeById(id);
            if (!result)
                return BadRequest(new ApiResponse<IEnumerable<TypeDTO>>
                {
                    Success = false,
                    Message = $"Sorry Type Didnot Deleted! Please Try Again!!.",
                    Errors = ["Empty result"]
                });

            return Ok(new ApiResponse<TypeDTO>
            {
                Success = true,
                Message = $"Type With Id {id} Deleted Successfully",
                Data = null
            });
        }

    }
}
