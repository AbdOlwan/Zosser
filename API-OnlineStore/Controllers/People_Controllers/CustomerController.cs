using API_OnlineStore.Common;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Cart_F;
using BLL_OnlineStore.DTOs.EntitiesDTOs.People_F;
using BLL_OnlineStore.Interfaces.PeopleBusServices;
using DAL_OnlineStore.Entities.Models.CartModels;
using DAL_OnlineStore.Entities.Models.People;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API_OnlineStore.Controllers.People_Controllers
{
    [Route("api/v1/customers")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerServices _service;

        public CustomerController(ICustomerServices service)
        {
            _service = service;
        }

        // =================================================
        // Get All Customers : 
        // =================================================
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<CustomerDTO>>> GetAllCustomersAsync()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var Customers = await _service.GetAllCustomers();
                if (Customers == null || Customers.Count == 0)
                {
                    return NotFound(new ApiResponse<IEnumerable<CustomerDTO>>
                    {
                        Success = false,
                        Message = $"No Customers found.",
                        Errors = ["Empty result"]
                    });
                }
                return Ok(new ApiResponse<IEnumerable<CustomerDTO>>
                {
                    Success = true,
                    Message = " retrieved successfully",
                    Data = Customers
                });

            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving Customers.");
            }
        }

        // =================================================
        // Get  Customer By Id: 
        // =================================================

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CustomerDTO>> GetCustomerByIdAsync(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.GetCustomerById(id);
            if (result == null)
            {
                return BadRequest(new ApiResponse<IEnumerable<CustomerDTO>>
                {
                    Success = false,
                    Message = $"There is no Customer With Id {id}",
                    Errors = ["Empty result"]
                });
            }

            return Ok(new ApiResponse<CustomerDTO>
            {
                Success = true,
                Message = " retrieved successfully",
                Data = result
            });
        }
        // =================================================
        // Add New Customer : 
        // =================================================
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<CustomerDTO>> AddNewCustomerAsync([FromBody] CustomerDTO Customer)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            // أولًا: التحقق من أن الكائن نفسه وبياناته الأساسية موجودة
            if (Customer == null )
            {
                return BadRequest(new ApiResponse<IEnumerable<CustomerDTO>>
                {
                    Success = false,
                    Message = $"Not Accepted: Customer data is missing or Name is empty.",
                    Errors = ["Empty result"]
                });
            }
            //var PhoneNum = newDoctor.Phone.Trim().ToString();

            var NewCustomer = await _service.AddNewCustomer(Customer);
            if (NewCustomer == null)
            {
                return BadRequest(new ApiResponse<IEnumerable<CustomerDTO>>
                {
                    Success = false,
                    Message = $"Customer Not Added",
                    Errors = ["Empty result"]
                });
            }
            return Ok(new ApiResponse<CustomerDTO>
            {
                Success = true,
                Message = " retrieved successfully",
                Data = NewCustomer
            });
        }



        // =================================================
        // Update Customer By Id: 
        // =================================================

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> UpdateCustomerByIdAsync(CustomerDTO Customer)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.UpdateCustomerById(Customer);
            if (!result)
            {
                return BadRequest(new ApiResponse<IEnumerable<CustomerDTO>>
                {
                    Success = false,
                    Message = $"Sorry Customer Didnot Updated!!",
                    Errors = ["Empty result"]
                });
            }
            return Ok(new ApiResponse<CustomerDTO>
            {
                Success = true,
                Message = " Customer Updated Successfully",
                Data = Customer
            });
        }


        // =================================================
        // Delete Customer By Id: 
        // =================================================
        //here we use HttpDelete method

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> DeleteCustomerByIdAsync(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.DeleteCustomerById(id);
            if (!result)
                return BadRequest(new ApiResponse<IEnumerable<CustomerDTO>>
                {
                    Success = false,
                    Message = $"Sorry Customer Didnot Deleted! Please Try Again!!",
                    Errors = ["Empty result"]
                });

            return Ok(new ApiResponse<CustomerDTO>
            {
                Success = true,
                Message = $"Customer With Id {id} Deleted Successfully",
                Data = null
            });
        }
    }
}
