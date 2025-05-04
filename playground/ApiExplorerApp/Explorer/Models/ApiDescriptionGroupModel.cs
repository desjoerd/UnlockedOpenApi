namespace ApiExplorerApp.Explorer.Models;

public class ApiDescriptionGroupModel(string? groupName, IReadOnlyList<ApiDescriptionModel> items)
{
    public string? GroupName { get; set; } = groupName;
    public IReadOnlyList<ApiDescriptionModel> Items { get; set; } = items;
}