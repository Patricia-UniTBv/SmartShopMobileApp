using API.Models;
using API.Repository.Interfaces;
using AutoMapper;
using DTO;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class TransactionRepository: BaseRepository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(SmartShopDBContext context, IMapper _mapper) : base(context, _mapper)
        {
        }

        public async Task<ICollection<TransactionDTO>> GetAllTransactions()
        {
            var result = await _dbSet.ToListAsync();
            return _mapper.Map<ICollection<Transaction>, ICollection<TransactionDTO>>(result!);
        }

        public async Task<ICollection<TransactionDTO>> GetAllTransactionsWithDiscount(int userId, int supermarketId)
        {
            var result = await _dbSet
                               .Include(t => t.ShoppingCart)
                               .Where(t => t.VoucherDiscount != null
                                           && t.ShoppingCart.UserID == userId
                                           && t.ShoppingCart.SupermarketID == supermarketId)
                               .ToListAsync();
            return _mapper.Map<ICollection<Transaction>, ICollection<TransactionDTO>>(result!);
        }
        public async Task<TransactionDTO> GetTransactionBySupermarketId(int shoppingCartId)
        {
            var result = await _dbSet.Where(t => t.ShoppingCartID == shoppingCartId).FirstOrDefaultAsync();
            return _mapper.Map<Transaction, TransactionDTO>(result!);
        }

        public async Task AddTransaction(TransactionDTO transaction)
        {
            var result = _mapper.Map<TransactionDTO, Transaction>(transaction);
            await _dbSet.AddAsync(result);
            _context.SaveChanges();
        }

        public void UpdateVoucherDiscountForTransaction(TransactionDTO transaction)
        {
            var updatedField = _mapper.Map<TransactionDTO, Transaction>(transaction);
            _context.ChangeTracker.Clear();
            _context.Update(updatedField);
            _context.SaveChanges();
        }

    }
}
