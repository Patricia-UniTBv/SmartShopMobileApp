using API.Models;
using API.Repository.Interfaces;
using AutoMapper;
using DTO;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class UserRepository:BaseRepository<User>, IUserRepository 
    {
        public UserRepository(SmartShopDBContext context, IMapper _mapper) : base(context, _mapper)
        {
        }

        public async Task<UserDTO> GetUserByID(int ID)
        {
            var result = await _dbSet.SingleAsync(u => u.UserID == ID);
            return _mapper.Map<User, UserDTO>(result);
        }

        public async Task<UserDTO> GetUserByEmailAndPassword(string email, string password)
        {
            var result = await _dbSet.SingleAsync(u => u.Email == email && u.Password == password) ;
            return _mapper.Map<User, UserDTO>(result);
        }

        public async Task AddNewUser(UserDTO newUser)
        {
            var result = _mapper.Map<UserDTO, User>(newUser);
            await _dbSet.AddAsync(result);
            _context.SaveChanges();
        }
    }
}
