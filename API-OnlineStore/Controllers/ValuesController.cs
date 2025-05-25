using API_OnlineStore.Common;
using API_OnlineStore.Helpers;
using BLL_OnlineStore.DTOs.EntitiesDTOs;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Order_F;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Payment_F;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Shipment_F;
using BLL_OnlineStore.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API_OnlineStore.Controllers
{
    //[ApiController]
    //[Route("api/v1/orders")]
    //public class OrdersController : ControllerBase
    //{
    //    private readonly IOrderService _orderService;

    //    public OrdersController(IOrderService orderService)
    //    {
    //        _orderService = orderService;
    //    }

    //    [HttpGet]
    //    public async Task<ActionResult<IEnumerable<OrderDTO>>> GetAll()
    //    {
    //        try
    //        {
    //            var orders = await _orderService.GetAllOrdersAsync();
    //            return Ok(orders);
    //        }
    //        catch (Exception ex)
    //        {
    //            return StatusCode(500, new ApiResponse(500, ex.Message));
    //        }
    //    }

    //    [HttpGet("{id:int}")]
    //    public async Task<ActionResult<OrderDTO>> GetById(int id)
    //    {
    //        try
    //        {
    //            var order = await _orderService.GetOrderByIdAsync(id);
    //            if (order == null)
    //                return NotFound(new ApiResponse(404, $"Order {id} not found"));
    //            return Ok(order);
    //        }
    //        catch (Exception ex)
    //        {
    //            return StatusCode(500, new ApiResponse(500, ex.Message));
    //        }
    //    }

    //    [HttpPost]
    //    public async Task<ActionResult<OrderDTO>> Create([FromBody] OrderDTO dto)
    //    {
    //        if (!ModelState.IsValid)
    //            return BadRequest(new ApiResponse(400, ModelState));
    //        try
    //        {
    //            var created = await _orderService.CreateOrderAsync(dto);
    //            return CreatedAtAction(nameof(GetById), new { id = created.OrderID }, created);
    //        }
    //        catch (Exception ex)
    //        {
    //            return StatusCode(500, new ApiResponse(500, ex.Message));
    //        }
    //    }

    //    [HttpPut("{id:int}")]
    //    public async Task<IActionResult> Update(int id, [FromBody] OrderDTO dto)
    //    {
    //        if (id != dto.OrderID)
    //            return BadRequest(new ApiResponse(400, "ID mismatch"));
    //        if (!ModelState.IsValid)
    //            return BadRequest(new ApiResponse(400, ModelState));
    //        try
    //        {
    //            if (!await _orderService.OrderExistsAsync(id))
    //                return NotFound(new ApiResponse(404, $"Order {id} not found"));
    //            await _orderService.UpdateOrderAsync(dto);
    //            return NoContent();
    //        }
    //        catch (Exception ex)
    //        {
    //            return StatusCode(500, new ApiResponse(500, ex.Message));
    //        }
    //    }

    //    [HttpDelete("{id:int}")]
    //    public async Task<IActionResult> Delete(int id)
    //    {
    //        try
    //        {
    //            if (!await _orderService.OrderExistsAsync(id))
    //                return NotFound(new ApiResponse(404, $"Order {id} not found"));
    //            await _orderService.DeleteOrderAsync(id);
    //            return Ok(new ApiResponse(200, $"Order {id} deleted"));
    //        }
    //        catch (Exception ex)
    //        {
    //            return StatusCode(500, new ApiResponse(500, ex.Message));
    //        }
    //    }
    //}

    //[ApiController]
    //[Route("api/v1/order-items")]
    //public class OrderItemsController : ControllerBase
    //{
    //    private readonly IOrderItemService _service;
    //    public OrderItemsController(IOrderItemService service) => _service = service;

    //    [HttpGet("by-order/{orderId:int}")]
    //    public async Task<ActionResult<IEnumerable<OrderItemDTO>>> GetByOrder(int orderId)
    //    {
    //        try { return Ok(await _service.GetItemsByOrderIdAsync(orderId)); }
    //        catch (Exception ex) { return StatusCode(500, new ApiResponse(500, ex.Message)); }
    //    }

    //    [HttpGet("{id:int}")]
    //    public async Task<ActionResult<OrderItemDTO>> Get(int id)
    //    {
    //        try
    //        {
    //            var item = await _service.GetOrderItemByIdAsync(id);
    //            if (item == null) return NotFound(new ApiResponse(404, $"Item {id} not found"));
    //            return Ok(item);
    //        }
    //        catch (Exception ex)
    //        {
    //            return StatusCode(500, new ApiResponse(500, ex.Message));
    //        }
    //    }

    //    [HttpPost]
    //    public async Task<ActionResult<OrderItemDTO>> Create([FromBody] OrderItemDTO dto)
    //    {
    //        if (!ModelState.IsValid) return BadRequest(new ApiResponse(400, ModelState));
    //        try
    //        {
    //            var created = await _service.CreateOrderItemAsync(dto);
    //            return CreatedAtAction(nameof(Get), new { id = created.OrderItemID }, created);
    //        }
    //        catch (Exception ex) { return StatusCode(500, new ApiResponse(500, ex.Message)); }
    //    }

    //    [HttpPut("{id:int}")]
    //    public async Task<IActionResult> Update(int id, [FromBody] OrderItemDTO dto)
    //    {
    //        if (id != dto.OrderItemID) return BadRequest(new ApiResponse(400, "ID mismatch"));
    //        if (!ModelState.IsValid) return BadRequest(new ApiResponse(400, ModelState));
    //        try
    //        {
    //            if (!await _service.OrderItemExistsAsync(id))
    //                return NotFound(new ApiResponse(404, $"Item {id} not found"));
    //            await _service.UpdateOrderItemAsync(dto);
    //            return NoContent();
    //        }
    //        catch (Exception ex) { return StatusCode(500, new ApiResponse(500, ex.Message)); }
    //    }

    //    [HttpDelete("{id:int}")]
    //    public async Task<IActionResult> Delete(int id)
    //    {
    //        try
    //        {
    //            if (!await _service.OrderItemExistsAsync(id))
    //                return NotFound(new ApiResponse(404, $"Item {id} not found"));
    //            await _service.DeleteOrderItemAsync(id);
    //            return Ok(new ApiResponse(200, $"Item {id} deleted"));
    //        }
    //        catch (Exception ex) { return StatusCode(500, new ApiResponse(500, ex.Message)); }
    //    }
    //}

    //[ApiController]
    //[Route("api/v1/payments")]
    //public class PaymentsController : ControllerBase
    //{
    //    private readonly IPaymentService _service;
    //    public PaymentsController(IPaymentService service) => _service = service;

    //    [HttpGet]
    //    public async Task<ActionResult<IEnumerable<PaymentDTO>>> GetAll() => Ok(await _service.GetAllPaymentsAsync());

    //    [HttpGet("{id:int}")]
    //    public async Task<ActionResult<PaymentDTO>> Get(int id)
    //    {
    //        var p = await _service.GetPaymentByIdAsync(id);
    //        if (p == null) return NotFound(new ApiResponse(404, $"Payment {id} not found"));
    //        return Ok(p);
    //    }

    //    [HttpPost]
    //    public async Task<ActionResult<PaymentDTO>> Create([FromBody] PaymentDTO dto)
    //    {
    //        if (!ModelState.IsValid) return BadRequest(new ApiResponse(400, ModelState));
    //        var created = await _service.CreatePaymentAsync(dto);
    //        return CreatedAtAction(nameof(Get), new { id = created.PaymentID }, created);
    //    }

    //    [HttpPut("{id:int}")]
    //    public async Task<IActionResult> Update(int id, [FromBody] PaymentDTO dto)
    //    {
    //        if (id != dto.PaymentID) return BadRequest(new ApiResponse(400, "ID mismatch"));
    //        if (!ModelState.IsValid) return BadRequest(new ApiResponse(400, ModelState));
    //        if (!await _service.PaymentExistsAsync(id)) return NotFound(new ApiResponse(404, $"Payment {id} not found"));
    //        await _service.UpdatePaymentAsync(dto);
    //        return NoContent();
    //    }

    //    [HttpDelete("{id:int}")]
    //    public async Task<IActionResult> Delete(int id)
    //    {
    //        if (!await _service.PaymentExistsAsync(id)) return NotFound(new ApiResponse(404, $"Payment {id} not found"));
    //        await _service.DeletePaymentAsync(id);
    //        return Ok(new ApiResponse(200, $"Payment {id} deleted"));
    //    }
    //}

    //[ApiController]
    //[Route("api/v1/shipments")]
    //public class ShipmentsController : ControllerBase
    //{
    //    private readonly IShipmentService _service;
    //    public ShipmentsController(IShipmentService service) => _service = service;

    //    [HttpGet]
    //    public async Task<ActionResult<IEnumerable<ShipmentDTO>>> GetAll() => Ok(await _service.GetAllShipmentsAsync());

    //    [HttpGet("{id:int}")]
    //    public async Task<ActionResult<ShipmentDTO>> Get(int id)
    //    {
    //        var s = await _service.GetShipmentByIdAsync(id);
    //        if (s == null) return NotFound(new ApiResponse(404, $"Shipment {id} not found"));
    //        return Ok(s);
    //    }

    //    [HttpPost]
    //    public async Task<ActionResult<ShipmentDTO>> Create([FromBody] ShipmentDTO dto)
    //    {
    //        if (!ModelState.IsValid) return BadRequest(new ApiResponse(400, ModelState));
    //        var created = await _service.CreateShipmentAsync(dto);
    //        return CreatedAtAction(nameof(Get), new { id = created.ShipmentID }, created);
    //    }

    //    [HttpPut("{id:int}")]
    //    public async Task<IActionResult> Update(int id, [FromBody] ShipmentDTO dto)
    //    {
    //        if (id != dto.ShipmentID) return BadRequest(new ApiResponse(400, "ID mismatch"));
    //        if (!ModelState.IsValid) return BadRequest(new ApiResponse(400, ModelState));
    //        if (!await _service.ShipmentExistsAsync(id)) return NotFound(new ApiResponse(404, $"Shipment {id} not found"));
    //        await _service.UpdateShipmentAsync(dto);
    //        return NoContent();
    //    }

    //    [HttpDelete("{id:int}")]
    //    public async Task<IActionResult> Delete(int id)
    //    {
    //        if (!await _service.ShipmentExistsAsync(id)) return NotFound(new ApiResponse(404, $"Shipment {id} not found"));
    //        await _service.DeleteShipmentAsync(id);
    //        return Ok(new ApiResponse(200, $"Shipment {id} deleted"));
    //    }
    //}
}
