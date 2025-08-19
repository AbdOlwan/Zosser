using Shared.DTOs.ProductDTOs;

namespace BLL.Services.Interfaces.ProductInterfaces
{
    public interface IProductImageService
    {
        Task<ProductImageResponseDTO> AddImage(ProductImageCreateDTO imageDto);
        Task UpdateImage(ProductImageUpdateDTO imageDto);
        Task DeleteImage(int id);
        Task<ProductImageResponseDTO?> GetImageById(int id);
        Task<IEnumerable<ProductImageResponseDTO>> GetImagesByProductId(int productId);
    }
}
