namespace Earth.Shared.Broadcasts.InProc.Interfaces;

public partial interface IPublisher<T>
{
    IDisposable Subscribe(T subscriber);
}
