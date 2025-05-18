& dotnet kiota generate `
  --openapi "../Unlocked.Api/openapi.json" `
  --language Csharp `
  --clean-output `
  --output "./output" `
  --namespace-name "Unlocked.Client" `
  --class-name "UnlockedApiClient"