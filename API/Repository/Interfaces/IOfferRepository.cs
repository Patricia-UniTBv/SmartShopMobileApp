using DTO;

namespace API.Repository.Interfaces
{
    public interface IOfferRepository
    {
        Task<ICollection<OfferDTO>> GetAllCurrentOffers();
        Task<OfferDTO> GetActiveOfferForProduct(int productId, int supermarketId, DateTime currentDate);
    }
}
