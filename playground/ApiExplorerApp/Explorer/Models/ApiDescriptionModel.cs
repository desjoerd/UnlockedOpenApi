using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace ApiExplorerApp.Explorer.Models;

public class ApiDescriptionModel
{
    private ApiDescriptionModel()
    {
    }

    public string? HttpMethod { get; set; }
    public string? RelativePath { get; set; }
    public required IEnumerable<MetadataModel> Metadata { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public AuthorizationDescriptor? Authorization { get; set; }

    public static async Task<ApiDescriptionModel> Create(IAuthorizationPolicyProvider? policyProvider, ApiDescription description)
    {
        return new ApiDescriptionModel
        {
            HttpMethod = description.HttpMethod,
            RelativePath = description.RelativePath,
            Metadata = description.ActionDescriptor.EndpointMetadata.Select(x => new MetadataModel(x)),
            Authorization = await AuthorizationDescriptor.Create(policyProvider, description),
        };
    }
}