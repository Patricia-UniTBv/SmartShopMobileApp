﻿using API.Models;
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

        [HttpPost("AddProductToShoppingCart/{numberOfProducts}/{supermarketId}")]
        public async Task<IActionResult> AddProductToShoppingCart( ProductDTO product, int numberOfProducts, int supermarketId)// SA PUN FROMBODY!
        {
            try
            {
                var shoppingCart = await _unitOfWork.ShoppingCartRepository.GetLatestShoppingCartForCurrentUser(1); //provizoriu, UserID trebuie modificat dupa autentificare!
                var currentUser = await _unitOfWork.UserRepository.GetUserByID(1); //provizoriu

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
                var cartItem = new CartItemDTO
                {
                    ProductID = product.ProductId,
                    ShoppingCartID = shoppingCart.ShoppingCartID,
                    Quantity = numberOfProducts, 
                };

                var productToAdd = await _unitOfWork.ProductRepository.GetProductById(product.ProductId);
                shoppingCart.TotalAmount += productToAdd.Price * numberOfProducts;
                shoppingCart.SupermarketID = supermarketId;
                await _unitOfWork.ShoppingCartRepository.UpdateShoppingCart(shoppingCart);

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
