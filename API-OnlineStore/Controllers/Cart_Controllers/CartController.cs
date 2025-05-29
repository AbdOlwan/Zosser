using API_OnlineStore.Common;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Cart_F;
using BLL_OnlineStore.Interfaces.CartBusServices;
using DAL_OnlineStore.Entities.Models.CartModels;
using DAL_OnlineStore.Entities.Models.People;
using Microsoft.AspNetCore.Mvc;

namespace API_OnlineStore.Controllers.Cart_Controllers
{
    [Route("api/v1/carts")]
    [ApiController]
    public class CartController(ICartServices service) : ControllerBase
    {
        private readonly ICartServices _service = service;

        // =================================================
        // Get All Carts : 
        // =================================================


        // GET api/v1/carts
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<CartDTO>>> GetAllCartsAsync()
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, ModelState));
            try
            {
                var Carts = await _service.GetAllCarts();
                if (Carts == null || Carts.Count == 0)
                {
                    return NotFound(new ApiResponse(400, "No carts found"));
                }
                return Ok(new ApiResponse(200, Carts));
 

            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"An error occurred while retrieving carts: {ex.Message}"));
            }

        }


        // =================================================
        // Get  Cart By Id: 
        // =================================================

        // GET api/v1/carts/{id}
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CartDTO>> GetCartByIdAsync(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, ModelState));

            var result = await _service.GetCartById(id);
            if (result == null)
            {
                return NotFound(new ApiResponse(400, "No carts found."));

            }

            return Ok(new ApiResponse(200, result));

        }
        // =================================================
        // Add New Cart : 
        // =================================================
        // POST api/v1/carts
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<CartDTO>> AddNewCartAsync([FromBody] CartDTO Cart)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, ModelState));
            // أولًا: التحقق من أن الكائن نفسه وبياناته الأساسية موجودة
            if (Cart == null)
            {
                return NotFound(new ApiResponse(400, $" cart With ID{Cart?.CartId} Not Added."));

            }
            //var PhoneNum = newDoctor.Phone.Trim().ToString();

            var NewCart = await _service.AddNewCart(Cart);
            if (NewCart == null)
            {
                return NotFound(new ApiResponse(400, $" cart With ID{Cart?.CartId} Not Added."));

            }
            return Ok(new ApiResponse(200, NewCart));

        }

        // =================================================
        // Update Cart By Id: 
        // =================================================

        // PUT api/v1/carts/{id}
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> UpdateCartByIdAsync(CartDTO Cart)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, ModelState));

            var result = await _service.UpdateCartById(Cart);
            if (!result)
            {
                return NotFound(new ApiResponse(400, $" cart With ID{Cart?.CartId} Not Added."));

            }
            return Ok(new ApiResponse(200, Cart));
        }


        // =================================================
        // Delete Carrier By Id: 
        // =================================================
        //here we use HttpDelete method

        // DELETE api/v1/carts/{id}
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> DeleteCartByIdAsync(int id)
        {


            var result = await _service.DeleteCartById(id);
            if (!result)
                return NotFound(new ApiResponse(400, $" cart With ID{id} Not Added."));

            return Ok(new ApiResponse(200, $"Cart With ID{id} Deleted successfully"));

        }

    }
}
