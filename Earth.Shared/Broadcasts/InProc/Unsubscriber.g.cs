using Earth.Shared.Broadcasts.InProc.Base;

namespace Earth.Shared.Broadcasts.InProc;

internal partial class Unsubscriber<T> : BaseUnsubscriber<T>
{
    public Unsubscriber(
        HashSet<T> allSubscribers,
        Dictionary<int, TransmissionQueue> allQueues,
        T subscriber) : base(allSubscribers, allQueues, subscriber) { }
}
