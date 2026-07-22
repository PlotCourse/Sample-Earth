using Earth.Shared.Broadcasts.Remote.Base;
using Earth.Shared.Broadcasts.Remote.Interfaces;

namespace Earth.Shared.Broadcasts.Remote;

public partial class RetryPolicy : BaseRetryPolicy, IRetryPolicy
{
    public RetryPolicy(int[] intervalsInSeconds, bool neverGiveUp) : base(intervalsInSeconds, neverGiveUp) { }
}
