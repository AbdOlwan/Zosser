using API_OnlineStore.Common;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Cart_F;
using BLL_OnlineStore.Interfaces.CartBusServices;
using Microsoft.AspNetCore.Mvc;

namespace API_OnlineStore.Controllers.Cart_Controllers
{
    [Route("api/v1/carts")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartServices _service;

        public CartController(ICartServices service)
        {
            _service = service;
        }

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
                return BadRequest(ModelState);
            try
            {
                var Carts = await _service.GetAllCarts();
                if (Carts == null || Carts.Count == 0)
                {
                    return NotFound(new ApiResponse<IEnumerable<CartDTO>>
                    {
                        Success = false,
                        Message = "No carts found.",
                        Errors = ["Empty result"]
                    });

                }
                return Ok(new ApiResponse<IEnumerable<CartDTO>>
                {
                    Success = true,
                    Message = "Carts fetched successfully",
                    Data = Carts
                });

            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving Carriers.");

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
                return BadRequest(ModelState);

            var result = await _service.GetCartById(id);
            if (result == null)
            {
                return NotFound(new ApiResponse<IEnumerable<CartDTO>>
                {
                    Success = false,
                    Message = $"No cart With ID{id} found.",
                    Errors = ["Empty result"]
                });
            }

            return Ok(new ApiResponse<CartDTO>
            {
                Success = true,
                Message = "Cart retrieved successfully",
                Data = result
            });
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
                return BadRequest(ModelState);
            // أولًا: التحقق من أن الكائن نفسه وبياناته الأساسية موجودة
            if (Cart == null)
            {
                return NotFound(new ApiResponse<IEnumerable<CartDTO>>
                {
                    Success = false,
                    Message = $" cart With ID{Cart?.CartId} Not Added.",
                    Errors = ["Empty result"]
                });
            }
            //var PhoneNum = newDoctor.Phone.Trim().ToString();

            var NewCart = await _service.AddNewCart(Cart);
            if (NewCart == null)
            {
                return NotFound(new ApiResponse<IEnumerable<CartDTO>>
                {
                    Success = false,
                    Message = $" cart With ID{Cart?.CartId} Not Added.",
                    Errors = ["Empty result"]
                });
            }
            return Ok(new ApiResponse<CartDTO>
            {
                Success = true,
                Message = "Cart retrieved successfully",
                Data = NewCart
            });
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
                return BadRequest(ModelState);

            var result = await _service.UpdateCartById(Cart);
            if (!result)
            {
                return NotFound(new ApiResponse<IEnumerable<CartDTO>>
                {
                    Success = false,
                    Message = $" cart With ID{Cart?.CartId} Not Added.",
                    Errors = ["Empty result"]
                });
            }
            return Ok(new ApiResponse<CartDTO>
            {
                Success = true,
                Message = "Cart Updated successfully",
                Data = Cart
            });
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
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.DeleteCartById(id);
            if (!result)
                return NotFound(new ApiResponse<IEnumerable<CartDTO>>
                {
                    Success = false,
                    Message = $" cart With ID{id} Not Added.",
                    Errors = ["Empty result"]
                });
            return Ok(new ApiResponse<CartDTO>
            {
                Success = true,
                Message = $"Cart With ID{id} Deleted successfully",
                Data = null
            });
        }

    }
}
