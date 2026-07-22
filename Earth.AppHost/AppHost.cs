using Earth.AppHost;
var builder = DistributedApplication.CreateBuilder(args);

builder.AddProjects();


builder.Build().Run();
