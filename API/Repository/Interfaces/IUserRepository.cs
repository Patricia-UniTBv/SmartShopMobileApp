using DTO;

namespace API.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<UserDTO> GetUserByID(int ID);
        Task<UserDTO> GetUserByEmailAndPassword(string email, string password);
        Task AddNewUser(UserDTO newUser);
    }
}
