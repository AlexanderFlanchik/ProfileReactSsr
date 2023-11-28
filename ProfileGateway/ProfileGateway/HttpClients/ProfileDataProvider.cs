using ProfileGateway.Models;
using System.Text.Json;

namespace ProfileGateway.HttpClients
{
    public class ProfileDataProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ExternalUrls _externalUrls;

        public ProfileDataProvider(HttpClient httpClient, ExternalUrls externalUrls)
        {
            _httpClient = httpClient;
            _externalUrls = externalUrls;
        }

        public async Task<ProfileInfo?> GetProfileAsync(string userId)
        {
            using var response = await _httpClient.GetAsync($"{_externalUrls.ProfileHttpDataSourceUrl}{userId}");
            
            response.EnsureSuccessStatusCode();

            var profileContent = await response.Content.ReadAsStringAsync();
            var profileInfo = JsonSerializer.Deserialize<ProfileInfo>(profileContent, new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            
            return profileInfo;
        }
    }
}