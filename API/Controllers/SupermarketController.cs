using API.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class SupermarketController: ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public SupermarketController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllSupermarkets")]
        public async Task<IActionResult> GetAllSupermarkets()
        {
            try
            {
                var products = await _unitOfWork.SupermarketRepository.GetAllSupermarkets();

                return Ok(products);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
