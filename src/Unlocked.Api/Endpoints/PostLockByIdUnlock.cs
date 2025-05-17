using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Http.HttpResults;

using Unlocked.Api.Endpoints.Models;

namespace Unlocked.Api.Endpoints;

internal static class PostLockByIdUnlock
{
    internal static void MapPostLockByIdUnlock(this IEndpointRouteBuilder routes)
    {
        routes
            .MapPost("locks/unlock", Handler)
            .WithName(nameof(PostLockByIdUnlock));
    }

    internal class UnlockRequestBody
    {
    }

    internal class UnlockResponse
    {
        public required LockStatus Status { get; set; }
    }

    private static async Task<Results<Ok<UnlockResponse>, NotFound, ProblemHttpResult>> Handler(
        UnlockRequestBody requestBody,
        CancellationToken cancellationToken)
    {
        // Simulate some async work
        await Task.Delay(1000, cancellationToken);

        // Simulate a successful unlock operation
        var response = new UnlockResponse
        {
            Status = LockStatus.Unlocked,
        };

        return TypedResults.Ok(response);
    }
}