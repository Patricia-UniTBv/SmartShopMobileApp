﻿using API.Repository.Interfaces;
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

        [HttpGet("GetAllTransactedShoppingCartsByUserId")]
        public async Task<IActionResult> GetAllTransactedShoppingCartsByUserId(int id, int supermarketId)
        {
            try
            {
                var shoppingCarts = await _unitOfWork.ShoppingCartRepository.GetAllTransactedShoppingCartsByUserId(id, supermarketId);
                return Ok(shoppingCarts);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("GetAllTransactedShoppingCartsWithSupermarketByUserId")]
        public async Task<IActionResult> GetAllTransactedShoppingCartsWithSupermarketByUserId(int id)
        {
            try
            {
                var shoppingCarts = await _unitOfWork.ShoppingCartRepository.GetAllTransactedShoppingCartsWithSupermarketByUserId(id);
                return Ok(shoppingCarts);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("GetLatestShoppingCartByUserIdAndSupermarketId")]
        public async Task<IActionResult> GetLatestShoppingCartByUserId(int id, int supermarketId)
        {
            try
            {
                var latestShoppingCart = await _unitOfWork.ShoppingCartRepository.GetLatestShoppingCartForCurrentUserAndSupermarket(id, supermarketId);
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
                if (latestShoppingCart != null)
                {
                    var cartItems = await _unitOfWork.CartItemRepository.GetProductsByShoppingCart(latestShoppingCart.ShoppingCartID);
                    return Ok(cartItems);

                }
                else return Ok(null);

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
