using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace ApiExplorerApp.Explorer.Models;

public class AuthorizationDescriptor
{
    public bool IsAllowAnonymous { get; set; }
    public bool RequiresAuthorization { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<RequirementDescriptor>? Requirements { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<string>? AuthenticationSchemes { get; set; }

    public static async Task<AuthorizationDescriptor?> Create(IAuthorizationPolicyProvider? policyProvider, ApiDescription description)
    {
        if (policyProvider is null)
        {
            return null;
        }

        var isAllowAnonymous = description.IsAllowAnonymous();
        var policy = await description.GetAuthorizationPolicy(policyProvider);

        if (policy is null)
        {
            return new AuthorizationDescriptor
            {
                IsAllowAnonymous = isAllowAnonymous,
                RequiresAuthorization = false,
            };
        }

        return new AuthorizationDescriptor
        {
            IsAllowAnonymous = isAllowAnonymous,
            RequiresAuthorization = true,
            Requirements = policy.Requirements.Select(requirement => new RequirementDescriptor(requirement)).ToList(),
            AuthenticationSchemes = policy.AuthenticationSchemes,
        };
    }
}