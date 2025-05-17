using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace ApiExplorerApp.Explorer;

public static class OpenApiAuthorizationExtensions
{
    public static Task<AuthorizationPolicy?> GetAuthorizationPolicy(this IAuthorizationPolicyProvider policyProvider, ApiDescription description)
    {
        if (description.IsAllowAnonymous())
        {
            return Task.FromResult<AuthorizationPolicy?>(null);
        }

        var authorizeData = description.ActionDescriptor.EndpointMetadata.OfType<IAuthorizeData>();
        var endpointPolicies = description.ActionDescriptor.EndpointMetadata.OfType<AuthorizationPolicy>();

        return AuthorizationPolicy.CombineAsync(policyProvider, authorizeData, endpointPolicies);
    }

    public static Task<AuthorizationPolicy?> GetAuthorizationPolicy(this ApiDescription description, IAuthorizationPolicyProvider policyProvider)
    {
        ArgumentNullException.ThrowIfNull(description);
        ArgumentNullException.ThrowIfNull(policyProvider);

        return policyProvider.GetAuthorizationPolicy(description);
    }

    public static bool IsAllowAnonymous(this ApiDescription description)
    {
        ArgumentNullException.ThrowIfNull(description);

        return description.ActionDescriptor.EndpointMetadata.OfType<IAllowAnonymous>().Any();
    }
}