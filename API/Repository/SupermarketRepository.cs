using API.Models;
using API.Repository.Interfaces;
using AutoMapper;
using DTO;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class SupermarketRepository : BaseRepository<Supermarket>, ISupermarketRepository
    {
        public SupermarketRepository(SmartShopDBContext context, IMapper _mapper) : base(context, _mapper)
        {
        }

        public async Task<ICollection<SupermarketDTO>> GetAllSupermarkets()
        {
            var result = await _dbSet.ToListAsync();
            return _mapper.Map<ICollection<Supermarket>, ICollection<SupermarketDTO>>(result);
        }

        public async Task<SupermarketDTO> GetSupermarketById(int supermarketId)
        {
            var supermarket = await _dbSet.Where(l => l.SupermarketID == supermarketId).FirstAsync();
            return _mapper.Map<Supermarket, SupermarketDTO>(supermarket);
        }
    }
}
