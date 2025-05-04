using Projects;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Unlocked_Api>("api")
    .WithExternalHttpEndpoints();

builder.Build().Run();
