using System.ComponentModel.DataAnnotations;

using Asp.Versioning;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

using Unlocked.Api.Endpoints.Models;

namespace Unlocked.Api.Endpoints;

internal static class PostLockByIdUnlockV1
{
    internal static void MapPostLockByIdUnlockV1(this IEndpointRouteBuilder routes)
    {
        routes
            .MapPost("locks/unlock", HandlerV1)
            .HasDeprecatedApiVersion(new ApiVersion(1))
            .WithName(nameof(PostLockByIdUnlockV1));
    }

    internal class UnlockRequestBody
    {
    }

    internal class UnlockRequestBodyV2
    {
        [MinLength(6)]
        [MaxLength(6)]
        public required string Pin { get; set; }
    }

    internal class UnlockResponse
    {
        public required LockStatus Status { get; set; }
    }

    private static async Task<Results<Ok<UnlockResponse>, NotFound, ProblemHttpResult>> HandlerV1(
        UnlockRequestBody requestBody,
        ApiVersion apiVersion,
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

    private static async Task<Results<Ok<UnlockResponse>, NotFound, ProblemHttpResult>> HandlerV2(
        [FromRoute] Guid lockId,
        UnlockRequestBodyV2 requestBody,
        ApiVersion apiVersion,
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