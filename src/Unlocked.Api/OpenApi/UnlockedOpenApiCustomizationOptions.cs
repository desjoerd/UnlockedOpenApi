namespace Unlocked.Api.OpenApi;

public class UnlockedOpenApiCustomizationOptions
{
    public IDictionary<string, string> Servers { get; private set; } = new SortedDictionary<string, string>(StringComparer.InvariantCulture);
}