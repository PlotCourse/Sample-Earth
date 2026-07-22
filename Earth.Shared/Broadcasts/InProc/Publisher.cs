using Earth.Shared.Broadcasts.InProc.Base;
using Earth.Shared.Broadcasts.InProc.Interfaces;

namespace Earth.Shared.Broadcasts.InProc;

/// <summary>
/// An in-process publisher for broadcasts that supports calls like:
///     DoBroadcast(r => r.MyMethod(data));
/// Where "MyMethod" is defined on the subscribers; the inner type.
/// 
/// DoBroadcast is designed to quickly return control to the calling
/// thread but to still allow the controlling thread to rely on:
/// 
/// 1) Broadcasts will be handled in the correct order per subscriber, and
/// 2) A broadcast to a subscriber will not start until the subscriber has
///     returned from the previous broadcast.
/// 
/// The caller of DoBroadcast can rely on these requirements while
/// making rapid additional calls to DoBroadcast while any subscriber may
/// be slowly handling these calls in a separate thread.
/// 
/// See also TransmissionQueue.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class Publisher<TSubscriber> : BasePublisher<TSubscriber>, IPublisher<TSubscriber>
{
}
