using System.ComponentModel.DataAnnotations;

using Asp.Versioning;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Unlocked.Api.Endpoints;

internal static class PostLockByIdUnlock
{
    internal static void MapPostLockByIdUnlock(this IEndpointRouteBuilder routes)
    {
        routes
            .MapPost("lock/unlock", HandlerV1)
            .HasDeprecatedApiVersion(new ApiVersion(1))
            .WithName("UnlockV1");

        routes
            .MapPost("lock/{lockId:guid}/unlock", HandlerV2)
            .HasApiVersion(new ApiVersion(2))
            .WithName("UnlockV2");
    }

    internal class UnlockRequestBodyV1
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

    internal enum LockStatus
    {
        Locked,
        Unlocked,
        Unknown
    }

    private static async Task<Results<Ok<UnlockResponse>, NotFound, ProblemHttpResult>> HandlerV1(
        UnlockRequestBodyV1 requestBody,
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