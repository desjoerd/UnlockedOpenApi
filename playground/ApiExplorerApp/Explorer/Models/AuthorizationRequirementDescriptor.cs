using Microsoft.AspNetCore.Authorization;

namespace ApiExplorerApp.Explorer.Models;

public class RequirementDescriptor(IAuthorizationRequirement requirement)
{
    public string? Name { get; set; } = requirement.GetType().Name;
    public string? Description { get; set; } = requirement.ToString();
}