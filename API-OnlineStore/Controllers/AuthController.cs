using BLL_OnlineStore.DTOs.UserDTOs;
using BLL_OnlineStore.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_OnlineStore.Controllers
{
    [Route("api/v1/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(model);
            if(result == null)
            {
                return BadRequest(ModelState);
            }

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(new
            {
                userName = result.Username,
                Token = result.Token,
            });
        }


        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginDTO loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.LoginAsync(loginDto);
            if (!result.IsAuthenticated)
                return BadRequest(result.Message);
            return Ok(new
            {
                userName = result.Username,
                Token = result.Token,
            });


        }


        //[Authorize(Roles = "Admin")] // Optional: Only Admins can add roles
        [HttpPost("add-role")]
        public async Task<IActionResult> AddUserRole([FromBody] AddRoleDTO roleDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.AddUserRoleAsynce(roleDto);

            if (!result)
                return BadRequest("Failed to add role to user. Either user does not exist or the role is already assigned.");

            return Ok("Role added successfully.");
        }

        //[Authorize(Roles ="Admin")]
        [HttpPost("add-claim")]
        public async Task<IActionResult> AddUserClaimAsync([FromBody] AddClaimDTO claim)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.AddUserClaimAsync(claim);
            if (!result)
                return BadRequest("Failed to add Claim to user. Either user does not exist or the Claim is already assigned.");
            return Ok("Claim added successfully.");
        }

    }
}
