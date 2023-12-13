using API.Repository.Interfaces;
using DTO;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ShoppingCartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork; 
        }
        [HttpGet("GetLatestShoppingCartByUserId")]
        public async Task<IActionResult> GetLatestShoppingCartByUserId(int id)
        {
            try
            {
                var latestShoppingCart = await _unitOfWork.ShoppingCartRepository.GetLatestShoppingCartForCurrentUser(id);
                return Ok(latestShoppingCart);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("GetLatestShoppingCartForCurrentUser")]
        public async Task<IActionResult> GetLatestShoppingCartItemsForCurrentUser(int id)
        {
            try
            {
                var latestShoppingCart = await _unitOfWork.ShoppingCartRepository.GetLatestShoppingCartForCurrentUser(id);
                var cartItems = await _unitOfWork.CartItemRepository.GetProductsByShoppingCart(latestShoppingCart.ShoppingCartID);
                return Ok(cartItems);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("UpdateShoppingCartWhenTransacted")]
        public async Task<IActionResult> UpdateShoppingCartWhenTransacted(int id)
        {
            try
            {
                var shoppingCart = await _unitOfWork.ShoppingCartRepository.GetShoppingCartById(id);
                shoppingCart.IsTransacted = true;
                await _unitOfWork.ShoppingCartRepository.UpdateShoppingCart(shoppingCart);
                return Ok(shoppingCart);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("UpdateShoppingCart")]
        public async Task<IActionResult> UpdateShoppingCart([FromBody] ShoppingCartDTO shoppingCart)
        {
            try
            {
                await _unitOfWork.ShoppingCartRepository.UpdateShoppingCart(shoppingCart);
                return Ok(shoppingCart);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
