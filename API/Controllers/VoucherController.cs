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


        [HttpGet("GetVoucherForUserAndSupermarket/{userId}/{supermarketId}")]
        public async Task<IActionResult> GetVoucherForUserAndSupermarket(int userId, int supermarketId)
        {
            try
            {
                var existingVoucher = await _unitOfWork.VoucherRepository.GetVoucherForUserAndSupermarket(userId, supermarketId);

                return Ok(existingVoucher);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("UpdateVoucherForSpecificUser/{userId}/{supermarketId}/{totalAmount}")]
        public async Task<IActionResult> UpdateVoucher(int userId, int supermarketId, decimal totalAmount)
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
                    decimal earnedMoney = 0.05m * totalAmount;
                    earnedMoneyRounded = (int)Math.Round(earnedMoney);
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

        [HttpPut("UpdateVoucherForSpecificUserWhenItIsUsed/{userId}/{supermarketId}")]
        public async Task<IActionResult> UpdateVoucherWhenItIsUsed(int userId, int supermarketId)
        {
            try
            {
                var existingVoucher = await _unitOfWork.VoucherRepository.GetVoucherForUserAndSupermarket(userId, supermarketId);

                if (existingVoucher == null)
                {
                    return NotFound("Voucher not found");
                }

                existingVoucher.EarnedPoints = 0;

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
