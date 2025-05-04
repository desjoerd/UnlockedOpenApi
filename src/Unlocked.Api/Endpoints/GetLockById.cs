using System.Text.Json.Serialization;

using Asp.Versioning;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

using Unlocked.Api.Endpoints.Models;

namespace Unlocked.Api.Endpoints;

public static class GetLockById
{
    public static void MapGetLockById(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("locks/{lockId:guid}", Handler)
            .HasApiVersion(new ApiVersion(1))
            .HasApiVersion(new ApiVersion(2))
            .WithName(nameof(GetLockById));
    }

    private class LockItem
    {
        public required LockStatus Status { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Location { get; set; }
    }

    private static Results<Ok<LockItem>, NotFound<ProblemDetails>> Handler(
        Guid lockId
    )
    {
        return TypedResults.Ok(new LockItem
        {
            Status = LockStatus.Unlocked
        });
    }
}
