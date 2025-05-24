using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Authentication.BearerToken;

using Unlocked.Api.Endpoints;
using Unlocked.Api.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.NumberHandling = JsonNumberHandling.Strict;
});

builder.Services.AddOptions<UnlockedOpenApiCustomizationOptions>()
    .BindConfiguration("UnlockedOpenApiCustomization");

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<UnlockedOpenApiServersDocumentTransformer>();
    options.AddDocumentTransformer<SecuritySchemeDocumentTransformer>();

    options.AddLoggingOpenApiTransformers();
});

builder.Services.AddValidation();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter(namingPolicy: System.Text.Json.JsonNamingPolicy.KebabCaseLower));
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
    app.MapOpenApi("/openapi/{documentName}.yaml");
}

app.UseHttpsRedirection();

var apiGroup = app
    .MapGroup("");

apiGroup.ProducesProblem(500);

apiGroup.MapGetLockById();
apiGroup.MapPostLockByIdUnlock();

app.MapControllers();

app.MapGet("/", () => "Welcome to the Unlocked OpenAPI!");

app.Run();

public partial class Program;
