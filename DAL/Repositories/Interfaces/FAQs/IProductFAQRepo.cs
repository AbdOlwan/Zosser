using DAL.Entities.Models.FaqModels;

namespace DAL.Repositories.Interfaces.FAQs
{
    public interface IProductFAQRepo : IBaseFAQRepo<ProductFAQ>
    {
        Task<IEnumerable<ProductFAQ>> GetByProductIdAsync(int productId);
    }
}
