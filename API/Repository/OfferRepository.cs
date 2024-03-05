using API.Models;
using API.Repository.Interfaces;
using AutoMapper;
using DTO;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class OfferRepository :BaseRepository<Offer>, IOfferRepository
    {
        public OfferRepository(SmartShopDBContext context, IMapper _mapper) : base(context, _mapper)
        {
        }
        public async Task<ICollection<OfferDTO>> GetAllCurrentOffers()
        {
            var currentDate = DateTime.Now;

            var result = await _dbSet
                .Where(o => o.OfferStartDate <= currentDate && o.OfferEndDate >= currentDate)
                .ToListAsync();

            return _mapper.Map<ICollection<Offer>, ICollection<OfferDTO>>(result);
        }

    }
}
