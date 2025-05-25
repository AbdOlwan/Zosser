using BLL_OnlineStore.DTOs.EntitiesDTOs.Payment_F;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_OnlineStore.Interfaces.PaymentBusServices
{
    public interface IPaymentService
    {
        Task<IEnumerable<PaymentDTO>> GetAllPaymentsAsync();
        Task<PaymentDTO?> GetPaymentByIdAsync(int id);
        Task<PaymentDTO> CreatePaymentAsync(PaymentDTO createPaymentDTO);
        Task UpdatePaymentAsync(PaymentDTO updatePaymentDTO);
        Task DeletePaymentAsync(int id);
        Task<bool> PaymentExistsAsync(int id);
    }

}
