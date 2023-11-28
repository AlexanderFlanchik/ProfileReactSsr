using ProfileGateway.Models;
using System.Text.Json;

namespace ProfileGateway.HttpClients
{
    public class ProfileRenderProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ExternalUrls _externalUrls;

        public ProfileRenderProvider(HttpClient httpClient, ExternalUrls externalUrls)
        {
            _httpClient = httpClient;
            _externalUrls = externalUrls;
        }

        public async Task<ProfileRenderResponseDto?> GetRenderResultAsync(ProfileRenderRequestDto renderRequest)
        {
            var renderResponse = await _httpClient.PostAsJsonAsync(_externalUrls.ProfileHttpRendererUrl, renderRequest);
            renderResponse.EnsureSuccessStatusCode();

            var renderedContent = await renderResponse.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<ProfileRenderResponseDto>(renderedContent, 
                    new JsonSerializerOptions 
                    { 
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
                    }
            );

            return response;
        }
    }
}