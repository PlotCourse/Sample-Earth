using Shared_NotificationTypeDetails = Earth.Shared.Broadcasts.Remote.NotificationTypeDetails;
using Shared_NotificationCategory = Earth.Shared.Broadcasts.Remote.Base.NotificationCategory;
using ContractDataType_OceanSurfaceBroadcastObservables = Earth.Ocean.Contract.Data.OceanSurfaceBroadcastObservables;
using Earth.Ocean.Broadcasts.Interfaces;
using Earth.Ocean.Contract.Broadcasts;

using ContractDataType_HurricaneCoordinate = Earth.Ocean.Contract.Data.HurricaneCoordinate;

namespace Earth.Ocean.Broadcasts.Base;

/// <summary>
/// Routes messages and changes to observables from this component's singleton publisher to
/// clients outside this Web API by using a SignalR hub.
/// </summary>
internal abstract partial class BaseOceanSurfaceRelayToHub(
    IOceanSurfaceExternalPublisher sourcePublisher,
    Earth.Shared.Broadcasts.Remote.Interfaces.IPublisher<OceanSurfaceHub, ContractDataType_OceanSurfaceBroadcastObservables, OceanSurfaceNotificationType> remotePublisher)
    : Earth.Ocean.Client.Broadcasts.OceanSurfaceSubscriber(sourcePublisher)
{
    public override int Temperature
    {
        get => sourcePublisher.Temperature;
        set
        {
            remotePublisher.Send(
                "UpdateTemperature",
                new object[] { value },
                OceanSurfaceNotificationType.Observable_Temperature
            );
        }
    }

    public override void NewHurricane(
        ContractDataType_HurricaneCoordinate location)
    {
        remotePublisher.Send(
            "NewHurricane",
            new object[]
            {
                location
            },
            OceanSurfaceNotificationType.Message_NewHurricane
        );
    }


    public static Dictionary<OceanSurfaceNotificationType, Shared_NotificationTypeDetails> GetNotificationDetailsByType()
    {
        var detailsByType = new Dictionary<OceanSurfaceNotificationType, Shared_NotificationTypeDetails>();

        detailsByType.Add(
            OceanSurfaceNotificationType.Observable_Temperature,
            new Shared_NotificationTypeDetails(
                Shared_NotificationCategory.ObservablesUpdate));

        detailsByType.Add(
            OceanSurfaceNotificationType.Message_NewHurricane,
            new Shared_NotificationTypeDetails(
                Shared_NotificationCategory.Message));

        return detailsByType;
    }
}
