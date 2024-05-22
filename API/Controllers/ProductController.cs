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

        [HttpGet("GetAllProductsForSupermarket/{supermarketID}")]
        public async Task<IActionResult> GetAllProductsForSupermarket(int supermarketID)
        {
            try
            {
                var products = await _unitOfWork.ProductRepository.GetAllProductsForSupermarket(supermarketID);

                return Ok(products);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("GetProductById")]
        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                var result = await _unitOfWork.ProductRepository.GetProductById(id);

                return Ok(result);
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
        public async Task<IActionResult> AddProductToShoppingCart([FromBody] ProductDTO product,int productId, int numberOfProducts, int supermarketId, int userId)
        {
            try
            {
                var shoppingCart = await _unitOfWork.ShoppingCartRepository.GetLatestShoppingCartForCurrentUser(userId); 
                var currentUser = await _unitOfWork.UserRepository.GetUserByID(userId); 
                var product1 = await _unitOfWork.ProductRepository.GetProductByIdAndSupermarket(productId, supermarketId);

                if (shoppingCart == null)
                {
                    shoppingCart = new ShoppingCartDTO
                    {
                        UserID = currentUser.UserID,
                        CreationDate = DateTime.Now,
                        TotalAmount = 0,
                        IsTransacted = false,
                        SupermarketID = supermarketId
                    };

                    await _unitOfWork.ShoppingCartRepository.AddShoppingCart(shoppingCart);
                    shoppingCart = await _unitOfWork.ShoppingCartRepository.GetLatestShoppingCartForCurrentUser(currentUser.UserID);
                }

                var currentDate = DateTime.Now;
                var activeOffer = await _unitOfWork.OfferRepository.GetActiveOfferForProduct(productId, supermarketId, currentDate);

                decimal productPrice = product1.Price;
                if (activeOffer != null)
                {
                    productPrice -= productPrice * (decimal)activeOffer.OfferPercentage / 100;
                }
                product1.Price = productPrice;

                var cartItem = new CartItemDTO
                {
                    ProductID = product1.ProductID,
                    ShoppingCartID = shoppingCart.ShoppingCartID,
                    Quantity = numberOfProducts, 
                };

                shoppingCart.TotalAmount += productPrice * numberOfProducts;
                shoppingCart.SupermarketID = supermarketId;
                await _unitOfWork.ShoppingCartRepository.UpdateShoppingCart(shoppingCart);

                await _unitOfWork.CartItemRepository.AddCartItem(cartItem);

                return Ok(product1);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
       
    }
}
