using API.Models;
using API.Repository.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly SmartShopDBContext _context;
        protected readonly DbSet<T> _dbSet;
        protected readonly IMapper _mapper;

        public BaseRepository(SmartShopDBContext context, IMapper mapper)
        {
            _context = context;
            _dbSet = context.Set<T>();
            _mapper = mapper;
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<ICollection<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public Task UpdateAsync(T entity)
        {
            _context.Update(entity);
            return Task.CompletedTask;
        }

        public Task UpdateRangeAsync(IEnumerable<T> entities)
        {
            _context.UpdateRange(entities);
            return Task.CompletedTask;
        }
        public Task DeleteRangeAsync(IEnumerable<T> entities)
        {
            _context.RemoveRange(entities);
            return Task.CompletedTask;
        }
    }
}
