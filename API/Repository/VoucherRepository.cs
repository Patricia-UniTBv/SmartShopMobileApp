using API.Models;
using API.Repository.Interfaces;
using AutoMapper;
using DTO;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class VoucherRepository: BaseRepository<Voucher>, IVoucherRepository
    {
        public VoucherRepository(SmartShopDBContext context, IMapper _mapper) : base(context, _mapper)
        {
        }

        public async Task<VoucherDTO> GetVoucherForUserAndSupermarket(int userId, int supermarketId)
        {
            var result = await _dbSet.Where(v =>v.UserID == userId && v.SupermarketID == supermarketId).FirstOrDefaultAsync();
            return _mapper.Map<Voucher, VoucherDTO>(result!);
        }

        public void UpdateVoucherForSpecificUser(VoucherDTO voucher)
        {
            var updatedVoucher = _mapper.Map<VoucherDTO, Voucher>(voucher);
            _context.ChangeTracker.Clear();
            _context.Update(updatedVoucher);
            _context.SaveChanges();
        }

        public async Task CreateVoucherForUserAndSupermarket(VoucherDTO newVoucher)
        {
            var result = _mapper.Map<VoucherDTO, Voucher>(newVoucher);
            await _dbSet.AddAsync(result);
            _context.SaveChanges();
        }



    }
}
