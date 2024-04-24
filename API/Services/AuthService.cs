using API.Repository;
using API.Repository.Interfaces;
using API.Services.Interfaces;
using DTO;

namespace API.Services
{
    public sealed class AuthService : IAuthService
    {
        private readonly ITokenService _tokenService;
        private IUserRepository _userRepository;

        public AuthService(ITokenService tokenService, IUserRepository userRepository)
        {
            _tokenService = tokenService;
            _userRepository = userRepository;
        }

        public async Task<ApiResponse<AuthResponseDTO>> LoginAsync(LoginRequestDTO loginDto, CancellationToken cancellationToken = default)
        {
            var user = _userRepository.GetUserByEmailAndPassword(loginDto.Email, loginDto.Password).Result;
            var loggedInUser = new LoggedInUser(user.UserID, user.LastName, user.FirstName, user.Email, user.Password, user.PreferredLanguage, user.PreferredCurrency);
            
            var jwt = _tokenService.GenerateJWT(loggedInUser);

            var authResponse = new AuthResponseDTO
            {
                UserId = loggedInUser.UserId,
                Email = loggedInUser.Email,
                FirstName = loggedInUser.FirstName,
                LastName = loggedInUser.LastName,
                Password = loggedInUser.Password,
                PreferredLanguage = loggedInUser.PreferredLanguage,
                PreferredCurrency = loggedInUser.PreferredCurrency,
                Token = jwt
            };
            return ApiResponse<AuthResponseDTO>.Success(authResponse);
        }
    }
}
