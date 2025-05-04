# Unlocked Api

## Table of Contents
- [Unlocked Api](#unlocked-api)
  - [Table of Contents](#table-of-contents)
  - [Generate OpenAPI on build](#generate-openapi-on-build)
    - [Remarks](#remarks)
  - [OpenApi schema possible mismatches](#openapi-schema-possible-mismatches)
    - [MVC Controllers](#mvc-controllers)
    - [`null` is valid on non-nullable reference types, unless](#null-is-valid-on-non-nullable-reference-types-unless)
    - [Constructor parameters are optional by default](#constructor-parameters-are-optional-by-default)
    - [Required properties](#required-properties)
    - [DataAnnotations with Minimal APIs before .NET 10](#dataannotations-with-minimal-apis-before-net-10)
  - [.NET 10 Preview 3 bugs](#net-10-preview-3-bugs)
  - [Describing API Responses](#describing-api-responses)
  - [Enums](#enums)
  - [Insert and reference custom schemas](#insert-and-reference-custom-schemas)
  - [Spectral OpenApi Linting](#spectral-openapi-linting)

## Generate OpenAPI on build

See: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/openapi/aspnetcore-openapi?view=aspnetcore-10.0&tabs=net-cli%2Cvisual-studio-code#generate-openapi-documents-at-build-time

```
dotnet add package Microsoft.Extensions.ApiDescription.Server
```

Settings in `csproj`:
```xml
<PropertyGroup>
  <OpenApiDocumentsDirectory>.</OpenApiDocumentsDirectory>
  <OpenApiGenerateDocumentsOptions>--file-name openapi</OpenApiGenerateDocumentsOptions>
</PropertyGroup>
```

### Remarks

.yaml on build is not yet supported in .NET 10-preview-3

The default document is `v1`. This will be in the output without suffix.
See https://github.com/dotnet/aspnetcore/issues/58782.

Documents not matching `v1` will be in the output as `{file-name}_{doc-name}`. For example for `v2` and filename `openapi` the resulting file is `openapi_v2.json`, where the `v1` document will be called `openapi.json`.

## OpenApi schema possible mismatches

As OpenAPI generates schemas, they can give you a false sense of security. The generated schemas are not always correct.

### MVC Controllers

ASP.NET Core OpenApi will use the Http.JsonOptions as the json configuration for generating schemas. Where MVC has different JsonOptions, so when using MVC Controllers with custom JsonSerializerOptions the changes need to be replicated to the Http.JsonOptions.

### `null` is valid on non-nullable reference types, unless
The generated schemas will tell that `null` is not valid on non-nullable reference types. This is correct, but System.Text.Json does not enforce this by default. You can set the `JsonSerializerOptions` to enforce this.

Global:
```xml
<ItemGroup>
  <RuntimeHostConfigurationOption Include="System.Text.Json.Serialization.RespectNullableAnnotationsDefault" Value="true" />
</ItemGroup>
```

Local:
```csharp
JsonSerializerOptions options = new()
{
    RespectNullableAnnotations = true
};
```

### Constructor parameters are optional by default
Record/class constructor parameters are optional by System.Text.Json (resulting in the default). The schema will show that the constructor parameters are required, but the deserialization will not enforce this. You can set the `JsonSerializerOptions` to enforce this.

Global:
```xml
<ItemGroup>
  <RuntimeHostConfigurationOption Include="System.Text.Json.Serialization.RespectRequiredConstructorParametersDefault" Value="true" />
</ItemGroup>
```

Local:
```csharp
JsonSerializerOptions options = new()
{
    RespectRequiredConstructorParameters = true
};
```

### Required properties
This works as expected. Both the schema will show that it's required and the deserialization will enforce this.

### DataAnnotations with Minimal APIs before .NET 10

DataAnnotations are used by OpenApi to enrich the schema. But by default, Minimal APIs do NOT Validate. In .NET 10 you can enable this.

From release notes preview 3: https://github.com/dotnet/core/blob/main/release-notes/10.0/preview/preview3/aspnetcore.md#validation-support-in-minimal-apis

csproj:
```xml
<PropertyGroup>
  <!-- Enable the generation of interceptors for the validation attributes -->
  <InterceptorsNamespaces>$(InterceptorsNamespaces);Microsoft.AspNetCore.Http.Validation.Generated</InterceptorsNamespaces>
</PropertyGroup>
```

Program.cs:
```csharp
builder.Services.AddValidation();
```

## .NET 10 Preview 3 bugs

Minimal api validation is not working for records: https://github.com/dotnet/aspnetcore/issues/61379.


## Describing API Responses

Explicit Metadata Added to the builder

```csharp
.Produces<UnlockResponse>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound);
```

Metadata from Results<T>:

```csharp
Results<Ok<UnlockResponse>, NotFound>
```

## Enums

Enums by default are serialized as numbers. Configure in JsonSerializerOptions (Http.JsonOptions).


## Insert and reference custom schemas

See community standup -> https://youtu.be/oqAowFs5zZQ?t=3176


## Spectral OpenApi Linting

Install with Node
```sh
npm install --save -D @stoplight/spectral-cli
npm install --save -D @stoplight/spectral-url-versioning
```

.spectral.yml
```yaml
extends: ["spectral:oas", "@stoplight/spectral-url-versioning"]
```