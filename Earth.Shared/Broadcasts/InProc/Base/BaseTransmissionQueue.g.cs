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
using System.Collections.Concurrent;

namespace Earth.Shared.Broadcasts.InProc.Base;

/// <summary>
/// Guarantees actions are run in the order received in this queue but without
/// blocking the thread that initiates processing.  Used by the Publisher per
/// subscriber so that actions can be quickly queued for all subscribers and processed
/// separately as fast as each subscriber will allow them to be synchronously called
/// and without any subscriber blocking the processing of actions for other subscribers.
/// 
/// See also Publisher.
/// </summary>
public abstract class BaseTransmissionQueue : ConcurrentQueue<Action>
{
    protected int _processing = 0;

    public void Process()
    {
        if (Count == 0)
        {
            return;
        }

        if (0 == Interlocked.Exchange(ref _processing, 1))
        {
            Task.Run(() =>
            {
                while (TryDequeue(out Action action))
                {
                    action();
                }

                Interlocked.Exchange(ref _processing, 0);

                //Need to check for any new actions added to the queue after the loop
                //above but before turning off the processing flag in the line above.
                Process();
            });
        }
    }
}
