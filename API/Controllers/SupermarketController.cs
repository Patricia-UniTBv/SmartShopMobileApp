using API.Repository.Interfaces;
using DTO;
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
                var supermarkets = await _unitOfWork.SupermarketRepository.GetAllSupermarkets();

                return Ok(supermarkets);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("AddSupermarket")]
        public async Task<IActionResult> AddSupermarket([FromBody] SupermarketDTO supermarket)
        {
            try
            {
                await _unitOfWork.SupermarketRepository.AddSupermarket(supermarket);

                return Ok(supermarket);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
