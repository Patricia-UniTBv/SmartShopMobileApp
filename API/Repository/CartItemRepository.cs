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
        public async Task<ICollection<ProductDTO>> GetProductsByShoppingCart(int shoppingCartId)//sa modific sa ia tot obiectul Product dupa ID din CartItems
        {
            var result = await _dbSet
                .Where(item => item.ShoppingCartID == shoppingCartId)
                .Select(item => new
                {
                    Product = item.Product,
                    Quantity = item.Quantity
                })
                .ToListAsync();

            var productDTOs = result.Select(item => new ProductDTO
            {
                ProductId = item.Product.ProductID,
                Quantity = item.Quantity,
                Name = item.Product.Name,

            }).ToList();

            return productDTOs;
        }
        public async Task AddCartItem(CartItemDTO item)
        {
            var result = _mapper.Map<CartItemDTO, CartItem>(item);
            await _dbSet.AddAsync(result);
            _context.SaveChanges();
        }

        public void DeleteCartItem(CartItemDTO item)
        {
            var cartItem = _mapper.Map<CartItemDTO, CartItem>(item);
            _context.ChangeTracker.Clear();
            _context.Remove(cartItem);
            _context.SaveChanges();
        }

        public async Task<CartItemDTO> GetCartItemById(int id)
        {
            var cart = await _dbSet.SingleAsync(c => c.CartItemID == id);
            return _mapper.Map<CartItem, CartItemDTO>(cart);
        }

        public async Task<CartItemDTO> GetCartItemByProductIdAndShoppingCartId(int productId, int shoppingCartId)
        {
            var carts = await _dbSet.Where(item => item.ProductID == productId && item.ShoppingCartID == shoppingCartId).ToListAsync();
            var cart = carts.FirstOrDefault();
            return _mapper.Map<CartItem, CartItemDTO>(cart!);
        }

    }
}
