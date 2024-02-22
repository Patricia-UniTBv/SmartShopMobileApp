using API.Models;
using API.Repository.Interfaces;
using AutoMapper;
using DTO;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class LocationRepository : BaseRepository<Location>, ILocationRepository
    {
        public LocationRepository(SmartShopDBContext context, IMapper _mapper) : base(context, _mapper)
        {
        }
        public async Task<ICollection<LocationDTO>> GetLocationsForSupermarketId(int supermarketId)
        {
            var locations = await _dbSet.Where(l => l.SupermarketID == supermarketId).ToListAsync();
            return _mapper.Map<ICollection<Location>, ICollection<LocationDTO>>(locations);
        }

    }
}
