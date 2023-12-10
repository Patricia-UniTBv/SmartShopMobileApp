using API.Models;
using API.Repository.Interfaces;
using AutoMapper;
using DTO;

namespace API.Repository
{
    public class VoucherRepository: BaseRepository<Voucher>, IVoucherRepository
    {
        public VoucherRepository(SmartShopDBContext context, IMapper _mapper) : base(context, _mapper)
        {
        }

        public async Task AddVoucherForSpecificUser(double earnedMoney)
        {

            //var result = _mapper.Map <, ShoppingCart> (shoppingCart);
            //await _dbSet.AddAsync(result);
            //_context.SaveChanges();
        }
    }
}
