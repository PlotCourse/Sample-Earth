namespace Earth.Shared.Broadcasts.Remote.Interfaces;

public partial interface IRetryPolicy
{
    int[] IntervalsInSeconds { get; }
    bool NeverGiveUp { get; }

    void Reset();
    TimeSpan? NextRetryDelay();
}
