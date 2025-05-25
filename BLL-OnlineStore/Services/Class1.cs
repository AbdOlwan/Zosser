using AutoMapper;
using BLL_OnlineStore.DTOs.EntitiesDTOs;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Order_F;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Payment_F;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Shipment_F;
using BLL_OnlineStore.Services.Interfaces;
using DAL_OnlineStore.Entities.Models.OrderModels;
using DAL_OnlineStore.Entities.Models.PaymentModels;
using DAL_OnlineStore.Entities.Models.ShipmentModels;
using DAL_OnlineStore.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL_OnlineStore.Services
{
    //public class OrderService : IOrderService
    //{
    //    private readonly IUnitOfWork _unitOfWork;
    //    private readonly IMapper _mapper;

    //    public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
    //    {
    //        _unitOfWork = unitOfWork;
    //        _mapper = mapper;
    //    }

    //    public async Task<IEnumerable<OrderDTO>> GetAllOrdersAsync()
    //    {
    //        var orders = await _unitOfWork.orders.GetAllOrdersAsync();
    //        return _mapper.Map<IEnumerable<OrderDTO>>(orders);
    //    }

    //    public async Task<OrderDTO?> GetOrderByIdAsync(int id)
    //    {
    //        var order = await _unitOfWork.orders.GetOrderByIdAsync(id);
    //        if (order == null) return null;
    //        return _mapper.Map<OrderDTO>(order);
    //    }

    //    public async Task<OrderDTO> CreateOrderAsync(OrderDTO createOrderDTO)
    //    {
    //        var order = _mapper.Map<Order>(createOrderDTO);
    //        await _unitOfWork.orders.AddOrderAsync(order);
    //        await _unitOfWork.CompleteAsync();
    //        return _mapper.Map<OrderDTO>(order);
    //    }

    //    public async Task UpdateOrderAsync(OrderDTO updateOrderDTO)
    //    {
    //        var order = _mapper.Map<Order>(updateOrderDTO);
    //        await _unitOfWork.orders.UpdateOrderAsync(order);
    //        await _unitOfWork.CompleteAsync();
    //    }

    //    public async Task DeleteOrderAsync(int id)
    //    {
    //        await _unitOfWork.orders.DeleteOrderAsync(id);
    //        await _unitOfWork.CompleteAsync();
    //    }

    //    public async Task<bool> OrderExistsAsync(int id)
    //    {
    //        return await _unitOfWork.orders.OrderExistsAsync(id);
    //    }
    //}

    //public class OrderItemService : IOrderItemService
    //{
    //    private readonly IUnitOfWork _unitOfWork;
    //    private readonly IMapper _mapper;

    //    public OrderItemService(IUnitOfWork unitOfWork, IMapper mapper)
    //    {
    //        _unitOfWork = unitOfWork;
    //        _mapper = mapper;
    //    }

    //    public async Task<IEnumerable<OrderItemDTO>> GetItemsByOrderIdAsync(int orderId)
    //    {
    //        var items = await _unitOfWork.OrderItems.GetItemsByOrderIdAsync(orderId);
    //        return _mapper.Map<IEnumerable<OrderItemDTO>>(items);
    //    }

    //    public async Task<OrderItemDTO?> GetOrderItemByIdAsync(int id)
    //    {
    //        var item = await _unitOfWork.OrderItems.GetOrderItemByIdAsync(id);
    //        if (item == null) return null;
    //        return _mapper.Map<OrderItemDTO>(item);
    //    }

    //    public async Task<OrderItemDTO> CreateOrderItemAsync(OrderItemDTO createOrderItemDTO)
    //    {
    //        var item = _mapper.Map<OrderItem>(createOrderItemDTO);
    //        await _unitOfWork.OrderItems.AddOrderItemAsync(item);
    //        await _unitOfWork.CompleteAsync();
    //        return _mapper.Map<OrderItemDTO>(item);
    //    }

    //    public async Task UpdateOrderItemAsync(OrderItemDTO updateOrderItemDTO)
    //    {
    //        var item = _mapper.Map<OrderItem>(updateOrderItemDTO);
    //        await _unitOfWork.OrderItems.UpdateOrderItemAsync(item);
    //        await _unitOfWork.CompleteAsync();
    //    }

    //    public async Task DeleteOrderItemAsync(int id)
    //    {
    //        await _unitOfWork.OrderItems.DeleteOrderItemAsync(id);
    //        await _unitOfWork.CompleteAsync();
    //    }

    //    public async Task<bool> OrderItemExistsAsync(int id)
    //    {
    //        return await _unitOfWork.OrderItems.OrderItemExistsAsync(id);
    //    }
    //}

    //public class PaymentService : IPaymentService
    //{
    //    private readonly IUnitOfWork _unitOfWork;
    //    private readonly IMapper _mapper;

    //    public PaymentService(IUnitOfWork unitOfWork, IMapper mapper)
    //    {
    //        _unitOfWork = unitOfWork;
    //        _mapper = mapper;
    //    }

    //    public async Task<IEnumerable<PaymentDTO>> GetAllPaymentsAsync()
    //    {
    //        var payments = await _unitOfWork.Payments.GetAllPaymentsAsync();
    //        return _mapper.Map<IEnumerable<PaymentDTO>>(payments);
    //    }

    //    public async Task<PaymentDTO?> GetPaymentByIdAsync(int id)
    //    {
    //        var payment = await _unitOfWork.Payments.GetPaymentByIdAsync(id);
    //        if (payment == null) return null;
    //        return _mapper.Map<PaymentDTO>(payment);
    //    }

    //    public async Task<PaymentDTO> CreatePaymentAsync(PaymentDTO createPaymentDTO)
    //    {
    //        var payment = _mapper.Map<Payment>(createPaymentDTO);
    //        await _unitOfWork.Payments.AddPaymentAsync(payment);
    //        await _unitOfWork.CompleteAsync();
    //        return _mapper.Map<PaymentDTO>(payment);
    //    }

    //    public async Task UpdatePaymentAsync(PaymentDTO updatePaymentDTO)
    //    {
    //        var payment = _mapper.Map<Payment>(updatePaymentDTO);
    //        await _unitOfWork.Payments.UpdatePaymentAsync(payment);
    //        await _unitOfWork.CompleteAsync();
    //    }

    //    public async Task DeletePaymentAsync(int id)
    //    {
    //        await _unitOfWork.Payments.DeletePaymentAsync(id);
    //        await _unitOfWork.CompleteAsync();
    //    }

    //    public async Task<bool> PaymentExistsAsync(int id)
    //    {
    //        return await _unitOfWork.Payments.PaymentExistsAsync(id);
    //    }
    //}

    //public class ShipmentService : IShipmentService
    //{
    //    private readonly IUnitOfWork _unitOfWork;
    //    private readonly IMapper _mapper;

    //    public ShipmentService(IUnitOfWork unitOfWork, IMapper mapper)
    //    {
    //        _unitOfWork = unitOfWork;
    //        _mapper = mapper;
    //    }

    //    public async Task<IEnumerable<ShipmentDTO>> GetAllShipmentsAsync()
    //    {
    //        var shipments = await _unitOfWork.Shipments.GetAllShipmentsAsync();
    //        return _mapper.Map<IEnumerable<ShipmentDTO>>(shipments);
    //    }

    //    public async Task<ShipmentDTO?> GetShipmentByIdAsync(int id)
    //    {
    //        var shipment = await _unitOfWork.Shipments.GetShipmentByIdAsync(id);
    //        if (shipment == null) return null;
    //        return _mapper.Map<ShipmentDTO>(shipment);
    //    }

    //    public async Task<ShipmentDTO> CreateShipmentAsync(ShipmentDTO createShipmentDTO)
    //    {
    //        var shipment = _mapper.Map<Shipment>(createShipmentDTO);
    //        await _unitOfWork.Shipments.AddShipmentAsync(shipment);
    //        await _unitOfWork.CompleteAsync();
    //        return _mapper.Map<ShipmentDTO>(shipment);
    //    }

    //    public async Task UpdateShipmentAsync(ShipmentDTO updateShipmentDTO)
    //    {
    //        var shipment = _mapper.Map<Shipment>(updateShipmentDTO);
    //        await _unitOfWork.Shipments.UpdateShipmentAsync(shipment);
    //        await _unitOfWork.CompleteAsync();
    //    }

    //    public async Task DeleteShipmentAsync(int id)
    //    {
    //        await _unitOfWork.Shipments.DeleteShipmentAsync(id);
    //        await _unitOfWork.CompleteAsync();
    //    }

    //    public async Task<bool> ShipmentExistsAsync(int id)
    //    {
    //        return await _unitOfWork.Shipments.ShipmentExistsAsync(id);
    //    }
    //}
}
