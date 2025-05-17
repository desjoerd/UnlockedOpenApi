using Asp.Versioning;

using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace VersionedUnlocked.Api.OpenApi;

public static class OpenApiVersioningExtensions
{
    public static void PatchDocumentForVersion(this OpenApiOptions options, ApiVersion apiVersion)
    {
        var previousShouldInclude = options.ShouldInclude;
        options.ShouldInclude = apiDescription =>
        {
            var prevResult = previousShouldInclude(apiDescription);
            if (!prevResult)
            {
                return false;
            }

            var apiVersionModel = apiDescription.ActionDescriptor.GetApiVersionMetadata();
            if (apiVersionModel.IsMappedTo(apiVersion))
            {
                return true;
            }

            return false;
        };

        options.AddOperationTransformer(new MarkAsDeprecatedOperationTransformer(apiVersion));
        options.AddDocumentTransformer(new SetVersionDocumentTransformer(apiVersion));
        options.AddDocumentTransformer(new MoveApiVersionFromPathToServersDocumentTransformer(apiVersion));
    }

    private class MarkAsDeprecatedOperationTransformer(ApiVersion apiVersion) : IOpenApiOperationTransformer
    {
        public Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
        {
            var versionMetadata = context.Description.ActionDescriptor.GetApiVersionMetadata();
            if (versionMetadata.Map(ApiVersionMapping.Explicit).DeprecatedApiVersions.Contains(apiVersion))
            {
                operation.Deprecated = true;
            }
            return Task.CompletedTask;
        }
    }

    private class SetVersionDocumentTransformer(ApiVersion apiVersion) : IOpenApiDocumentTransformer
    {
        public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
        {
            document.Info.Version = $"v{apiVersion.ToString()}";

            return Task.CompletedTask;
        }
    }

    private class MoveApiVersionFromPathToServersDocumentTransformer(ApiVersion apiVersion) : IOpenApiDocumentTransformer
    {
        public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
        {
            // rewrite servers to include the version in the path

            document.Servers ??= [];
            if (document.Servers.Count == 0)
            {
                document.Servers.Add(new OpenApiServer { Url = "" });
            }

            foreach (var server in document.Servers)
            {
                var versionPart = $"v{apiVersion.MajorVersion}";

                var uri = new Uri(server.Url, UriKind.RelativeOrAbsolute);
                var path = uri.IsAbsoluteUri ? uri.PathAndQuery : uri.OriginalString;
                path = path.TrimStart('/');

                string newUrl = server.Url;
                if (!path.StartsWith(versionPart))
                {
                    if (uri.IsAbsoluteUri)
                    {
                        newUrl = new UriBuilder(uri)
                        {
                            Path = $"/{versionPart}/{path}",
                        }.Uri.ToString();
                    }
                    else
                    {
                        newUrl = $"/{versionPart}/{path}";
                    }
                }

                server.Url = newUrl.TrimEnd('/');
            }

            // rewrite operations to not include the version in the path
            var paths = document.Paths.ToList();
            document.Paths.Clear();
            foreach (var path in paths)
            {
                var newPath = path.Key.StartsWith("/v{version}/")
                    ? string.Concat("/", path.Key.AsSpan("/v{version}/".Length))
                    : path.Key;

                newPath = newPath.StartsWith($"/v{apiVersion.MajorVersion}/")
                    ? string.Concat("/", newPath.AsSpan($"/v{apiVersion.MajorVersion}/".Length))
                    : newPath;

                document.Paths.Add(newPath, path.Value);

                // also remove the version from the parameters
                foreach (var operation in path.Value.Operations)
                {
                    var parameters = operation.Value.Parameters?.ToList() ?? [];
                    foreach (var parameter in parameters)
                    {
                        if (parameter is { Name: "version", In: ParameterLocation.Path })
                        {
                            // remove the version parameter from the operation
                            operation.Value.Parameters!.Remove(parameter);
                        }
                    }
                }
            }
            return Task.CompletedTask;
        }
    }
}