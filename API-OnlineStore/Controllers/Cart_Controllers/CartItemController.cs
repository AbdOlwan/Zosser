using API_OnlineStore.Common;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Cart_F;
using BLL_OnlineStore.Interfaces.CartBusServices;
using DAL_OnlineStore.Entities.Models.CartModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API_OnlineStore.Controllers.Cart_Controllers
{
    [Route("api/v1/cartItems")]
    [ApiController]
    public class CartItemController(ICartItemServices service) : ControllerBase
    {
        private readonly ICartItemServices _service = service;

        // =================================================
        // Get All CartItems : 
        // =================================================
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<CartItemDTO>>> GetAllCartItemsAsync()
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, ModelState));
            try
            {
                var CartItems = await _service.GetAllCartItems();
                if (CartItems == null || CartItems.Count == 0)
                {

                    return NotFound(new ApiResponse(400, "No cartItems found"));

                }

                return Ok(new ApiResponse(200, "cartItem retrieved successfully"));

            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"An error occurred while retrieving cartItem: {ex.Message}"));
            }

        }





        // =================================================
        // Get  CartItem By Id: 
        // =================================================

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CartItemDTO>> GetCartItemByIdAsync(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, ModelState));

            var result = await _service.GetCartItemById(id);
            if (result == null)
            {
                return NotFound(new ApiResponse(400, $" cartItem With ID{id} Not Found."));
            }

            return Ok(new ApiResponse(200, result));

        }


        // =================================================
        // Add New CartItem : 
        // =================================================
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<CartItemDTO>> AddNewCartItemAsync([FromBody] CartItemDTO CartItem)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, ModelState));
            // أولًا: التحقق من أن الكائن نفسه وبياناته الأساسية موجودة
            if (CartItem == null)
            {

                return NotFound(new ApiResponse(400, $" cart With ID{CartItem?.CartItemId} Not Added."));

            }
            //var PhoneNum = newDoctor.Phone.Trim().ToString();

            var NewCartItem = await _service.AddNewCartItem(CartItem);
            if (NewCartItem == null)
            {
                return NotFound(new ApiResponse(400, $" cartItem With ID{CartItem?.CartItemId} Not Added."));
            }
            return Ok(new ApiResponse(200, NewCartItem));
        }




        // =================================================
        // Update CartItem By Id: 
        // =================================================

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> UpdateCartItemByIdAsync(CartItemDTO CartItem)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, ModelState));

            var result = await _service.UpdateCartItemById(CartItem);
            if (!result)
            {
                return NotFound(new ApiResponse(400, $" cart With ID{CartItem.CartItemId} Not Updated."));

            }
            return Ok(new ApiResponse(200, CartItem));
        }


        // =================================================
        // Delete Carrier By Id: 
        // =================================================
        //here we use HttpDelete method

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> DeleteCartItemByIdAsync(int id)
        {


            var result = await _service.DeleteCartItemById(id);
            if (!result)

                return NotFound(new ApiResponse(400, $" Sorry CartItem Didnot Deleted! Please Try Again"));


            return Ok(new ApiResponse(200, "CartItem  Deleted Successfully"));

        }


    }
}
