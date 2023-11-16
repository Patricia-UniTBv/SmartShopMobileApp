using API.Models;
using API.Repository.Interfaces;
using AutoMapper;
using DTO;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class CartItemRepository: BaseRepository<CartItem>, ICartItemRepository
    {
        public CartItemRepository(SmartShopDBContext context, IMapper _mapper) : base(context, _mapper)
        {
        }
        public async Task AddCartItem(CartItemDTO item)
        {
            var result = _mapper.Map<CartItemDTO, CartItem>(item);
            await _dbSet.AddAsync(result);
            _context.SaveChanges();
        }
    }
}
