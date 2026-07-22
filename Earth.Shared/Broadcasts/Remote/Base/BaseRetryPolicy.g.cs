/*
MIT License

Copyright (c) 2025-2026 PlotCourse LLC

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
namespace Earth.Shared.Broadcasts.Remote.Base;

public abstract class BaseRetryPolicy
{
    /// <summary>
    /// a dynamic policy configured with the parameters here.
    /// </summary>
    /// <param name="intervalsInSeconds">intervals in the order they should be used per retry.</param>
    /// <param name="neverGiveUp">
    /// If true NextRetryDelay will always return a timespan.
    /// When the intervalsInSeconds have all been used, the last value will be reused indefinitely.
    /// </param>
    public BaseRetryPolicy(int[] intervalsInSeconds, bool neverGiveUp)
    {
        IntervalsInSeconds = intervalsInSeconds;
        NeverGiveUp = neverGiveUp;
    }

    public int[] IntervalsInSeconds { get; }
    public bool NeverGiveUp { get; }

    protected int next = 0;

    public virtual void Reset()
    {
        next = 0;
    }

    public virtual TimeSpan? NextRetryDelay()
    {
        var allUsed = next == IntervalsInSeconds.Length;

        if (allUsed && !NeverGiveUp)
        {
            return null;
        }

        var ix = allUsed
            ? IntervalsInSeconds.Length - 1
            : next;

        if (next < IntervalsInSeconds.Length)
        {
            next++;
        }

        return TimeSpan.FromSeconds(IntervalsInSeconds[ix]);
    }
}
