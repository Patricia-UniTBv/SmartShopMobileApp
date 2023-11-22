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
        public async Task<IActionResult> DeleteLeaveRequestById(int productId, int shoppingCartId)
        {
            try
            {
                var cartItem = await _unitOfWork.CartItemRepository.GetCartItemByProductIdAndShoppingCartId(productId, shoppingCartId);
                _unitOfWork.CartItemRepository.DeleteCartItem(cartItem);

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
