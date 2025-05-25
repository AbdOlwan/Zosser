using AutoMapper;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Payment_F;
using BLL_OnlineStore.Interfaces.PaymentBusServices;
using DAL_OnlineStore.Entities.Models.PaymentModels;
using DAL_OnlineStore.Repositories.Interfaces;
using DAL_OnlineStore.Repositories.Interfaces.PaymentRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_OnlineStore.Services.PaymentBusServices
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PaymentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PaymentDTO>> GetAllPaymentsAsync()
        {
            var payments = await _unitOfWork.Payments.GetAllPaymentsAsync();
            return _mapper.Map<IEnumerable<PaymentDTO>>(payments);
        }

        public async Task<PaymentDTO?> GetPaymentByIdAsync(int id)
        {
            var payment = await _unitOfWork.Payments.GetPaymentByIdAsync(id);
            if (payment == null) return null;
            return _mapper.Map<PaymentDTO>(payment);
        }

        public async Task<PaymentDTO> CreatePaymentAsync(PaymentDTO createPaymentDTO)
        {
            var payment = _mapper.Map<Payment>(createPaymentDTO);
            await _unitOfWork.Payments.AddPaymentAsync(payment);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<PaymentDTO>(payment);
        }

        public async Task UpdatePaymentAsync(PaymentDTO updatePaymentDTO)
        {
            var payment = _mapper.Map<Payment>(updatePaymentDTO);
            await _unitOfWork.Payments.UpdatePaymentAsync(payment);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeletePaymentAsync(int id)
        {
            await _unitOfWork.Payments.DeletePaymentAsync(id);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<bool> PaymentExistsAsync(int id)
        {
            return await _unitOfWork.Payments.PaymentExistsAsync(id);
        }
    }

}
