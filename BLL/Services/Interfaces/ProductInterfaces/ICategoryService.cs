using Shared.DTOs.ProductDTOs.General;

namespace BLL.Services.Interfaces.ProductInterfaces
{
    public interface ICategoryService
    {
        Task<CategoryResponseDTO> CreateCategoryAsync(CategoryCreateDTO categoryDto);
        Task<CategoryResponseDTO> UpdateCategoryAsync(CategoryUpdateDTO categoryDto);
        Task DeleteCategoryAsync(int id);
        Task<CategoryResponseDTO> GetCategoryByIdAsync(int id);
        Task<IEnumerable<CategoryResponseDTO>> GetAllCategoriesAsync();
    }
}
