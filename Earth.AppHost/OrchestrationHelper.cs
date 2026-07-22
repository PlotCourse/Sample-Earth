namespace Earth.AppHost;

internal static class OrchestrationHelperExtension
{
    public static void AddProjects(this IDistributedApplicationBuilder builder)
    {
        var helper = new OrchestrationHelper(builder);

        //Note: The departure from the extension method pattern here is intended to allow an easy
        //  override of any part of the orchestration setup.
        helper.AddProjects();
    }
}

/// <summary>
/// This class can be used for auto-generated Web API dependency structure based on component
/// dependency structure defined in PlotStax.
/// 
/// To use this class, remove all calls for PlotStax Web APIs in Program.cs to
/// "builder.AddProject" and replace with this to call the method above:
/// 
///     builder.AddProjects();
///     
/// If this class is not used, the names of the project resources should still match those
/// used by this class because the names are used in other places in generated code.  If
/// different names are desired these can be changed in the PlotStax tool using:
///     WebApiBuilder.WithCustomResourceName(...)
/// </summary>
internal class OrchestrationHelper : BaseOrchestrationHelper
{
    public OrchestrationHelper(IDistributedApplicationBuilder builder) : base(builder) { }

}
