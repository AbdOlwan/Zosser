using DAL.Entities.Models.FaqModels;

namespace DAL.Repositories.Interfaces.FAQs
{
    public interface IBaseFAQRepo<T> where T : BaseFAQ
    {
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
    }
}
