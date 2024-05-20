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

                decimal earnedMoney = 0;
                if (totalAmount >= 150)
                {
                    earnedMoney = Math.Round(totalAmount/2000, 2);
                }
                existingVoucher.EarnedPoints += earnedMoney;

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

        [HttpPost("CreateVoucherForUserAndSupermarket")]
        public async Task<IActionResult> CreateVoucherForUser(int userId, int supermarketId)
        {
            try
            {
                VoucherDTO newVoucher = new()
                {
                    UserID = userId,
                    SupermarketID = supermarketId,
                    EarnedPoints = 0
                };

                await _unitOfWork.VoucherRepository.CreateVoucherForUserAndSupermarket(newVoucher);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
