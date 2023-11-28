using Grpc.Net.Client;

namespace ProfileGateway.Services
{
    public class PingService(
            IHostApplicationLifetime applicationLifetime, 
            GrpcChannel channel, 
            ProfileRenderService.ProfileRenderServiceClient renderServiceClient,
            ILogger<PingService> logger
        ) : BackgroundService
    {
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Initialization of GRPC support background worker..");

            applicationLifetime.ApplicationStopping.Register(() => {
                logger.LogInformation("Closing GRPC prerender channel..");
                channel?.Dispose();
            });

            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var pingRequest = new PingRequest { Ping = "PING!!!" };
                    _ = await renderServiceClient.pingAsync(pingRequest);
                }
                catch
                {
                    logger.LogError("Unable to send ping request to rendering service. It does not respond.");
                }

                await Task.Delay(TimeSpan.FromMinutes(1));
            }
        }
    }
}