using BLL.Services.Interfaces.FAQs;
using DAL.Entities.Models.FaqModels;
using DAL.Repositories.Interfaces;
using DAL.Repositories.Interfaces.FAQs;
using Mapster;
using Microsoft.Extensions.Logging;
using Shared.DTOs.FAQs;
using Shared.Utils;

namespace BLL.Services.Implementations.FAQs
{
    public abstract class BaseFAQService<TEntity, TReadDto, TCreateDto, TUpdateDto>
           : IBaseFAQService<TReadDto, TCreateDto, TUpdateDto>
           where TEntity : BaseFAQ
           where TReadDto : BaseFAQReadDto
           where TCreateDto : BaseFAQCreateDto
           where TUpdateDto : BaseFAQUpdateDto
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly ILogger _logger;
        protected readonly string _entityName;

        protected BaseFAQService(IUnitOfWork unitOfWork, ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _entityName = typeof(TEntity).Name;
        }

        public virtual async Task<IEnumerable<TReadDto>> GetAllAsync()
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                var entities = await GetRepo().GetAllAsync();
                return entities.Adapt<IEnumerable<TReadDto>>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all {Entity}s", _entityName);
                throw;
            }
        }

        public virtual async Task<TReadDto> GetByIdAsync(int id)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                var entity = await GetRepo().GetByIdAsync(id);
                if (entity == null)
                    throw new KeyNotFoundException($"{_entityName} with ID {id} not found.");

                return entity.Adapt<TReadDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting {Entity} by ID: {Id}", _entityName, id);
                throw;
            }
        }

        public virtual async Task<TReadDto> AddAsync(TCreateDto dto)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                var entity = dto.Adapt<TEntity>();
                var addedEntity = await GetRepo().AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                return addedEntity.Adapt<TReadDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding {Entity}", _entityName);
                throw;
            }
        }

        public virtual async Task UpdateAsync(int id, TUpdateDto dto)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                var entity = await GetRepo().GetByIdAsync(id);
                if (entity == null)
                    throw new KeyNotFoundException($"{_entityName} with ID {id} not found.");

                dto.Adapt(entity);
                await GetRepo().UpdateAsync(entity);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating {Entity} ID: {Id}", _entityName, id);
                throw;
            }
        }

        public virtual async Task DeleteAsync(int id)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                var entity = await GetRepo().GetByIdAsync(id);
                if (entity == null)
                    throw new KeyNotFoundException($"{_entityName} with ID {id} not found.");

                await GetRepo().DeleteAsync(id);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting {Entity} ID: {Id}", _entityName, id);
                throw;
            }
        }

        protected abstract IBaseFAQRepo<TEntity> GetRepo();
    }
}
