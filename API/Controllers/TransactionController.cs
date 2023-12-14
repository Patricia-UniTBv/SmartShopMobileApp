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
    }
}
