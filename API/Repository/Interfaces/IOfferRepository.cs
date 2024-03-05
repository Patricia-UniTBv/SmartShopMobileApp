using DTO;

namespace API.Repository.Interfaces
{
    public interface IOfferRepository
    {
        Task<ICollection<OfferDTO>> GetAllCurrentOffers();
    }
}
