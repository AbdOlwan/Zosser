using Shared.DTOs.FAQs;

namespace BLL.Services.Interfaces.FAQs
{
    public interface IProductFAQService : IBaseFAQService<ProductFAQReadDto, ProductFAQCreateDto, ProductFAQUpdateDto>
    {
        Task<IEnumerable<ProductFAQSimpleDto>> GetByProductIdAsync(int productId);
        Task<FAQCollectionDto> GetGroupedFAQsAsync();
    }
}
