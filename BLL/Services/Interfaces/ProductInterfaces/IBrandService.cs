using Shared.DTOs.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces.ProductInterfaces
{
    public interface IBrandService
    {
        Task<BrandResponseDTO> CreateBrandAsync(BrandCreateDTO brandDto);
        Task<BrandResponseDTO> UpdateBrandAsync( BrandUpdateDTO brandDto);
        Task DeleteBrandAsync(int id);
        Task<BrandResponseDTO> GetBrandByIdAsync(int id);
        Task<IEnumerable<BrandResponseDTO>> GetAllBrandsAsync();
    }
}
