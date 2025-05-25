using API_OnlineStore.Common;
using API_OnlineStore.Helpers;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Cart_F;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Payment_F;
using BLL_OnlineStore.Interfaces.PaymentBusServices;
using DAL_OnlineStore.Entities.Models.CartModels;
using DAL_OnlineStore.Entities.Models.PaymentModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API_OnlineStore.Controllers.Payment_Controllers
{
    [ApiController]
    [Route("api/v1/payments")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _service;
        public PaymentsController(IPaymentService service) => _service = service;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentDTO>>> GetAll() => Ok(await _service.GetAllPaymentsAsync());

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PaymentDTO>> Get(int id)
        {
            var p = await _service.GetPaymentByIdAsync(id);
            if (p == null) return NotFound(new ApiResponse(404, $"Payment {id} not found"));
            return Ok(p);
        }

        [HttpPost]
        public async Task<ActionResult<PaymentDTO>> Create([FromBody] PaymentDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiResponse(400, ModelState));
            var created = await _service.CreatePaymentAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = created.PaymentID }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] PaymentDTO dto)
        {
            if (id != dto.PaymentID) return BadRequest(new ApiResponse(400, "ID mismatch"));
            if (!ModelState.IsValid) return BadRequest(new ApiResponse(400, ModelState));
            if (!await _service.PaymentExistsAsync(id)) return NotFound(new ApiResponse(404, $"Payment {id} not found"));
            await _service.UpdatePaymentAsync(dto);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _service.PaymentExistsAsync(id)) return NotFound(new ApiResponse(404, $"Payment {id} not found"));
            await _service.DeletePaymentAsync(id);
            return Ok(new ApiResponse(200, $"Payment {id} deleted"));
        }
    }
}
