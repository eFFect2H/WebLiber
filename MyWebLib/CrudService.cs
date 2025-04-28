using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebLib
{
    public interface ICrudService<TEntity, TDto, TKey>
    {
        Task<IEnumerable<TDto>> GetAllAsync();
        Task<TDto?> GetByIdAsync(TKey id);
        Task<TDto> CreateAsync(TDto dto);
        Task<TDto> UpdateAsync(TKey id, TDto dto);
        Task DeleteAsync(TKey id);
        Task<bool> ExistsAsync(TKey id);
    }
    /// <summary>
    /// Базовая реализация CRUD-сервиса с AutoMapper и репозиторием.
    /// </summary>
    /// <typeparam name="TEntity">Тип сущности</typeparam>
    /// <typeparam name="TDto">Тип DTO</typeparam>
    /// <typeparam name="TKey">Тип первичного ключа</typeparam>
    public class CrudService<TEntity, TDto, TKey> : ICrudService<TEntity, TDto, TKey>, ICrudQueryable<TDto>
    where TEntity : class
    {
        private readonly IRepository<TEntity, TKey> _repository;
        private readonly IMapper _mapper;

        public CrudService(IRepository<TEntity, TKey> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<TDto>>(entities);
        }

        public async Task<TDto?> GetByIdAsync(TKey id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity == null ? default : _mapper.Map<TDto>(entity);
        }

        public async Task<TDto> CreateAsync(TDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);
            await _repository.AddAsync(entity);
            return _mapper.Map<TDto>(entity);
        }

        public async Task<TDto> UpdateAsync(TKey id, TDto dto)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException("Entity not found");

            _mapper.Map(dto, entity);
            await _repository.UpdateAsync(entity);
            return _mapper.Map<TDto>(entity);
        }

        public async Task DeleteAsync(TKey id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException("Entity not found");

            await _repository.DeleteAsync(entity);
        }

        public Task<bool> ExistsAsync(TKey id) => _repository.ExistsAsync(id);

        /// <summary>
        /// Запрос с поддержкой фильтрации, пагинации и сортировки
        /// </summary>
        public async Task<IEnumerable<TDto>> QueryAsync(QueryFilter filter)
        {
            var query = _repository.Query().ApplyQuery(filter);
            var result = await query.ToListAsync();
            return _mapper.Map<IEnumerable<TDto>>(result);
        }
    }

}
