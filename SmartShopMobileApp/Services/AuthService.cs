using DTO;
using SmartShopMobileApp.Helpers;
using SmartShopMobileApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmartShopMobileApp.Services
{
    public class AuthService : IAuthService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public AuthService() { }

        public AuthService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> IsUserAuthenticated()
        {
            var serializedData = await SecureStorage.Default.GetAsync(AppConstants.AuthStorageKeyName);
            return !string.IsNullOrWhiteSpace(serializedData);
        }

        public async Task<AuthResponseDTO?> GetAuthenticatedUserAsync()
        {
            var serializedData = await SecureStorage.Default.GetAsync(AppConstants.AuthStorageKeyName);
            if (!string.IsNullOrWhiteSpace(serializedData))
            {
                return JsonSerializer.Deserialize<AuthResponseDTO>(serializedData);
            }
            return null;
        }

        public async Task<string?> LoginAsync(LoginRequestDTO dto)
        {
            var httpClient = new HttpClient(App.Current.MainPage.Handler.MauiContext.Services.GetService<IHttpClientHandlerService>().GetInsecureHandler());
            string Uri = "https://webappapiuni.azurewebsites.net/api/";

            var uri = new Uri(Uri + $"Auth/login?Email={dto.Email}&Password={dto.Password}");

            var response = await httpClient.PostAsJsonAsync<LoginRequestDTO>(uri, dto);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                ApiResponse<AuthResponseDTO> authResponse =
                    JsonSerializer.Deserialize<ApiResponse<AuthResponseDTO>>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                if (authResponse.Status)
                {
                    var serializedData = JsonSerializer.Serialize(authResponse.Data);
                    await SecureStorage.Default.SetAsync(AppConstants.AuthStorageKeyName, serializedData);
                }
                else
                {
                    return authResponse.Errors.FirstOrDefault();
                }
            }
            else
            {
                return "Error in logging in";
            }
            return null;
        }

        public void Logout() => SecureStorage.Default.Remove(AppConstants.AuthStorageKeyName);

        public async Task<HttpClient> GetAuthenticatedHttpClientAsync()
        {
            var httpClient = _httpClientFactory.CreateClient(AppConstants.HttpClientName);

            var authenticatedUser = await GetAuthenticatedUserAsync();

            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", authenticatedUser.Token);

            return httpClient;
        }

        private static string HashPassword(string password)
        {
            byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            StringBuilder builder = new();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }
}
