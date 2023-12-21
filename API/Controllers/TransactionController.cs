using API.Models;
using API.Repository.Interfaces;
using DTO;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    [Route("api/[controller]")]
    public class TransactionController: ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;

        public TransactionController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllTransactions")]
        public async Task<IActionResult> GetAllTransactions(int userId, int supermarketId)
        {
            try
            {
                var transactions = await _unitOfWork.TransactionRepository.GetAllTransactions(userId,supermarketId);

                return Ok(transactions);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("GetAllTransactionsWithDiscount/{userId}/{supermarketId}")]
        public async Task<IActionResult> GetAllTransactionsWithDiscount(int userId, int supermarketId)
        {
            try
            {
                var transactions = await _unitOfWork.TransactionRepository.GetAllTransactionsWithDiscount(userId, supermarketId);

                return Ok(transactions);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("GetTransactionBySupermarketId")]
        public async Task<IActionResult> GetTransactionBySupermarketId(int shoppingCartId)
        {
            try
            {
                var transaction = await _unitOfWork.TransactionRepository.GetTransactionBySupermarketId(shoppingCartId);

                return Ok(transaction);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("AddTransaction")]
        public async Task<IActionResult> AddTransaction([FromBody] TransactionDTO transaction)
        {
            try
            {
                await _unitOfWork.TransactionRepository.AddTransaction(transaction);

                return Ok(transaction);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //[HttpPut("UpdateVoucherDiscountForTransaction")]
        //public async Task<IActionResult> UpdateVoucherDiscountForTransaction()
        //{
        //    try
        //    {
        //        var lastTransaction = 
        //        transaction.VoucherDiscount = transaction.TotalAmount;
        //        _unitOfWork.TransactionRepository.UpdateVoucherDiscountForTransaction(transaction);

        //        return Ok(transaction);
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e.Message);
        //    }
        //}
    }
}
