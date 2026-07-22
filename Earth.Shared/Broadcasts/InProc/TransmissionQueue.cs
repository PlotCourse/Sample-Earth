using Earth.Shared.Broadcasts.InProc.Base;

namespace Earth.Shared.Broadcasts.InProc;

/// <summary>
/// Guarantees actions are run in the order received in this queue but without
/// blocking the thread that initiates processing.  Used by the Publisher per
/// subscriber so that actions can be quickly queued for all subscribers and processed
/// separately as fast as each subscriber will allow them to be synchronously called
/// and without any subscriber blocking the processing of actions for other subscribers.
/// 
/// See also Publisher.
/// </summary>
public class TransmissionQueue : BaseTransmissionQueue
{
}
