using System.Text.Json.Serialization;

using Asp.Versioning;

using Microsoft.AspNetCore.Authorization;

namespace ApiExplorerApp.Explorer.Models;

public class MetadataModel
{
    public MetadataModel(object x)
    {
        Name = x.GetType().Name;
        Summary = x.ToString();
        Description = x switch
        {
            IAuthorizeData authorizeData => $"Policy :{authorizeData.Policy}, Roles: {string.Join(", ", authorizeData.Roles)}, AuthenticationSchemes: {string.Join(", ", authorizeData.AuthenticationSchemes)}",
            ApiVersionMetadata apiVersionMetadata => $"Explicit Versions: {string.Join(", ", apiVersionMetadata.Map(ApiVersionMapping.Explicit).DeclaredApiVersions)}",
            _ => null
        };
        Interfaces = x.GetType().GetInterfaces().Select(x => x.Name).ToArray();
    }

    public string Name { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Summary { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Description { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? Interfaces { get; set; }
}