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
    public class CustomerController(ICustomerServices service) : ControllerBase
    {
        private readonly ICustomerServices _service = service;

        // =================================================
        // Get All Customers : 
        // =================================================
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<CustomerDTO>>> GetAllCustomersAsync()
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, ModelState));
            try
            {
                var Customers = await _service.GetAllCustomers();
                if (Customers == null || Customers.Count == 0)
                {
                    return NotFound(new ApiResponse(400, "No Customers found."));
                }
                return Ok(new ApiResponse(200, Customers));


            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"An error occurred while retrieving Customers: {ex.Message}"));
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
                return BadRequest(new ApiResponse(400, ModelState));

            var result = await _service.GetCustomerById(id);
            if (result == null)
            {
                return BadRequest(new ApiResponse(400, $"There is no Customer With Id {id}"));

            }

            return Ok(new ApiResponse(200, result));

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
                return BadRequest(new ApiResponse(400, ModelState));
            // أولًا: التحقق من أن الكائن نفسه وبياناته الأساسية موجودة
            if (Customer == null )
            {
                return BadRequest(new ApiResponse(400, $"Not Accepted: Customer data is missing or Name is empty."));

            }
            //var PhoneNum = newDoctor.Phone.Trim().ToString();

            var NewCustomer = await _service.AddNewCustomer(Customer);
            if (NewCustomer == null)
            {
                return BadRequest(new ApiResponse(400, $"Customer Not Added"));

            }
            return Ok(new ApiResponse(200, NewCustomer));
 
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
                return BadRequest(new ApiResponse(400, ModelState));

            var result = await _service.UpdateCustomerById(Customer);
            if (!result)
            {
                return BadRequest(new ApiResponse(400, $"Sorry Customer Didnot Updated!!"));

            }
            return Ok(new ApiResponse(200, Customer));
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


            var result = await _service.DeleteCustomerById(id);
            if (!result)
                return BadRequest(new ApiResponse(400, $"Sorry Customer Didnot Deleted! Please Try Again!!"));


            return Ok(new ApiResponse(200, $"Customer With Id {id} Deleted Successfully"));

        }
    }
}
