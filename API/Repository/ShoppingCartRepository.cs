using API.Models;
using API.Repository.Interfaces;
using AutoMapper;
using DTO;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class ShoppingCartRepository : BaseRepository<ShoppingCart>, IShoppingCartRepository
    {
        public ShoppingCartRepository(SmartShopDBContext context, IMapper _mapper) : base(context, _mapper)
        {
        }
        public async Task<ShoppingCartDTO?> GetShoppingCartForSpecificUser(int userID)
        {
            var result = await _dbSet.FirstOrDefaultAsync(cart => cart.UserID == userID);
            if (result == null)
            {
                return null;
            }
            return _mapper.Map<ShoppingCart, ShoppingCartDTO>(result);
        }
        public async Task<ShoppingCartDTO> GetLatestShoppingCartForCurrentUser(int userID)
        {
            var result = await _dbSet.Where(cart=> cart.UserID == userID && cart.IsTransacted == false).OrderByDescending(cart => cart.CreationDate).FirstOrDefaultAsync();
            if (result != null)
                return _mapper.Map<ShoppingCart, ShoppingCartDTO>(result!);
            else return new ShoppingCartDTO();
        }

        public async Task<ShoppingCartDTO> GetShoppingCartById(int id)
        {
            var result = await _dbSet.Where(cart => cart.ShoppingCartID == id).FirstOrDefaultAsync();
            if (result != null)
                return _mapper.Map<ShoppingCart, ShoppingCartDTO>(result!);
            else return new ShoppingCartDTO();
        }

        public async Task AddShoppingCart(ShoppingCartDTO shoppingCart)
        {
            var result = _mapper.Map<ShoppingCartDTO, ShoppingCart>(shoppingCart);
            await _dbSet.AddAsync(result);
            _context.SaveChanges();
        }

        public async Task UpdateShoppingCart(ShoppingCartDTO shoppingCart)
        {
            var cart = _mapper.Map<ShoppingCartDTO, ShoppingCart>(shoppingCart);
            _context.ChangeTracker.Clear();
            _context.Update(cart);
            _context.SaveChanges();
        }
    }
}
