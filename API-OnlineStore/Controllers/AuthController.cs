using API_OnlineStore.Common;
using API_OnlineStore.Helpers;
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

        /// <summary>
        /// Register a new user and return authentication token if successful.
        /// </summary>
        /// <param name="model">Registration data (username, email, password, etc.)</param>
        /// <returns>Authentication token and user information</returns>
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, ModelState));

            try
            {
                var result = await _authService.RegisterAsync(model);

                if (result == null)
                    return BadRequest(new ApiResponse(400, "Registration failed"));

                if (!result.IsAuthenticated)
                    return BadRequest(new ApiResponse(400, result.Message));

                return Ok(new
                {
                    userName = result.Username,
                    token = result.Token
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"An error occurred while registering user: {ex.Message}"));
            }
        }



        /// <summary>
        /// Authenticate user and return JWT token if credentials are valid.
        /// </summary>
        /// <param name="loginDto">User login data (email/username and password)</param>
        /// <returns>JWT token and user information if login is successful</returns>
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDTO loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, ModelState));

            try
            {
                var result = await _authService.LoginAsync(loginDto);

                if (!result.IsAuthenticated)
                    return BadRequest(new ApiResponse(400, result.Message));

                return Ok(new
                {
                    userName = result.Username,
                    token = result.Token
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"An error occurred while logging in: {ex.Message}"));
            }
        }



        /// <summary>
        /// Add a role to a specific user.
        /// </summary>
        /// <param name="roleDto">Role assignment data (user ID/email and role name)</param>
        /// <returns>Status of role assignment</returns>
        [HttpPost("add-role")]
        // [Authorize(Roles = "Admin")] // Uncomment to restrict to Admins only
        public async Task<IActionResult> AddUserRole([FromBody] AddRoleDTO roleDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400,  ModelState));

            try
            {
                var result = await _authService.AddUserRoleAsynce(roleDto);

                if (!result)
                    return BadRequest(new ApiResponse(400, "Failed to add role to user. Either user does not exist or the role is already assigned."));

                return Ok(new ApiResponse(200, "Role added successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"An error occurred while assigning role: {ex.Message}"));
            }
        }


        /// <summary>
        /// Add a claim to a specific user.
        /// </summary>
        /// <param name="claim">Claim data (user ID/email and claim info)</param>
        /// <returns>Status of claim assignment</returns>
        [HttpPost("add-claim")]
        // [Authorize(Roles = "Admin")] // Uncomment to restrict to Admins only
        public async Task<IActionResult> AddUserClaimAsync([FromBody] AddClaimDTO claim)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, ModelState));

            try
            {
                var result = await _authService.AddUserClaimAsync(claim);

                if (!result)
                    return BadRequest(new ApiResponse(400, "Failed to add claim to user. Either user does not exist or the claim is already assigned."));

                return Ok(new ApiResponse(200, "Claim added successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"An error occurred while assigning claim: {ex.Message}"));
            }
        }


    }
}
