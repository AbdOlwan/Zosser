using BLL.Exceptions;
using BLL.Services.Interfaces.Shipments;
using Microsoft.AspNetCore.Mvc;
using Shared.Constants;
using Shared.DTOs;
using Shared.DTOs.Shipments;
using Shared.Interfaces;
using Shared.Utils;
using System.ComponentModel.DataAnnotations;

namespace API.Controllers.Shipments
{
    [Route("api/v1/delivery-agents")]
    [ApiController]
    public class DeliveryAgentController : ControllerBase
    {
        private readonly IDeliveryAgentService _deliveryAgentService;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<DeliveryAgentController> _logger;

        public DeliveryAgentController(
            IDeliveryAgentService deliveryAgentService,
            ICurrentUserService currentUserService,
            ILogger<DeliveryAgentController> logger)
        {
            _deliveryAgentService = deliveryAgentService;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        /// <summary>
        /// إنشاء مندوب توصيل جديد
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<DeliveryAgentResponseDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateDeliveryAgent([FromBody] DeliveryAgentCreateDTO createDto)
        {
            using var activity = ActivitySourceProvider.StartActivity();

            if (!ModelState.IsValid)
                return ApiResponse.CreateBadRequest(ModelState).ToActionResult();

            try
            {
                var createdAgent = await _deliveryAgentService.CreateDeliveryAgentAsync(createDto);
                return ApiResponse<DeliveryAgentResponseDTO>
                    .CreateCreated(createdAgent, ApiMessages.Created)
                    .ToActionResult();
            }
            catch (BusinessException ex)
            {
                _logger.LogWarning(ex, "Business rule violation: {Message}", ex.Message);
                return ApiResponse.CreateConflict(ex.Message).ToActionResult();
            }
            catch (ServiceException ex)
            {
                _logger.LogError(ex, "Service error while creating delivery agent");
                return ApiResponse.CreateInternalServerError(ApiMessages.InternalServerError).ToActionResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error creating delivery agent");
                return ApiResponse.CreateInternalServerError(ApiMessages.InternalServerError).ToActionResult();
            }
        }

        /// <summary>
        /// تحديث بيانات مندوب التوصيل
        /// </summary>
        [HttpPut]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateDeliveryAgent([FromBody] DeliveryAgentUpdateDTO updateDto)
        {
            using var activity = ActivitySourceProvider.StartActivity();

            if (!ModelState.IsValid)
                return ApiResponse.CreateBadRequest(ModelState).ToActionResult();

            try
            {
                await _deliveryAgentService.UpdateDeliveryAgentAsync(updateDto);
                return ApiResponse.CreateSuccess(ApiMessages.Updated).ToActionResult();
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Delivery agent not found: {Message}", ex.Message);
                return ApiResponse.CreateNotFound(ex.Message).ToActionResult();
            }
            catch (BusinessException ex)
            {
                _logger.LogWarning(ex, "Business rule violation: {Message}", ex.Message);
                return ApiResponse.CreateConflict(ex.Message).ToActionResult();
            }
            catch (ServiceException ex)
            {
                _logger.LogError(ex, "Service error while updating delivery agent");
                return ApiResponse.CreateInternalServerError(ApiMessages.InternalServerError).ToActionResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error updating delivery agent");
                return ApiResponse.CreateInternalServerError(ApiMessages.InternalServerError).ToActionResult();
            }
        }

        /// <summary>
        /// حذف مندوب التوصيل
        /// </summary>
        [HttpDelete("{deliveryAgentId:int}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteDeliveryAgent([Range(1, int.MaxValue)] int deliveryAgentId)
        {
            using var activity = ActivitySourceProvider.StartActivity();

            try
            {
                await _deliveryAgentService.DeleteDeliveryAgentAsync(deliveryAgentId);
                return ApiResponse.CreateSuccess(ApiMessages.Deleted).ToActionResult();
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Delivery agent not found: ID {DeliveryAgentId}", deliveryAgentId);
                return ApiResponse.CreateNotFound(ex.Message).ToActionResult();
            }
            catch (BusinessException ex)
            {
                _logger.LogWarning(ex, "Business rule violation: {Message}", ex.Message);
                return ApiResponse.CreateBadRequest(ex.Message).ToActionResult();
            }
            catch (ServiceException ex)
            {
                _logger.LogError(ex, "Service error while deleting delivery agent");
                return ApiResponse.CreateInternalServerError(ApiMessages.InternalServerError).ToActionResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error deleting delivery agent");
                return ApiResponse.CreateInternalServerError(ApiMessages.InternalServerError).ToActionResult();
            }
        }

        /// <summary>
        /// الحصول على مندوب توصيل بواسطة المعرف
        /// </summary>
        [HttpGet("{deliveryAgentId:int}")]
        [ProducesResponseType(typeof(ApiResponse<DeliveryAgentResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDeliveryAgentById([Range(1, int.MaxValue)] int deliveryAgentId)
        {
            using var activity = ActivitySourceProvider.StartActivity();

            try
            {
                var agent = await _deliveryAgentService.GetDeliveryAgentByIdAsync(deliveryAgentId);
                return ApiResponse<DeliveryAgentResponseDTO>
                    .CreateSuccess(agent, ApiMessages.Success)
                    .ToActionResult();
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Delivery agent not found: ID {DeliveryAgentId}", deliveryAgentId);
                return ApiResponse.CreateNotFound(ex.Message).ToActionResult();
            }
            catch (ServiceException ex)
            {
                _logger.LogError(ex, "Service error retrieving delivery agent");
                return ApiResponse.CreateInternalServerError(ApiMessages.InternalServerError).ToActionResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error retrieving delivery agent");
                return ApiResponse.CreateInternalServerError(ApiMessages.InternalServerError).ToActionResult();
            }
        }

        /// <summary>
        /// الحصول على جميع مندوبي التوصيل
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<DeliveryAgentResponseDTO>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllDeliveryAgents()
        {
            using var activity = ActivitySourceProvider.StartActivity();

            try
            {
                var agents = await _deliveryAgentService.GetAllDeliveryAgentsAsync();
                return ApiResponse<IEnumerable<DeliveryAgentResponseDTO>>
                    .CreateSuccess(agents, ApiMessages.Success)
                    .ToActionResult();
            }
            catch (ServiceException ex)
            {
                _logger.LogError(ex, "Service error retrieving all delivery agents");
                return ApiResponse.CreateInternalServerError(ApiMessages.InternalServerError).ToActionResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error retrieving all delivery agents");
                return ApiResponse.CreateInternalServerError(ApiMessages.InternalServerError).ToActionResult();
            }
        }

        // سيتم تفعيلها لاحقًا عند تفعيل الخدمات المعطلة
        /*
        [HttpGet("{deliveryAgentId:int}/card")]
        [ProducesResponseType(typeof(ApiResponse<DeliveryAgentCardDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDeliveryAgentCard([Range(1, int.MaxValue)] int deliveryAgentId)
        {
            // تنفيذ مشابه للوظائف الأخرى
        }

        [HttpGet("cards")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<DeliveryAgentCardDTO>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllDeliveryAgentCards()
        {
            // تنفيذ مشابه للوظائف الأخرى
        }
        */
    }
}
