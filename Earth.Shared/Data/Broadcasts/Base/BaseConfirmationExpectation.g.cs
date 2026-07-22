using System.Text.Json.Serialization;

namespace Earth.Shared.Data.Broadcasts.Base;

/// <summary>
/// Passed to Broadcast subscribers when confirmation is needed.
/// </summary>
public class BaseConfirmationExpectation
{
    protected string _notificationId;
    protected bool _confirmAsync;

    [JsonPropertyName("notificationId")]
    public virtual string NotificationId => this._notificationId;
    
    [JsonPropertyName("confirmAsync")]
    public virtual bool ConfirmAsync => this._confirmAsync;

    public BaseConfirmationExpectation(string notificationId, bool confirmAsync)
    {
        this._notificationId = notificationId;
        this._confirmAsync = confirmAsync;
    }
}
