using ApiExplorerApp.Explorer;
using ApiExplorerApp.Explorer.Models;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Unlocked.Api.Explorer.Endpoints;

public static class GetApiExplorer
{
    public static void MapGetApiExplorer(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("api-explorer", Handler).ExcludeFromDescription();
    }

    public static async Task<Ok<List<ApiDescriptionGroupModel>>> Handler(
        [FromServices] ExploredAppProvider exploredApp)
    {
        return TypedResults.Ok(await exploredApp.GetApiDescriptionGroups());
    }
}