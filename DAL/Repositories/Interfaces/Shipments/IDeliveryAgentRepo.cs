using DAL.Entities.Models.Shipment;

namespace DAL.Repositories.Interfaces.Shipments
{
    public interface IDeliveryAgentRepo
    {
        Task<DeliveryAgent> CreateDeliveryAgentAsync(DeliveryAgent deliveryAgent, CancellationToken cancellationToken = default);
        Task UpdateDeliveryAgentAsync(DeliveryAgent deliveryAgent, CancellationToken cancellationToken = default);
        Task DeleteDeliveryAgentAsync(int deliveryAgentId, CancellationToken cancellationToken = default);
        Task<DeliveryAgent?> GetDeliveryAgentByIdAsync(int deliveryAgentId, bool track = false, CancellationToken cancellationToken = default);
        Task<IEnumerable<DeliveryAgent>> GetAllDeliveryAgentsAsync(CancellationToken cancellationToken = default);
        Task<bool> DeliveryAgentExistsAsync(int deliveryAgentId, CancellationToken cancellationToken = default);
        Task<DeliveryAgent?> GetDeliveryAgentByNameAsync(string name, bool track = false, CancellationToken cancellationToken = default);
        Task<IEnumerable<DeliveryAgent>> GetActiveDeliveryAgentsAsync(CancellationToken cancellationToken = default);
    }
}
