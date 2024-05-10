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

        public async Task<List<UserDTO>> GetAllUsers()
        {
            var result = await _dbSet.ToListAsync();
            return _mapper.Map<List<User>, List<UserDTO>>(result);
        }

        public async Task<UserDTO> GetUserByEmailAndPassword(string email, string password)
        {
            var user = await _dbSet.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);

            if (user == null)
            {
                return new UserDTO();
            }
            return _mapper.Map<User, UserDTO>(user);
        }

        public async Task<UserDTO> GetUserByID(int ID)
        {
            var result = await _dbSet.SingleAsync(u => u.UserID == ID);
            return _mapper.Map<User, UserDTO>(result);
        }

        public async Task<Tuple<string,string>> GetPreferredLanguageAndCurrency(int userId)
        {
            var dbUser = await _dbSet.SingleAsync(u => u.UserID == userId);

            string language = dbUser.PreferredLanguage ?? "en";
            string currency = dbUser.PreferredCurrency ?? "RON";

            return Tuple.Create(language, currency);
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

        public async Task AddNewUser(UserDTO newUser)
        {
            var result = _mapper.Map<UserDTO, User>(newUser);
            await _dbSet.AddAsync(result);
            _context.SaveChanges();
        }

    }
}
