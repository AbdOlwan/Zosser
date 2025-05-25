using API_OnlineStore.Common;
using API_OnlineStore.Helpers;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Cart_F;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Order_F;
using BLL_OnlineStore.Interfaces.OrderBusServices;
using DAL_OnlineStore.Entities.Models.CartModels;
using DAL_OnlineStore.Entities.Models.OrderModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API_OnlineStore.Controllers.Order_Controllers
{
    [ApiController]
    [Route("api/v1/order-items")]
    public class OrderItemsController : ControllerBase
    {
        private readonly IOrderItemService _service;
        public OrderItemsController(IOrderItemService service) => _service = service;

        [HttpGet("by-order/{orderId:int}")]
        public async Task<ActionResult<IEnumerable<OrderItemDTO>>> GetByOrder(int orderId)
        {
            try { return Ok(await _service.GetItemsByOrderIdAsync(orderId)); }
            catch (Exception ex) { return StatusCode(500, new ApiResponse(500, ex.Message)); }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderItemDTO>> Get(int id)
        {
            try
            {
                var item = await _service.GetOrderItemByIdAsync(id);
                if (item == null) return NotFound(new ApiResponse(404, $"Item {id} not found"));
                return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, ex.Message));
            }
        }

        [HttpPost]
        public async Task<ActionResult<OrderItemDTO>> Create([FromBody] OrderItemDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiResponse(400, ModelState));
            try
            {
                var created = await _service.CreateOrderItemAsync(dto);
                return CreatedAtAction(nameof(Get), new { id = created.OrderItemID }, created);
            }
            catch (Exception ex) { return StatusCode(500, new ApiResponse(500, ex.Message)); }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] OrderItemDTO dto)
        {
            if (id != dto.OrderItemID) return BadRequest(new ApiResponse(400, "ID mismatch"));
            if (!ModelState.IsValid) return BadRequest(new ApiResponse(400, ModelState));
            try
            {
                if (!await _service.OrderItemExistsAsync(id))
                    return NotFound(new ApiResponse(404, $"Item {id} not found"));
                await _service.UpdateOrderItemAsync(dto);
                return NoContent();
            }
            catch (Exception ex) { return StatusCode(500, new ApiResponse(500, ex.Message)); }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (!await _service.OrderItemExistsAsync(id))
                    return NotFound(new ApiResponse(404, $"Item {id} not found"));
                await _service.DeleteOrderItemAsync(id);
                return Ok(new ApiResponse(200, $"Item {id} deleted"));
            }
            catch (Exception ex) { return StatusCode(500, new ApiResponse(500, ex.Message)); }
        }
    }

}
