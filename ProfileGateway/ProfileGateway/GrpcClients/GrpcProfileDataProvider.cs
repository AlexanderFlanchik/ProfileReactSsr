using Grpc.Net.Client;
using ProfileGateway.Models;

namespace ProfileGateway.GrpcClients
{
    public class GrpcProfileDataProvider
    {
        private readonly ExternalUrls _externalUrls;

        public GrpcProfileDataProvider(ExternalUrls externalUrls)
        {
            _externalUrls = externalUrls;
        }

        public async Task<ProfileResponse?> GetProfileDataAsync(string userId)
        {
            var dataRequest = new ProfileRequest { UserId = userId };
            using var channel = GrpcChannel.ForAddress(_externalUrls.ProfileGrpcDataSourceUrl);
            
            var dsClient = new ProfileDataService.ProfileDataServiceClient(channel);
            var response = await dsClient.GetProfileDataAsync(dataRequest);

            return response;
        }
    }
}