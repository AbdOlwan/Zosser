using Shared.DTOs.FAQs;

namespace BLL.Services.Interfaces.FAQs
{
    public interface IBaseFAQService<TDto, TCreateDto, TUpdateDto>
        where TDto : BaseFAQReadDto
        where TCreateDto : BaseFAQCreateDto
        where TUpdateDto : BaseFAQUpdateDto
    {
        Task<IEnumerable<TDto>> GetAllAsync();
        Task<TDto> GetByIdAsync(int id);
        Task<TDto> AddAsync(TCreateDto dto);
        Task UpdateAsync(int id, TUpdateDto dto);
        Task DeleteAsync(int id);
    }
}
