using Microsoft.AspNetCore.Authentication.BearerToken;

using Unlocked.Api.Endpoints;
using Unlocked.Api.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions<UnlockedOpenApiCustomizationOptions>()
    .BindConfiguration("UnlockedOpenApiCustomization");

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<UnlockedOpenApiServersDocumentTransformer>();

    options.AddLoggingOpenApiTransformers();
});

builder.Services.AddValidation();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter(namingPolicy: System.Text.Json.JsonNamingPolicy.KebabCaseLower));
});

builder.Services.Configure<RouteHandlerOptions>(options =>
{
    // set to true to catch JsonSerializer exceptions
    options.ThrowOnBadRequest = true;
});

builder.Services.AddAuthentication(BearerTokenDefaults.AuthenticationScheme)
    .AddBearerToken();

builder.Services.AddAuthorizationBuilder()
    .AddDefaultPolicy("Authenticated", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.AddAuthenticationSchemes(BearerTokenDefaults.AuthenticationScheme);
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var apiGroup = app
    .MapGroup("");

apiGroup
    .WithMetadata(new ProducesResponseTypeMetadata(200, typeof(int)))
    .ProducesProblem(400)
    .ProducesProblem(401)
    .ProducesProblem(403)
    .ProducesProblem(404)
    .ProducesProblem(500);


apiGroup.MapGetLockById();
apiGroup.MapGetWeatherForecast();
apiGroup.MapPostLockByIdUnlock();

app.Run();

internal partial class Program;
