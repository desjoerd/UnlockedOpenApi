using System.Text.Json.Serialization.Metadata;

using Microsoft.AspNetCore.OpenApi;

namespace VersionedUnlocked.Api.OpenApi;

public static class OpenApiLoggingTransformers
{
    public static void AddLoggingOpenApiTransformers(this OpenApiOptions options)
    {
        const bool LogSchemaReferenceId = true;
        const bool LogNullSchemaReferenceId = true;

        var prevShouldInclude = options.ShouldInclude;
        options.ShouldInclude = apiDescription =>
        {
            var result = prevShouldInclude(apiDescription);
            Console.WriteLine($"[Doc: {options.DocumentName}] ShouldInclude: {apiDescription.ActionDescriptor.DisplayName}, Result: {result}");
            return result;
        };

        var prevCreateSchemaReferenceId = options.CreateSchemaReferenceId;
        options.CreateSchemaReferenceId = (jsonTypeInfo) =>
        {
            var result = prevCreateSchemaReferenceId(jsonTypeInfo);

            if (LogSchemaReferenceId && (LogNullSchemaReferenceId || result != null))
            {
                Console.WriteLine($"[Doc: {options.DocumentName}] CreateSchemaReferenceId: {jsonTypeInfo.Type}, Result: {result ?? "null"}");
            }
            return result;
        };

        options.AddSchemaTransformer((schema, context, cancel) =>
        {
            string typeWithProp = string.Empty;
            string declaringType = string.Empty;
            if (context.JsonPropertyInfo != null)
            {
                declaringType = context.JsonPropertyInfo.DeclaringType.Name;
                typeWithProp = $" ({context.JsonPropertyInfo.DeclaringType.Name}.{context.JsonPropertyInfo.Name})";
            }

            Console.WriteLine($"[Doc: {context.DocumentName}] Schema Transformer: {context.JsonTypeInfo.Type.Name.PadRight(declaringType.Length)}{typeWithProp}");

            return Task.CompletedTask;
        });

        options.AddOperationTransformer((operation, context, cancel) =>
        {
            Console.WriteLine($"[Doc: {context.DocumentName}] Operation Transformer: {context.Description.HttpMethod} /{context.Description.RelativePath}");
            return Task.CompletedTask;
        });

        options.AddDocumentTransformer((doc, context, cancel) =>
        {
            Console.WriteLine($"[Doc: {context.DocumentName}] Document Transformer: {doc.Info.Title}");
            return Task.CompletedTask;
        });
    }


}