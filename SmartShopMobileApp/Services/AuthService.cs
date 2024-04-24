using DTO;
using SmartShopMobileApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmartShopMobileApp.Services
{
    public interface IAuthService
    {
        Task<bool> IsUserAuthenticated();
        Task<string> LoginAsync(LoginRequestDTO dto);
        Task<AuthResponseDTO> GetAuthenticatedUserAsync();
        Task<HttpClient> GetAuthenticatedHttpClientAsync();
        void Logout();
    }

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

        public async Task<string?> LoginAsync(LoginRequestDTO dto) //DECRIPTARE PAROLA!!!
        {
            var httpClient = new HttpClient(App.Current.MainPage.Handler.MauiContext.Services.GetService<IHttpClientHandlerService>().GetInsecureHandler());
            string Uri = DeviceInfo.Platform == DevicePlatform.Android ? "https://10.0.2.2:7116/api/" : "https://localhost:5152";

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
    }
}
