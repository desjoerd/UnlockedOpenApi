using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

using Unlocked.Api.Endpoints.Models;

namespace Unlocked.Api.Endpoints;

public static class GetLockById
{
    public static void MapGetLockById(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("locks/{lockId:guid}", Handler)
            .WithName(nameof(GetLockById));
    }

    private class LockItem
    {
        public required LockStatus LockStatus { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Location { get; set; }
    }

    private static Results<Ok<LockItem>, NotFound<ProblemDetails>> Handler(Guid lockId)
    {
        if (lockId == Guid.Empty)
        {
            return TypedResults.NotFound(new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Lock not found",
                Detail = "The lock with the specified ID was not found."
            });
        }

        return TypedResults.Ok(new LockItem
        {
            LockStatus = LockStatus.Unlocked
        });
    }
}
