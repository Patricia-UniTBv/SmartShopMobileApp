using API.Models;
using API.Repository.Interfaces;
using DTO;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class CartItemController: ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CartItemController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpDelete("DeleteCartItemFromShoppingCart")]
        public async Task<IActionResult> DeleteCartItemFromShoppingCart(int productId, int shoppingCartId, double quantity)
        {
            try
            {
                var cartItem = await _unitOfWork.CartItemRepository.GetCartItemByProductIdAndShoppingCartId(productId, shoppingCartId, quantity);
                _unitOfWork.CartItemRepository.DeleteCartItem(cartItem);

                var shoppingCart = await _unitOfWork.ShoppingCartRepository.GetLatestShoppingCartForCurrentUser(1); //provizoriu, UserID trebuie modificat dupa autentificare!
                var productToDelete = await _unitOfWork.ProductRepository.GetProductById(productId);
                shoppingCart.TotalAmount -= productToDelete.Price * quantity;
                await _unitOfWork.ShoppingCartRepository.UpdateShoppingCart(shoppingCart);
                
                await _unitOfWork.CompleteAsync();
                return Ok(cartItem);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
