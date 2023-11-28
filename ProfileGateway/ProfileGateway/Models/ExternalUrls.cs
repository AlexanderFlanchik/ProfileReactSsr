namespace ProfileGateway.Models
{
    public class ExternalUrls
    {
        public string ProfileHttpDataSourceUrl { get; set; }
        public string ProfileHttpRendererUrl { get; set; }
        public string ProfileHttpStreamRendererUrl { get; set; }
        public string ProfileHttpRendererContentUrl { get; set; }
        public string ProfileGrpcDataSourceUrl { get; set; }
        public string ProfileGrpcRendererUrl { get; set; }
    }
}