var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var api = builder.AddProject<Projects.API>("api")
    .WithReference(cache)
    .WaitFor(cache);

builder.AddNpmApp("angular-frontend", "../../Frontend/frontend")
    .WithReference(api)
    .WaitFor(api)
    .WithHttpEndpoint(env: "PORT")
    .WithNpmPackageInstallation();

builder.Build().Run();
