using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace Unlocked.Api.OpenApi;

public class SortResponsesOperationTransformer : IOpenApiOperationTransformer
{
    public Task TransformAsync(
        OpenApiOperation operation,
        OpenApiOperationTransformerContext context,
        CancellationToken cancellationToken)
    {
        var sortedResponses = operation.Responses
            .OrderBy(response => response.Key, StringComparer.OrdinalIgnoreCase);

        var newResponses = new OpenApiResponses();
        foreach (var response in sortedResponses)
        {
            newResponses.Add(response.Key, response.Value);
        }
        operation.Responses = newResponses;
        return Task.CompletedTask;
    }
}