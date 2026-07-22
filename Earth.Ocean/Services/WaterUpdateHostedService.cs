using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Earth.Ocean.Services.Interfaces;

namespace Earth.Ocean.Services;

//DEV_MANAGED_CODE_EXAMPLE: a background worker that periodically calls the water service to update its state
// The broadcast publisher is also a singleton so it could be injected here for direct publication by this class.
// Instead, just to make the example a bit more interesting, we go through the internal scoped service since it's
// already configured as a publisher (in DoWork() below).

internal class WaterUpdateHostedService : BackgroundService
{
    private readonly ILogger<WaterUpdateHostedService> _logger;
    private readonly IServiceProvider _services;

    public WaterUpdateHostedService(IServiceProvider services, ILogger<WaterUpdateHostedService> logger)
    {
        _services = services;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("WaterUpdateHostedService running.");

        await DoWork();

        using PeriodicTimer timer = new(TimeSpan.FromSeconds(5));

        try
        {
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                await DoWork();
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("WaterUpdateHostedService is stopping.");
        }
    }

    private async Task DoWork()
    {
        using var scope = _services.CreateScope();
        var water = scope.ServiceProvider.GetRequiredService<IWater>();

        await water.UpdateWaterState(null);
    }
}