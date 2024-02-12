using API.Models;
using API.Repository.Interfaces;
using AutoMapper;
using DTO;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class CategoryRepository :BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(SmartShopDBContext context, IMapper _mapper) : base(context, _mapper)
        {
        }
        public async Task<ICollection<CategoryDTO>> GetAllCategories()
        {
            var result = await _dbSet.ToListAsync();
            return _mapper.Map<ICollection<Category>, ICollection<CategoryDTO>>(result);
        }
    }
}
