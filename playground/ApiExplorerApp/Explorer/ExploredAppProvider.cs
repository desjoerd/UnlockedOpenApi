
using ApiExplorerApp.Explorer.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Testing;

using Swashbuckle.AspNetCore.Cli;

namespace ApiExplorerApp.Explorer;

public class ExploredAppProvider
{
    private IServiceProvider? _exploredAppServiceProvider;

    private readonly object _lock = new();

    public IServiceProvider GetExploredAppServiceProvider()
    {
        if (_exploredAppServiceProvider != null)
        {
            return _exploredAppServiceProvider;
        }
        lock (_lock)
        {
            if (_exploredAppServiceProvider != null)
            {
                return _exploredAppServiceProvider;
            }

            _exploredAppServiceProvider = HostingApplication.GetServiceProvider(typeof(Unlocked.Api.Endpoints.GetLockById).Assembly);
        }

        return _exploredAppServiceProvider;
    }

    public async Task<List<ApiDescriptionGroupModel>> GetApiDescriptionGroups()
    {
        var exploredServiceProvider = GetExploredAppServiceProvider();

        var apiExplorer = exploredServiceProvider.GetRequiredService<IApiDescriptionGroupCollectionProvider>();
        var policyProvider = exploredServiceProvider.GetRequiredService<IAuthorizationPolicyProvider>();

        var result = new List<ApiDescriptionGroupModel>();

        foreach (var group in apiExplorer.ApiDescriptionGroups.Items)
        {
            var items = new List<ApiDescriptionModel>();
            foreach (var description in group.Items)
            {
                items.Add(await ApiDescriptionModel.Create(policyProvider, description));
            }

            result.Add(new ApiDescriptionGroupModel(group.GroupName, items));
        }

        return result;
    }
}