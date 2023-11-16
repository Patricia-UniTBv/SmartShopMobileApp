using AutoMapper;
using API.Models;
using API.Repository.Interfaces;
using DTO;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace API.Repository
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(SmartShopDBContext context, IMapper _mapper) : base(context, _mapper)
        {
        }
        public async Task<ICollection<ProductDTO>> GetAllProducts()
        {
            var product = await _dbSet.ToListAsync();
            return _mapper.Map<ICollection<Product>, ICollection<ProductDTO>>(product);
        }

        public async Task<ProductDTO> GetProductByBarcode(string barcode)
        {
            var product = await _dbSet.Where(p => p.Barcode == barcode).FirstAsync();
            return _mapper.Map<Product, ProductDTO>(product);
        }

        public async Task<ProductDTO> AddProductToShoppingCart(ProductDTO product)
        {
            //provizoriu - sa modific dupa autentificare
            var userId = 1;
            var currentUser = new UserDTO() { UserID = userId };

            //var product = _mapper.Map<ProductDTO, Product>(productDTO);

            var cart =  _context.ShoppingCarts?.FirstOrDefault(cart => cart.UserID == currentUser.UserID);
            var shoppingCart = _mapper.Map<ShoppingCart, ShoppingCartDTO>(cart);

            if (shoppingCart == null)
            {
                shoppingCart = new ShoppingCartDTO
                {
                    UserID = currentUser.UserID,
                    CreationDate = DateTime.Now,
                    TotalAmount = 1, // va fi modificat in functie de numarul de produse cumparate
                    IsTransacted = false,
                };

                _context.ShoppingCarts?.Add(_mapper.Map<ShoppingCartDTO, ShoppingCart>(shoppingCart));

                //_context.ChangeTracker.Clear();
                _context.SaveChanges();
            }

            //var cartItem = new CartItemDTO
            //{
            //    ProductID = product.ProductId,
            //    ShoppingCartID = shoppingCart.ShoppingCartID,
            //    Quantity = 1, // va fi modificat in functie de numarul de produse cumparate
            //};

            //_context.CartItems.Add(_mapper.Map<CartItemDTO, CartItem>(cartItem));
            //_context.SaveChanges();

            return product;
        }
    }
}
