using Unlocked.Api.Explorer.Endpoints;

namespace ApiExplorerApp.Explorer;

public static class ExplorerEndpoints
{
    public static void MapExplorer(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("explorer")
            .DisableValidation();

        group.MapGetApiExplorer();
    }
}