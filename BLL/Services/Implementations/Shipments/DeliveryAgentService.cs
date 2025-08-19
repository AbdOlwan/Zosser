using BLL.Exceptions;
using BLL.Services.Interfaces.Shipments;
using DAL.Entities.Models.Shipment;
using DAL.Repositories.Interfaces;
using Mapster;
using Microsoft.Extensions.Logging;
using Shared.DTOs.Shipments;
using Shared.Interfaces;
using Shared.Utils;

namespace BLL.Services.Implementations.Shipments
{
    public class DeliveryAgentService : IDeliveryAgentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeliveryAgentService> _logger;
        private readonly ICurrentUserService _currentUserService;

        public DeliveryAgentService(
            IUnitOfWork unitOfWork,
            ILogger<DeliveryAgentService> logger,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _currentUserService = currentUserService;
        }

        public async Task<DeliveryAgentResponseDTO> CreateDeliveryAgentAsync(DeliveryAgentCreateDTO createDto, CancellationToken cancellationToken = default)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            _logger.LogInformation("Creating new delivery agent: {Name}", createDto.Name);

            // التحقق من عدم وجود اسم مكرر
            var existingAgent = await _unitOfWork.DeliveryAgents.GetDeliveryAgentByNameAsync(createDto.Name, false, cancellationToken);
            if (existingAgent != null)
            {
                throw new BusinessException($"Delivery agent with name '{createDto.Name}' already exists.");
            }

