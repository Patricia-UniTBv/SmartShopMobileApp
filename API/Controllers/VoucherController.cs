using API.Models;
using API.Repository.Interfaces;
using DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class VoucherController: ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public VoucherController(IUnitOfWork unitOfWork)
        {
            
            _unitOfWork = unitOfWork;
        }

        [HttpPut("UpdateVoucherForSpecificUser/{userId}/{supermarketId}/{totalAmount}")]
        public async Task<IActionResult> UpdateVoucher(int userId, int supermarketId, double totalAmount)
        {
            try
            {
                var existingVoucher = await _unitOfWork.VoucherRepository.GetVoucherForUserAndSupermarket(userId, supermarketId);

                if (existingVoucher == null)
                {
                    return NotFound("Voucher not found");
                }

                int earnedMoneyRounded = 0;
                if (totalAmount >= 150)
                {
                    double earnedMoneyDouble = 0.05 * totalAmount;
                    earnedMoneyRounded = (int)Math.Round(earnedMoneyDouble);
                }
                existingVoucher.EarnedPoints += earnedMoneyRounded;

                _unitOfWork.VoucherRepository.UpdateVoucherForSpecificUser(existingVoucher);

                return Ok(existingVoucher);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
