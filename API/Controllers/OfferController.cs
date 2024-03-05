using API.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class OfferController: ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public OfferController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpGet("GetAllCurrentOffers")]
        public async Task<IActionResult> GetAllCurrentOffers()
        {
            try
            {
                var offers = await _unitOfWork.OfferRepository.GetAllCurrentOffers();

                foreach(var o in offers)
                {
                    o.Product = await _unitOfWork.ProductRepository.GetProductById(o.ProductId);
                    o.Supermarket = await _unitOfWork.SupermarketRepository.GetSupermarketById(o.SupermarketId);
                }

                return Ok(offers);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


    }
}
