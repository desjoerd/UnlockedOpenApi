using Microsoft.AspNetCore.OpenApi;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;

namespace VersionedUnlocked.Api.OpenApi;

public class UnlockedOpenApiServersDocumentTransformer(IOptionsSnapshot<UnlockedOpenApiCustomizationOptions> options) : IOpenApiDocumentTransformer
{
    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        if (options.Value.Servers.Count == 0)
        {
            return Task.CompletedTask;
        }

        document.Servers ??= [];
        foreach ((string server, string serverUrl) in options.Value.Servers)
        {
            document.Servers.Add(new OpenApiServer
            {
                Url = serverUrl,
                Description = server
            });
        }

        return Task.CompletedTask;
    }
}