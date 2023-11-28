namespace ProfileGateway.GrpcClients
{
    public class GrpcProfileRenderProvider(ProfileRenderService.ProfileRenderServiceClient profileRenderServiceClient)
    {
        public async Task<ProfileRenderResponse> RenderProfileAsync(ProfileRenderRequest renderRequest)
        {
            var renderResult = await profileRenderServiceClient.renderAsync(renderRequest);

            return renderResult;
        }
    }
}
