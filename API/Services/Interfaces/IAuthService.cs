using DTO;

namespace API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ApiResponse<AuthResponseDTO>> LoginAsync(LoginRequestDTO loginDto, CancellationToken cancellationToken = default);
    }
}
