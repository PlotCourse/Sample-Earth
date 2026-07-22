using Earth.Shared.Broadcasts.Remote.Base;

namespace Earth.Shared.Broadcasts.Remote;

public partial class Client<TNotificationType> : BaseClient<TNotificationType>
    where TNotificationType : Enum
{
    public Client(string clientId, string connectionId) : base(clientId, connectionId) { }
}
