using PlotStax.Gen.Client;

var initializer = new Initializer<Projects.Earth_AppHost>("Earth");
initializer
    .UseAllAvailableProjectsAsWebApis()
    .WriteFiles();