            var deliveryAgent = createDto.Adapt<DeliveryAgent>();
            deliveryAgent.CreatedAt = DateTime.UtcNow;
            deliveryAgent.CreatedBy = _currentUserService.GetCurrentUserId();
            deliveryAgent.LastModifiedAt = DateTime.UtcNow;
            deliveryAgent.LastModifiedBy = _currentUserService.GetCurrentUserId();

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var createdAgent = await _unitOfWork.DeliveryAgents.CreateDeliveryAgentAsync(deliveryAgent, cancellationToken);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                _logger.LogInformation("Successfully created delivery agent ID: {Id}", createdAgent.Id);
                return createdAgent.Adapt<DeliveryAgentResponseDTO>();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Failed to create delivery agent: {Name}", createDto.Name);
                throw new ServiceException("An error occurred while creating delivery agent", ex);
            }
        }

        public async Task UpdateDeliveryAgentAsync(DeliveryAgentUpdateDTO updateDto, CancellationToken cancellationToken = default)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            _logger.LogInformation("Updating delivery agent ID: {Id}", updateDto.Id);

            var deliveryAgent = await _unitOfWork.DeliveryAgents.GetDeliveryAgentByIdAsync(updateDto.Id, true, cancellationToken);
            if (deliveryAgent == null)
            {
                throw new NotFoundException($"Delivery agent with ID {updateDto.Id} not found.");
            }

            // التحقق من عدم وجود اسم مكرر
            if (!string.IsNullOrEmpty(updateDto.Name) && updateDto.Name != deliveryAgent.Name)
            {
                var existingAgent = await _unitOfWork.DeliveryAgents.GetDeliveryAgentByNameAsync(updateDto.Name, false, cancellationToken);
                if (existingAgent != null)
                {
                    throw new BusinessException($"Delivery agent with name '{updateDto.Name}' already exists.");
                }
            }

            // تحديث الخصائص
            if (!string.IsNullOrEmpty(updateDto.Name))
            {
                deliveryAgent.Name = updateDto.Name;
            }

            deliveryAgent.PhoneNumber = updateDto.PhoneNumber ?? deliveryAgent.PhoneNumber;
            deliveryAgent.Email = updateDto.Email ?? deliveryAgent.Email;
            deliveryAgent.IsActive = updateDto.IsActive ?? deliveryAgent.IsActive;
            deliveryAgent.LastModifiedAt = DateTime.UtcNow;
            deliveryAgent.LastModifiedBy = _currentUserService.GetCurrentUserId();

            try
            {
                await _unitOfWork.DeliveryAgents.UpdateDeliveryAgentAsync(deliveryAgent, cancellationToken);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update delivery agent ID: {Id}", updateDto.Id);
                throw new ServiceException("An error occurred while updating delivery agent", ex);
            }
        }

        public async Task DeleteDeliveryAgentAsync(int deliveryAgentId, CancellationToken cancellationToken = default)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            _logger.LogInformation("Deleting delivery agent ID: {Id}", deliveryAgentId);

            var deliveryAgent = await _unitOfWork.DeliveryAgents.GetDeliveryAgentByIdAsync(deliveryAgentId, true, cancellationToken);
            if (deliveryAgent == null)
            {
                throw new NotFoundException($"Delivery agent with ID {deliveryAgentId} not found.");
            }

            // التحقق من عدم وجود تحصيلات مرتبطة
            if (deliveryAgent.PaymentCollections?.Any() == true)
            {
                throw new BusinessException($"Cannot delete delivery agent with active payment collections.");
            }

            try
            {
                await _unitOfWork.DeliveryAgents.DeleteDeliveryAgentAsync(deliveryAgentId, cancellationToken);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete delivery agent ID: {Id}", deliveryAgentId);
                throw new ServiceException("An error occurred while deleting delivery agent", ex);
            }
        }

        public async Task<DeliveryAgentResponseDTO> GetDeliveryAgentByIdAsync(int deliveryAgentId, CancellationToken cancellationToken = default)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            _logger.LogInformation("Retrieving delivery agent ID: {Id}", deliveryAgentId);

            var deliveryAgent = await _unitOfWork.DeliveryAgents.GetDeliveryAgentByIdAsync(deliveryAgentId, false, cancellationToken);
            if (deliveryAgent == null)
            {
                throw new NotFoundException($"Delivery agent with ID {deliveryAgentId} not found.");
            }

            return deliveryAgent.Adapt<DeliveryAgentResponseDTO>();
        }

        public async Task<IEnumerable<DeliveryAgentResponseDTO>> GetAllDeliveryAgentsAsync(CancellationToken cancellationToken = default)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            _logger.LogInformation("Retrieving all delivery agents");

            var deliveryAgents = await _unitOfWork.DeliveryAgents.GetAllDeliveryAgentsAsync(cancellationToken);
            return deliveryAgents.Adapt<IEnumerable<DeliveryAgentResponseDTO>>();
        }

        //public async Task<DeliveryAgentCardDTO> GetDeliveryAgentCardAsync(int deliveryAgentId, CancellationToken cancellationToken = default)
        //{
        //    using var activity = ActivitySourceProvider.StartActivity();
        //    _logger.LogInformation("Retrieving delivery agent card ID: {Id}", deliveryAgentId);

        //    var deliveryAgent = await _unitOfWork.DeliveryAgents.GetDeliveryAgentByIdAsync(deliveryAgentId, false, cancellationToken);
        //    if (deliveryAgent == null)
        //    {
        //        throw new NotFoundException($"Delivery agent with ID {deliveryAgentId} not found.");
        //    }

        //    // جلب التحصيلات غير المؤكدة
        //    var unverifiedCollections = await _unitOfWork.PaymentCollections.GetCollectionsByVerificationStatusAsync(deliveryAgentId, cancellationToken);

        //    return new DeliveryAgentCardDTO
        //    {
        //        Id = deliveryAgent.Id,
        //        Name = deliveryAgent.Name,
        //        PhoneNumber = deliveryAgent.PhoneNumber,
        //        IsActive = deliveryAgent.IsActive,
        //        ActiveCollections = unverifiedCollections.Count(),
        //        TotalCollections = deliveryAgent.PaymentCollections?.Count ?? 0
        //    };
        //}

        //public async Task<IEnumerable<DeliveryAgentCardDTO>> GetAllDeliveryAgentCardsAsync(CancellationToken cancellationToken = default)
        //{
        //    using var activity = ActivitySourceProvider.StartActivity();
        //    _logger.LogInformation("Retrieving all delivery agent cards");

        //    var deliveryAgents = await _unitOfWork.DeliveryAgents.GetAllDeliveryAgentsAsync(cancellationToken);
        //    var cards = new List<DeliveryAgentCardDTO>();

        //    foreach (var agent in deliveryAgents)
        //    {
        //        var unverifiedCollections = await _unitOfWork.PaymentCollections.GetUnverifiedCollectionsByAgentAsync(agent.Id, cancellationToken);

        //        cards.Add(new DeliveryAgentCardDTO
        //        {
        //            Id = agent.Id,
        //            Name = agent.Name,
        //            PhoneNumber = agent.PhoneNumber,
        //            IsActive = agent.IsActive,
        //            ActiveCollections = unverifiedCollections.Count(),
        //            TotalCollections = agent.PaymentCollections?.Count ?? 0
        //        });
        //    }

        //    return cards;
        //}
    }
}
