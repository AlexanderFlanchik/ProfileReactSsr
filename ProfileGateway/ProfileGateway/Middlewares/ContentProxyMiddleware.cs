using ProfileGateway.Models;

namespace ProfileGateway.Middlewares
{
    public class ContentProxyMiddleware : IMiddleware
    {
        private readonly ExternalUrls _externalUrls;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly static string[] _content_exts = { ".js", ".css", ".png", ".gif", ".svg" };

        public ContentProxyMiddleware(ExternalUrls externalUrls, IHttpClientFactory httpClientFactory)
        {
            _externalUrls = externalUrls;
            _httpClientFactory = httpClientFactory;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var request = context.Request;
            var path = request.Path;
           
            if (path.StartsWithSegments(new PathString("/content")) && _content_exts.Any(c => path.Value!.EndsWith(c)))
            {
                var segment = path.Value!.Replace("/content/", string.Empty);
                var contentUrl = $"{_externalUrls.ProfileHttpRendererContentUrl}{segment}";

                var client = _httpClientFactory.CreateClient();
                
                using var response = await client.GetAsync(contentUrl);
                response.EnsureSuccessStatusCode();

                using var stream = await response.Content.ReadAsStreamAsync();

                await stream.CopyToAsync(context.Response.Body);
            }
            else
            {
                await next(context);
            }
        }
    }
}