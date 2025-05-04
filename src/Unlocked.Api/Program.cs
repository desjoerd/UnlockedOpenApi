
using System.ComponentModel.DataAnnotations;

using Asp.Versioning;

using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.OpenApi.Models;

using Unlocked.Api.Endpoints;
using Unlocked.Api.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions<UnlockedOpenApiCustomizationOptions>()
    .BindConfiguration("UnlockedOpenApiCustomization");

builder.Services.AddOpenApi("v1", options =>
{
    options.AddDocumentTransformer<UnlockedOpenApiServersDocumentTransformer>();

    options.PatchDocumentForVersion(new ApiVersion(1));
    options.AddLoggingOpenApiTransformers();
});
builder.Services.AddOpenApi("v2", options =>
{
    options.AddDocumentTransformer<UnlockedOpenApiServersDocumentTransformer>();

    options.PatchDocumentForVersion(new ApiVersion(2));
    options.AddLoggingOpenApiTransformers();
});

builder.Services.AddValidation();

builder.Services.AddApiVersioning(options =>
    {
        options.ApiVersionReader = new UrlSegmentApiVersionReader();
        options.UnsupportedApiVersionStatusCode = 501;
    })
    .AddApiExplorer(options =>
    {
        options.SubstituteApiVersionInUrl = false;
        options.GroupNameFormat = "'v'VVV";
    })
    .EnableApiVersionBinding();

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
    .NewVersionedApi()
    .MapGroup("/v{version:apiVersion}");

apiGroup
    .WithMetadata(new ProducesResponseTypeMetadata(200, typeof(int)))
    .ProducesProblem(400)
    .ProducesProblem(401)
    .ProducesProblem(403)
    .ProducesProblem(404)
    .ProducesProblem(500);


apiGroup.MapGetWeatherForecast();
apiGroup.MapPostLockByIdUnlock();
apiGroup.MapGetLockById();

app.Run();

internal partial class Program {
}