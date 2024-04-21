using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmartShopMobileApp.Services
{
    public class UserService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IAuthService _authService;

        public UserService(IHttpClientFactory httpClientFactory, IAuthService authService)
        {
            _httpClientFactory = httpClientFactory;
            _authService = authService;
        }

        public async Task<IEnumerable<UserDTO>?> GetUsersAsync()
        {
            var httpClient = await _authService.GetAuthenticatedHttpClientAsync();

            var response = await httpClient.GetAsync("GetAllUsers");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var users = JsonSerializer.Deserialize<IEnumerable<UserDTO>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return users;
            }
            else
            {
                var statusCode = response.StatusCode;
            }
            return null;
        }

    }
}
