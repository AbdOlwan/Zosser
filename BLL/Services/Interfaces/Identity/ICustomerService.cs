using Shared.DTOs;
using Shared.DTOs.CustomerDtos;
using Shared.Models;

namespace BLL.Services.Interfaces.Identity
{
    public interface ICustomerService
    {
        Task<CustomerDto> GetCustomerByIdAsync(int id);
        //Task<CustomerDto> CreateCustomerAsync(CreateCustomerDto dto);
        Task<CustomerDto> UpdateCustomerAsync(UpdateCustomerDto dto);
        Task<bool> ToggleCustomerStatusAsync(ToggleCustomerStatusDto dto);
        Task<bool> ChangePasswordAsync(ChangePasswordDto dto);
        Task<IEnumerable<CustomerSummaryDto>> GetAllCustomersAsync();
    }
}
