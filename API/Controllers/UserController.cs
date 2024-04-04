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
                var employee = await _unitOfWork.UserRepository.UpdateLanguage(userId, language);

                return Ok(employee);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
