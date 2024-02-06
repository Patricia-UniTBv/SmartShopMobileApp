using API.Models;
using API.Repository.Interfaces;
using AutoMapper;

namespace API.Repository
{
    public class CreditCardRepository : BaseRepository<CreditCard>, ICreditCardRepository
    {
        public CreditCardRepository(SmartShopDBContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
