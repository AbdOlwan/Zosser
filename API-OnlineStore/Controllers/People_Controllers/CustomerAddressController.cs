using API_OnlineStore.Common;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Cart_F;
using BLL_OnlineStore.DTOs.EntitiesDTOs.People_F;
using BLL_OnlineStore.Interfaces.PeopleBusServices;
using DAL_OnlineStore.Entities.Models.CartModels;
using DAL_OnlineStore.Entities.Models.People;
using DAL_OnlineStore.Entities.Models.ReviewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API_OnlineStore.Controllers.People_Controllers
{
    [Route("api/v1/customerAddresses")]
    [ApiController]
    public class CustomerAddressController : ControllerBase
    {
        private readonly ICustomerAddressServices _service;

        public CustomerAddressController(ICustomerAddressServices service)
        {
            _service = service;
        }

        // =================================================
        // Get All CustomerAddresses : 
        // =================================================
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<CustomerAddressDTO>>> GetAllCustomerAddressesAsync()
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, ModelState));
            try
            {
                var CustomerAddresses = await _service.GetAllCustomerAddresses();
                if (CustomerAddresses == null || CustomerAddresses.Count == 0)
                {
                    return NotFound(new ApiResponse(400, "No CustomerAddresses Found"));
                }

                return Ok(new ApiResponse(200, CustomerAddresses));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"An error occurred while retrieving CustomerAddresses: {ex.Message}"));

            }
        }

        // =================================================
        // Get  CustomerAddress By Id: 
        // =================================================

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CustomerAddressDTO>> GetCustomerAddressByIdAsync(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, ModelState));

            var result = await _service.GetCustomerAddressById(id);
            if (result == null)
            {
                return NotFound(new ApiResponse(400, "No CustomerAddresses Found"));

            }
            return Ok(new ApiResponse(200, result));

        }

        // =================================================
        // Add New CustomerAddress : 
        // =================================================
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<CustomerAddressDTO>> AddNewCustomerAddressAsync([FromBody]
                                                                 CustomerAddressDTO CustomerAddress)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, ModelState));
            // أولًا: التحقق من أن الكائن نفسه وبياناته الأساسية موجودة
            if (CustomerAddress == null)
            {
                return BadRequest(new ApiResponse(400, " Not Accepted: CustomerAddress data is missing or Name is empty."));
            }
            //var PhoneNum = newDoctor.Phone.Trim().ToString();

            var NewCustomerAddress = await _service.AddNewCustomerAddress(CustomerAddress);
            if (NewCustomerAddress == null)
            {
                return BadRequest(new ApiResponse(400, " CustomerAddress Not Added."));

            }
            return Ok(new ApiResponse(200, " Added successfully"));
        }



        // =================================================
        // Update CustomerAddress By Id: 
        // =================================================

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> UpdateCustomerAddressByIdAsync(CustomerAddressDTO CustomerAddress)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, ModelState));

            var result = await _service.UpdateCustomerAddressById(CustomerAddress);
            if (!result)
            {
                return BadRequest(new ApiResponse(400, " Sorry CustomerAddress Didnot Updated!!."));

            }
            return Ok(new ApiResponse(200, " CustomerAddress Updated Successfully"));

        }

        // =================================================
        // Delete CustomerAddress By Id: 
        // =================================================
        //here we use HttpDelete method

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> DeleteCustomerAddressByIdAsync(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, ModelState));

            var result = await _service.DeleteCustomerAddressById(id);
            if (!result)
                return BadRequest(new ApiResponse(400, "Sorry CustomerAddress Didnot Deleted! Please Try Again!!"));


            return Ok(new ApiResponse(200, " CustomerAddress With Id {id} Deleted Successfully"));

        }
    }
}
