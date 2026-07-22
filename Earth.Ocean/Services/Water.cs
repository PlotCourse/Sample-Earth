using Earth.Ocean.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Earth.Ocean.Services;

internal partial class Water : IWater
{
    //DEV_MANAGED_CODE_EXAMPLE: implement a method that updates the status of the water using the Ocean "Surface" broadcast
    private static readonly int[] variousTemperatures = [ 63, 62, 64, 65 ];
    private static int updateCounter = 0;

    public override Task UpdateWaterState(HttpContext httpContext)
    {
        updateCounter++;
        
        var temperatureIx = updateCounter % 3;
        _oceanOceanSurfacePublisher.Temperature = variousTemperatures[temperatureIx];
        
        if (updateCounter % 10 == 5)
        {
            var x = Random.Shared.Next(1, 100);
            var y = Random.Shared.Next(1, 100);
            var hurricaneCoord = new Data.HurricaneCoordinate(updateCounter, x, y);

            _oceanOceanSurfacePublisher.NewHurricane(hurricaneCoord);
        }

        return Task.CompletedTask;
    }
}
