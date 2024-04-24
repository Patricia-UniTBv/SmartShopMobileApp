using DTO;

namespace API.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<List<UserDTO>> GetAllUsers();
        Task<UserDTO> GetUserByID(int ID);
        Task<UserDTO> GetUserByEmailAndPassword(string email, string password);
        Task<Tuple<string, string>> GetPreferredLanguageAndCurrency(int userId);
        Task<UserDTO> UpdateLanguage(int userId, string language);
        Task<UserDTO> UpdateCurrency(int userId, string currency);
        Task AddNewUser(UserDTO newUser);

    }
}
