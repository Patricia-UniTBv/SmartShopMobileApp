using API.Models;
using API.Repository.Interfaces;
using DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class VoucherController: ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public VoucherController(IUnitOfWork unitOfWork)
        {
            
            _unitOfWork = unitOfWork;
        }

        //[HttpPost("AddVoucherForSpecificUser")]
        //public async Task<IActionResult> AddVoucher(double earnedMoney)
        //{
        //    try
        //    {
        //        var newVoucher = new VoucherDTO
        //        {
        //            UserId = 1, // ID-ul utilizatorului pentru care creezi voucherul
        //            SupermarketId = 123,
        //            EarnedPoints = 50
        //        };

        //        _unitOfWork.VoucherRepository.AddVoucherForSpecificUser(double earnedMoney);
        //        return Ok(product);
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e.Message);
        //    }
        //}

    }
}
