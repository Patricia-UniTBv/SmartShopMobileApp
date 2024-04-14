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

        public async Task<UserDTO> UpdateLanguage(int userId, string language)
        {
            var dbUser = await _dbSet.SingleAsync(u => u.UserID == userId);
            var user = _mapper.Map<User, UserDTO>(dbUser);

            user.PreferredLanguage = language;

            var mapped = _mapper.Map<UserDTO, User>(user);

            _context.ChangeTracker.Clear();
            _context.Update(mapped);
            _context.SaveChanges();

            return user;
        }


        public async Task<UserDTO> UpdateCurrency(int userId, string currency)
        {
            var dbUser = await _dbSet.SingleAsync(u => u.UserID == userId);
            var user = _mapper.Map<User, UserDTO>(dbUser);

            user.PreferredCurrency = currency;

            var mapped = _mapper.Map<UserDTO, User>(user);

            _context.ChangeTracker.Clear();
            _context.Update(mapped);
            _context.SaveChanges();

            return user;
        }

    }
}
