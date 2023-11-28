using Grpc.Net.Client;
using ProfileGateway;
using ProfileGateway.GrpcClients;
using ProfileGateway.HttpClients;
using ProfileGateway.Middlewares;
using ProfileGateway.Models;
using ProfileGateway.Services;
using GoogleTimestamp = Google.Protobuf.WellKnownTypes.Timestamp;

var builder = WebApplication.CreateBuilder(args);

var urls = new ExternalUrls();

builder.Configuration.GetSection("ExternalUrls").Bind(urls);
builder.Services.AddSingleton(urls);
builder.Services.AddHttpClient();

var channel = GrpcChannel.ForAddress(urls.ProfileGrpcRendererUrl);

// HTTP clients
builder.Services.AddHttpClient<ProfileDataProvider>();
builder.Services.AddHttpClient<ProfileRenderProvider>();
builder.Services.AddScoped<ProfileDataProvider>();
builder.Services.AddScoped<ProfileRenderProvider>();

// GRPC Clients & Services
builder.Services.AddSingleton(channel);
builder.Services.AddSingleton<GrpcProfileDataProvider>();
builder.Services.AddSingleton<GrpcProfileRenderProvider>();
builder.Services.AddSingleton((sp) =>
{
    var externalUrls = sp.GetRequiredService<ExternalUrls>();
    var renderClient = new ProfileRenderService.ProfileRenderServiceClient(channel);

    return renderClient;
});

builder.Services.AddSingleton<PingService>();
builder.Services.AddHostedService((sp) => sp.GetRequiredService<PingService>());

builder.Services.AddScoped<ContentProxyMiddleware>();

var app = builder.Build();

app.UseMiddleware<ContentProxyMiddleware>();

app.MapGet("/", () => "Profile Gateway is OK!");
app.MapGet("/profile/{userId}",
    async (
        string userId,
        HttpContext context,
        ProfileDataProvider dataProvider,
        ProfileRenderProvider renderProvider,
        GrpcProfileDataProvider grpcDataprovider,
        GrpcProfileRenderProvider grpcRenderProvider) => {
        
    var isDataGrpc = context.Request.Query["data_grpc"].Any();
    var isRenderGrpc = context.Request.Query["render_grpc"].Any();

    ProfileInfo? data;
    if (isDataGrpc)
    {
        var dataResponse = await grpcDataprovider.GetProfileDataAsync(userId);

        data = MapProfileResponse(dataResponse!);
    }
    else
    {
        data = await dataProvider.GetProfileAsync(userId);
    }

    if (isRenderGrpc)
    {
        var renderReq = GetGrpcRenderRequest(data!);
        var renderResult = await grpcRenderProvider.RenderProfileAsync(renderReq);
        
        if (renderResult?.Status != 200)
        {
            return Results.BadRequest();
        }

        return Results.Content(renderResult.Html, "text/html");
    }

    var renderRequest = GetHttpRenderRequest(data!);
    var renderResponse = await renderProvider.GetRenderResultAsync(renderRequest);

    return renderResponse?.Status == 200 ?
            Results.Content(renderResponse.Html, "text/html") : Results.BadRequest();
});

app.MapGet("/ping", async (ProfileRenderService.ProfileRenderServiceClient renderServiceClient) => {

    var pingRequest = new PingRequest { Ping = "PING!!!" };
    var pingResponse = await renderServiceClient.pingAsync(pingRequest);

    return Results.Ok(pingResponse);
});

app.Run();

#region Create Render requests & responses helper methods

static ProfileRenderRequest GetGrpcRenderRequest(ProfileInfo data)
{
    var request = new ProfileRenderRequest
    {
        AvatarUrl = data.AvatarUrl,
        Name = data.Name,
        Age = data.Age,
        GeneralInfo = data.GeneralInfo,
        Email = data.Email,
        Phone = data.Phone,
        ZipCode = data.ZipCode,
        PreferredLanguage = data.PreferredLanguage,
        Address = new Address
        {
            City = data.Address?.City,
            Street = data.Address?.Street
        }
    };

    if (data.Categories is not null)
    {
        foreach (var category in data.Categories)
        {
            request.Categories.Add(category);
        }
    }

    if (data.Feedbacks is not null)
    {
        foreach (var feedback in data.Feedbacks)
        {
            request.Feedbacks.Add(
                new ProfileGateway.FeedbackData()
                {
                    CustomerName = feedback.CustomerName,
                    FeedbackBody = feedback.FeedbackBody,
                    Timestamp = GoogleTimestamp.FromDateTime(feedback.Timestamp.ToUniversalTime())
                });
        }
    }

    if (data.LatestQuestions is not null)
    {
        foreach (var question in data.LatestQuestions)
        {
            request.LatestQuestions.Add(
                new ProfileGateway.QuestionData()
                {
                    QuestionId = question.QuesitionId,
                    CustomerName = question.CustomerName,
                    Text = question.Text,
                    Timestamp = GoogleTimestamp.FromDateTime(question.Timestamp.ToUniversalTime())
                });
        }
    }

    return request;
}

static ProfileRenderRequestDto GetHttpRenderRequest(ProfileInfo data)
{
    return new ProfileRenderRequestDto()
    {
        AvatarUrl = data.AvatarUrl,
        Name = data.Name,
        Age = data.Age,
        GeneralInfo = data.GeneralInfo,
        Email = data.Email,
        Phone = data.Phone,
        ZipCode = data.ZipCode,
        PreferredLanguage = data.PreferredLanguage,
        Address = new AddressDto
        {
            City = data.Address?.City ?? string.Empty,
            Street = data.Address?.Street ?? string.Empty
        },
        Categories = data.Categories,
        Feedbacks = data.Feedbacks,
        LatestQuestions = data.LatestQuestions
    };
}

static ProfileInfo MapProfileResponse(ProfileResponse dataResponse)
{
    if (dataResponse is null)
    {
        return new ProfileInfo();
    }

    var response = new ProfileInfo()
    {
        AvatarUrl = dataResponse.AvatarUrl ?? string.Empty,
        Name = dataResponse.UserName ?? string.Empty,
        Age = dataResponse.UserAge,
        GeneralInfo = dataResponse.UserGeneralInfo ?? string.Empty,
        Email = dataResponse.EmailAddress ?? string.Empty,
        Phone = dataResponse.PhoneNumber ?? string.Empty,
        ZipCode = dataResponse.Zip ?? string.Empty,
        PreferredLanguage = dataResponse.PreferredLanguage ?? string.Empty,
        Address = new AddressDto
        {
            Street = dataResponse.AddressData?.Street ?? string.Empty,
            City = dataResponse.AddressData?.City ?? string.Empty,
        }
    };

    response.Categories = dataResponse!.Categories;

    response.Feedbacks = dataResponse.Feedbacks.Select(f => 
        new ProfileGateway.Models.FeedbackData { 
                CustomerName = f.CustomerName,
                FeedbackBody = f.FeedbackBody,
                Timestamp = f.Timestamp.ToDateTime()
        }).ToList();

    response.LatestQuestions = dataResponse.LatestQuestions.Select( q =>
        new ProfileGateway.Models.QuestionData
        {
            QuesitionId = q.QuestionId,
            CustomerName = q.CustomerName,
            Text = q.Text,
            Timestamp = q.Timestamp.ToDateTime()
        }).ToList();

    return response;
}

#endregion