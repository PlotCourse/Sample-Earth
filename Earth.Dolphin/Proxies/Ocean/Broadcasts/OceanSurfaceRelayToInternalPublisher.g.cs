using Earth.Shared.Data.Mappers;
using ContractDataType_HurricaneCoordinate = Earth.Ocean.Contract.Data.HurricaneCoordinate;
using InternalDataType_HurricaneCoordinate = Earth.Dolphin.Data.HurricaneCoordinate;
using Ocean_IOceanSurfacePublisher = Earth.Ocean.Contract.Broadcasts.IOceanSurfacePublisher;

namespace Earth.Dolphin.Proxies.Ocean.Broadcasts;

/// <summary>
/// The relay is a broadcast subscriber of the broadcast component's publisher singleton
/// (sourcePublisher) that maps record types from the broadcast component's contract types
/// to the record types used locally in this component and relays the mapped message to a
/// singleton publisher specific to this component and containing as many local subscribers
/// as needed.
/// 
/// Note that the sourcePublisher has been set up for DI by the Ocean component
/// to resolve to it's singleton publisher and since Earth.Dolphin is running in the same
/// Web API as Ocean, that's the instance that's injected here:
///     Earth.Ocean.Broadcasts.OceanSurfacePublisher
/// </summary>
internal partial class OceanSurfaceRelayToInternalPublisher(
    Ocean_IOceanSurfacePublisher sourcePublisher,
    Interfaces.IOceanSurfaceInternalPublisher destinationPublisher)
    : Earth.Ocean.Client.Broadcasts.OceanSurfaceSubscriber(sourcePublisher),
    Interfaces.IOceanSurfaceRelayToInternalPublisher
{
    public override int Temperature
    {
        get => sourcePublisher.Temperature;
        set
        {
            destinationPublisher.Temperature = value;
        }
    }

    public override void NewHurricane(
        ContractDataType_HurricaneCoordinate location)
    {
        destinationPublisher.NewHurricane(
            location?.Map<ContractDataType_HurricaneCoordinate, InternalDataType_HurricaneCoordinate>());
    }
}
