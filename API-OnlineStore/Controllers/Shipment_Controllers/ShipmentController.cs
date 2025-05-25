using API_OnlineStore.Common;
using API_OnlineStore.Helpers;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Cart_F;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Shipment_F;
using BLL_OnlineStore.Interfaces.ShipmentBusServices;
using DAL_OnlineStore.Entities.Models.CartModels;
using DAL_OnlineStore.Entities.Models.ShipmentModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API_OnlineStore.Controllers.Shipment_Controllers
{
    [ApiController]
    [Route("api/v1/shipments")]
    public class ShipmentsController : ControllerBase
    {
        private readonly IShipmentService _service;
        public ShipmentsController(IShipmentService service) => _service = service;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShipmentDTO>>> GetAll() => Ok(await _service.GetAllShipmentsAsync());

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ShipmentDTO>> Get(int id)
        {
            var s = await _service.GetShipmentByIdAsync(id);
            if (s == null) return NotFound(new ApiResponse(404, $"Shipment {id} not found"));
            return Ok(s);
        }

        [HttpPost]
        public async Task<ActionResult<ShipmentDTO>> Create([FromBody] ShipmentDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiResponse(400, ModelState));
            var created = await _service.CreateShipmentAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = created.ShipmentID }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] ShipmentDTO dto)
        {
            if (id != dto.ShipmentID) return BadRequest(new ApiResponse(400, "ID mismatch"));
            if (!ModelState.IsValid) return BadRequest(new ApiResponse(400, ModelState));
            if (!await _service.ShipmentExistsAsync(id)) return NotFound(new ApiResponse(404, $"Shipment {id} not found"));
            await _service.UpdateShipmentAsync(dto);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _service.ShipmentExistsAsync(id)) return NotFound(new ApiResponse(404, $"Shipment {id} not found"));
            await _service.DeleteShipmentAsync(id);
            return Ok(new ApiResponse(200, $"Shipment {id} deleted"));
        }
    }
}
