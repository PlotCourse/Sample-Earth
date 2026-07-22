
using Earth.Shared.Broadcasts.Remote.Base;

namespace Earth.Shared.Broadcasts.Remote;

public partial class ConfirmationTimeoutOptions : BaseConfirmationTimeoutOptions
{
    public ConfirmationTimeoutOptions(TimeSpan timeLimit, int retries) : base(timeLimit, retries) { }
}
