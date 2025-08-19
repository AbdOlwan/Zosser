using Shared.DTOs.Shipments;

namespace BLL.Services.Interfaces.Shipments
{
    public interface IDeliveryAgentService
    {
        Task<DeliveryAgentResponseDTO> CreateDeliveryAgentAsync(DeliveryAgentCreateDTO createDto, CancellationToken cancellationToken = default);
        Task UpdateDeliveryAgentAsync(DeliveryAgentUpdateDTO updateDto, CancellationToken cancellationToken = default);
        Task DeleteDeliveryAgentAsync(int deliveryAgentId, CancellationToken cancellationToken = default);
        Task<DeliveryAgentResponseDTO> GetDeliveryAgentByIdAsync(int deliveryAgentId, CancellationToken cancellationToken = default);
        Task<IEnumerable<DeliveryAgentResponseDTO>> GetAllDeliveryAgentsAsync(CancellationToken cancellationToken = default);
        //Task<DeliveryAgentCardDTO> GetDeliveryAgentCardAsync(int deliveryAgentId, CancellationToken cancellationToken = default);
        //Task<IEnumerable<DeliveryAgentCardDTO>> GetAllDeliveryAgentCardsAsync(CancellationToken cancellationToken = default);
    }
}
