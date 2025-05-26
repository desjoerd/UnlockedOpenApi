using System.ComponentModel.DataAnnotations;
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

    internal class LockItem
    {
        public required LockStatus LockStatus { get; set; }

        [RegularExpression(@"^[a-zA-Z ]*$")]
        [MaxLength(120)]
        public string? Location { get; set; }
    }

    /// <summary>
    /// Get a lock by ID
    /// </summary>
    /// <description>
    /// This endpoint retrieves a lock by its ID. If the lock is not found, it returns a 404 Not Found response.
    /// </description>
    /// <param name="lockId" example="7fbe9a8e-9961-4262-8160-831cf9e47e91">The id of the lock</param>
    internal static Results<Ok<LockItem>, NotFound<ProblemDetails>> Handler(Guid lockId)
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
            LockStatus = LockStatus.Unlocked,
            Location = "Front Door"
        });
    }
}
