namespace Earth.Main.Base;

internal abstract partial class BaseClientSetupHelper
{
    protected IServiceCollection _services;

    public BaseClientSetupHelper(IServiceCollection services)
    {
        _services = services;
    }

    public virtual void AddClientDependencies()
    {
        AddOceanClientDependencies();
    }
    public virtual void AddOceanClientDependencies()
    {
        var helper = new Earth.Ocean.Client.ClientSetupHelper(_services);
        helper.AddClientDependencies();
    }
}
