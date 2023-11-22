using API.Repository.Interfaces;
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

        [HttpGet("GetLatestShoppingCartForCurrentUser")]
        public async Task<IActionResult> GetLatestShoppingCartForCurrentUser(int id)
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

        //[HttpGet("GetLatestShoppingCartForCurrentUser")] //PROVIZORIU
        //public async Task<IActionResult> GetLatestShoppingCartForCurrentUser()
        //{
        //    try
        //    {
        //        var latestShoppingCart = await _unitOfWork.ShoppingCartRepository.GetLatestShoppingCartForCurrentUser(1);
        //        var cartItems = await _unitOfWork.CartItemRepository.GetProductsByShoppingCart(latestShoppingCart.ShoppingCartID);
        //        return Ok(cartItems);
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e.Message);
        //    }
        //}
    }
}
