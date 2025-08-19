using Shared.DTOs.FAQs;

namespace BLL.Services.Interfaces.FAQs
{
    public interface ISiteFAQService : IBaseFAQService<SiteFAQReadDto, SiteFAQCreateDto, SiteFAQUpdateDto>
    {
        Task<IEnumerable<BaseFAQSimpleDto>> GetSiteFAQsAsync();
    }
}
