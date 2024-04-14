using DTO;

namespace API.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<UserDTO> GetUserByID(int ID);
        Task<UserDTO> UpdateLanguage(int userId, string language);
        Task<UserDTO> UpdateCurrency(int userId, string currency);

    }
}
