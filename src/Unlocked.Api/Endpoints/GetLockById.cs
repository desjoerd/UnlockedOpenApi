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
            .HasApiVersion(new ApiVersion(2));
    }

    private class LockItem
    {
        public LockStatus Status { get; set; }
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
