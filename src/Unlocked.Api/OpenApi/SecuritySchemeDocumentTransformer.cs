namespace Unlocked.Api.OpenApi;

using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Extensions;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Models.Interfaces;
using Microsoft.OpenApi.Models.References;

public class SecuritySchemeDocumentTransformer
    (IAuthenticationSchemeProvider authenticationSchemeProvider)
 : IOpenApiDocumentTransformer
{
    public async Task TransformAsync(
        OpenApiDocument document,
        OpenApiDocumentTransformerContext context,
        CancellationToken cancellationToken)
    {
        var authenticationSchemes = await authenticationSchemeProvider.GetAllSchemesAsync();
        if (authenticationSchemes.Any(authScheme => authScheme.Name == BearerTokenDefaults.AuthenticationScheme))
        {
            // Add the security scheme at the document level
            document.AddComponent("Bearer", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Scheme = "bearer",
                In = ParameterLocation.Header,
                BearerFormat = "Custom Bearer Token",
                Flows = new OpenApiOAuthFlows
                {
                    ClientCredentials = new OpenApiOAuthFlow
                    {
                        TokenUrl = new Uri("/api/auth/token", UriKind.Relative),
                        Scopes = new Dictionary<string, string>
                        {
                            [".default"] = "Access Unlocked API with the .default scope",
                        }
                    }
                }
            });
            document.RegisterComponents();
            // Apply it as a requirement for all operations
            foreach (var operation in document.Paths.Values.SelectMany(path => path.Operations))
            {
                operation.Value.Security ??= [];
                operation.Value.Security.Add(new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("Bearer", document)] = [".default"],
                });
            }
        }
    }
}