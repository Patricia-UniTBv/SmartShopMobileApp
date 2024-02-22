using DTO;

namespace API.Repository.Interfaces
{
    public interface ILocationRepository
    {
        Task<ICollection<LocationDTO>> GetLocationsForSupermarketId(int supermarketId);
    }
}
