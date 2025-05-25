using API_OnlineStore.Common;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Cart_F;
using BLL_OnlineStore.Interfaces.CartBusServices;
using DAL_OnlineStore.Entities.Models.CartModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_OnlineStore.Controllers.CartItem_Controllers
{
    [Route("api/v1/cartItems")]
    [ApiController]
    public class CartItemController : ControllerBase
    {
        private readonly ICartItemServices _service;

        public CartItemController(ICartItemServices service)
        {
            _service = service;
        }

        // =================================================
        // Get All CartItems : 
        // =================================================
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<CartItemDTO>>> GetAllCartItemsAsync()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var CartItems = await _service.GetAllCartItems();
                if (CartItems == null || CartItems.Count == 0)
                {

                    return NotFound(new ApiResponse<IEnumerable<CartItemDTO>>
                    {
                        Success = false,
                        Message = $" cart get success",
                        Errors = ["Empty result"]
                    });

                }

                return Ok(new ApiResponse<IEnumerable<CartItemDTO>>
                {
                    Success = true,
                    Message = "Cart retrieved successfully",
                    Data = CartItems
                });


            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving Carriers.");

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
                return BadRequest(ModelState);

            var result = await _service.GetCartItemById(id);
            if (result == null)
            {
                return NotFound(new ApiResponse<IEnumerable<CartItemDTO>>
                {
                    Success = false,
                    Message = $" cartItem With ID{id} Not Found.",
                    Errors = ["Empty result"]
                });
            }

            return Ok(new ApiResponse<CartItemDTO>
            {
                Success = true,
                Message = "CartItem retrieved successfully",
                Data = result
            });
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
                return BadRequest(ModelState);
            // أولًا: التحقق من أن الكائن نفسه وبياناته الأساسية موجودة
            if (CartItem == null)
            {

                return NotFound(new ApiResponse<CartItemDTO>
                {
                    Success = false,
                    Message = $" cart With ID{CartItem?.CartItemId} Not Added.",
                    Errors = ["Empty result"]
                });
            }
            //var PhoneNum = newDoctor.Phone.Trim().ToString();

            var NewCartItem = await _service.AddNewCartItem(CartItem);
            if (NewCartItem == null)
            {
                return NotFound(new ApiResponse<CartItemDTO>
                {
                    Success = false,
                    Message = $" cartItem With ID{CartItem?.CartItemId} Not Added.",
                    Errors = ["Empty result"]
                });
            }
            return Ok(new ApiResponse<CartItemDTO>
            {
                Success = true,
                Message = "CartItem Added successfully",
                Data = NewCartItem
            });
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
                return BadRequest(ModelState);

            var result = await _service.UpdateCartItemById(CartItem);
            if (!result)
            {
                return NotFound(new ApiResponse<CartItemDTO>
                {
                    Success = false,
                    Message = $" cart With ID{CartItem.CartItemId} Not Updated.",
                    Errors = ["Empty result"]
                });
            }
            return Ok(new ApiResponse<CartItemDTO>
            {
                Success = true,
                Message = $" cart With ID{CartItem.CartItemId}  Updated Successfully.",
                Data = CartItem
            });
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
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.DeleteCartItemById(id);
            if (!result)

                 return NotFound(new ApiResponse<CartItemDTO>
                 {
                     Success = false,
                     Message = $" Sorry CartItem Didnot Deleted! Please Try Again",
                     Errors = ["Empty result"]
                 });

            return Ok(new ApiResponse<CartDTO>
            {
                Success = true,
                Message = "CartItem  Deleted Successfully",
                Data = null
            });
        }


    }
}
