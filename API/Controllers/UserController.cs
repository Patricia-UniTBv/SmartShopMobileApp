using API.Repository.Interfaces;
using DTO;
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

        
        [HttpGet("GetUserByEmailAndPassword")]
        public async Task<IActionResult> GetUserByEmailAndPassword(string email, string password)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetUserByEmailAndPassword(email, password);

                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("AddNewUser")]
        public async Task<IActionResult> AddNewUser(string email, string password, string firstName, string lastName, DateTime birthdate)
        {
            try
            {
                var newUser = new UserDTO
                {
                    Email = email,
                    Password = password, 
                    FirstName = firstName,
                    LastName = lastName,
                    Birthdate = birthdate
                };
                await _unitOfWork.UserRepository.AddNewUser(newUser);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
