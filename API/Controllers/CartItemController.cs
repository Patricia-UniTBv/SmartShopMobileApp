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

        [HttpGet("GetItemsForShoppingCart")]
        public async Task<IActionResult> GetItemsForShoppingCart(int shoppingCartId)
        {
            try
            {
                var cartItems = await _unitOfWork.CartItemRepository.GetProductsByShoppingCart(shoppingCartId);
             
                return Ok(cartItems);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("DeleteCartItemFromShoppingCart")]
        public async Task<IActionResult> DeleteCartItemFromShoppingCart(int productId, int shoppingCartId, double quantity, int userId)
        {
            try
            {
                var cartItem = await _unitOfWork.CartItemRepository.GetCartItemByProductIdAndShoppingCartId(productId, shoppingCartId, quantity);
                _unitOfWork.CartItemRepository.DeleteCartItem(cartItem);

                var shoppingCart = await _unitOfWork.ShoppingCartRepository.GetLatestShoppingCartForCurrentUser(userId);

                var productToDelete = await _unitOfWork.ProductRepository.GetProductById(productId);

                int supermarketId = shoppingCart.SupermarketID!.Value;
                var currentDate = DateTime.Now;
                var activeOffer = await _unitOfWork.OfferRepository.GetActiveOfferForProduct(productId, supermarketId, currentDate);

                decimal productPrice = productToDelete.Price;
                if (activeOffer != null)
                {
                    productPrice -= productPrice * (decimal)activeOffer.OfferPercentage / 100;
                }
                decimal totalPrice = productPrice * (decimal)quantity;

                shoppingCart.TotalAmount -= totalPrice;
                await _unitOfWork.ShoppingCartRepository.UpdateShoppingCart(shoppingCart);

                await _unitOfWork.CompleteAsync();
                return Ok(cartItem);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //public async Task<IActionResult> DeleteCartItemFromShoppingCart(int productId, int shoppingCartId, double quantity, int userId)
        //{
        //    try
        //    {
        //        var productToDelete = await _unitOfWork.ProductRepository.GetProductById(productId);

        //        var cartItem = await _unitOfWork.CartItemRepository.GetCartItemByProductIdAndShoppingCartId(productId, shoppingCartId, quantity);

        //        var shoppingCart = await _unitOfWork.ShoppingCartRepository.GetLatestShoppingCartForCurrentUser(userId);
        //        int supermarketId = shoppingCart.SupermarketID!.Value;

        //        var currentDate = DateTime.Now;
        //        var activeOffer = await _unitOfWork.OfferRepository.GetActiveOfferForProduct(productId, supermarketId, currentDate);

        //        decimal productPrice = productToDelete.Price;
        //        if (activeOffer != null)
        //        {
        //            productPrice -= productPrice * (decimal)activeOffer.OfferPercentage / 100;
        //        }

        //        _unitOfWork.CartItemRepository.DeleteCartItem(cartItem);

        //        shoppingCart.TotalAmount -= productPrice * (decimal)quantity;
        //        await _unitOfWork.ShoppingCartRepository.UpdateShoppingCart(shoppingCart);

        //        await _unitOfWork.CompleteAsync();
        //        return Ok(cartItem);
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e.Message);
        //    }
        //}

    }
}
