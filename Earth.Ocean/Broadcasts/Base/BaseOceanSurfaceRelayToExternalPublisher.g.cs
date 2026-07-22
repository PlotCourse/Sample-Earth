using Earth.Shared.Data.Mappers;
using Earth.Ocean.Broadcasts.Interfaces;
using ContractDataType_HurricaneCoordinate = Earth.Ocean.Contract.Data.HurricaneCoordinate;
using InternalDataType_HurricaneCoordinate = Earth.Ocean.Data.HurricaneCoordinate;

namespace Earth.Ocean.Broadcasts.Base;

/// <summary>
/// Note that this singleton is used as needed by code inside this component to
/// update the root publisher for the broadcast.
/// </summary>
internal abstract partial class BaseOceanSurfaceRelayToExternalPublisher(
    IOceanSurfaceInternalPublisher sourcePublisher,
    IOceanSurfaceExternalPublisher externalPublisher)
    : BaseOceanSurfaceInternalSubscriber(sourcePublisher), IOceanSurfaceRelayToExternalPublisher
{
    public override int Temperature
    {
        get => _publisher.Temperature;
        set
        {
            externalPublisher.Temperature = value;
        }
    }


    public override void NewHurricane(
        InternalDataType_HurricaneCoordinate location)
    {
        externalPublisher.NewHurricane(
            location?.Map<InternalDataType_HurricaneCoordinate, ContractDataType_HurricaneCoordinate>());
    }
}
