using ProfileDataService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IProfileProvider, ProfileProvider>();
builder.Services.AddGrpc();

var app = builder.Build();

app.MapGet("/", () => "Profile Data Service is OK");
app.MapGet("/profile/{userId}", async (string userId, IProfileProvider profileService)
    => Results.Ok(await profileService.GetProfileAsync(userId)));

app.MapGrpcService<ProfileInfoService>();

app.Run();