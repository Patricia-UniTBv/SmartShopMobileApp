using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartShopMobileApp.Services.Interfaces
{
   public interface IAuthService
    {
        Task<bool> IsUserAuthenticated();
        Task<string> LoginAsync(LoginRequestDTO dto);
        Task<AuthResponseDTO> GetAuthenticatedUserAsync();
        Task<HttpClient> GetAuthenticatedHttpClientAsync();
        void Logout();
    }
}
