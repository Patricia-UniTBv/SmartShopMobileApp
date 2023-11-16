using DTO;

namespace API.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<UserDTO> GetUserByID(int ID);
    }
}
