using Shared.Constants;
using Shared.DTOs.ProductDTOs;

namespace BLL.Services.Interfaces.ProductInterfaces
{
    public interface IProductService
    {
        Task<ProductResponseDTO?> CreateProduct(ProductCreateDTO productDto);
        Task<ProductResponseDTO?> UpdateProduct(ProductUpdateDTO productDto);
        Task DeleteProduct(int id);
        Task<ProductResponseDTO?> GetProductById(int id);
        Task<PagedResponse<ProductCardDto>> GetProductCardsPaged(int pageNumber, int pageSize);
        Task<IEnumerable<ProductResponseDTO>> GetAllProducts();
    }
}
