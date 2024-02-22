using API.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class LocationController: ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public LocationController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetLocationsBySupermarketId")]
        public async Task<IActionResult> GetLocationsBySupermarketId(int supermarketId)
        {
            try
            {
                var locations = await _unitOfWork.LocationRepository.GetLocationsForSupermarketId(supermarketId);

                return Ok(locations);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
