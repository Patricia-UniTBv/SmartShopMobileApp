using API.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class UserController: ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("UpdateLanguage")]
        public async Task<IActionResult> UpdateLanguage(int userId, string language)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.UpdateLanguage(userId, language);

                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("UpdateCurrency")]
        public async Task<IActionResult> UpdateCurrency(int userId, string currency)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.UpdateCurrency(userId, currency);

                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
