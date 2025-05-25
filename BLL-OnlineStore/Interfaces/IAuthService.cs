using BLL_OnlineStore.DTOs.UserDTOs;
using DAL_OnlineStore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_OnlineStore.Interfaces
{
    public interface IAuthService
    {
        Task<AuthModel?> RegisterAsync(RegisterDTO model);
        Task<AuthModel> LoginAsync(LoginDTO loginDto);

        Task<AuthModel> GetTokenAsync(TokenRequestDTO token);
        Task<bool> AddUserRoleAsynce(AddRoleDTO role);

        Task<bool> AddUserClaimAsync(AddClaimDTO claim);

    }
}
