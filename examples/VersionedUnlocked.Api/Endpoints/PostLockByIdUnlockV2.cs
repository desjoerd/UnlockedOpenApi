using System.ComponentModel.DataAnnotations;

using Asp.Versioning;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

using VersionedUnlocked.Api.Endpoints.Models;

namespace VersionedUnlocked.Api.Endpoints;

internal static class PostLockByIdUnlockV2
{
    internal static void MapPostLockByIdUnlockV2(this IEndpointRouteBuilder routes)
    {
        routes
            .MapPost("locks/{lockId:guid}/unlock", Handler)
            .HasApiVersion(new ApiVersion(2))
            .WithName(nameof(PostLockByIdUnlockV2));
    }

    internal class UnlockRequestBody
    {
        [MinLength(6)]
        [MaxLength(6)]
        public required string Pin { get; set; }
    }

    internal class UnlockResponse
    {
        public required LockStatus Status { get; set; }
    }

    private static async Task<Results<Ok<UnlockResponse>, NotFound, ProblemHttpResult>> Handler(
        [FromRoute] Guid lockId,
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
}