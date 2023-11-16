using API.Models;
using API.Repository.Interfaces;
using DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class ProductController: ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllProducts")]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var products = await _unitOfWork.ProductRepository.GetAllProducts();

                return Ok(products);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("GetProductByBarcode/{barcode}")]
        public async Task<IActionResult> GetProductByBarcode(string barcode)
        {
            try
            {
                var result = await _unitOfWork.ProductRepository.GetProductByBarcode(barcode);

                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("AddProductToShoppingCart")]
        public async Task<IActionResult> AddProductToShoppingCart([FromBody] ProductDTO product)
        {
            try
            {
                var shoppingCart = await _unitOfWork.ShoppingCartRepository.GetShoppingCartForSpecificUser(1); //provizoriu, UserID trebuie modificat dupa autentificare!
                var currentUser = await _unitOfWork.UserRepository.GetUserByID(1);

                if (shoppingCart == null)
                {
                    shoppingCart = new ShoppingCartDTO
                    {
                        UserID = currentUser.UserID,
                        CreationDate = DateTime.Now,
                        TotalAmount = 1, // va fi modificat in functie de numarul de produse cumparate
                        IsTransacted = false,
                    };

                    await _unitOfWork.ShoppingCartRepository.AddShoppingCart(shoppingCart);
                }
                var cartItem = new CartItemDTO
                {
                    ProductID = product.ProductId,
                    ShoppingCartID = shoppingCart.ShoppingCartID,
                    Quantity = 1, // va fi modificat in functie de numarul de produse cumparate
                };

                await _unitOfWork.CartItemRepository.AddCartItem(cartItem);

                return Ok(product);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
