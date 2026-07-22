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
using Earth.Shared.Data.Broadcasts;

namespace Earth.Shared.Broadcasts.Remote.Base;

public abstract class BaseClientSequenceTracker<TNotificationType>
     where TNotificationType : Enum
{
    protected ConcurrentDictionary<TNotificationType, CurrentSequenceNumber> _currentByNotificationType;

    public virtual void Reset(SequenceTracking<TNotificationType>[] trackings)
    {
        _currentByNotificationType = new ConcurrentDictionary<TNotificationType, CurrentSequenceNumber>(
            trackings.Select(s => new KeyValuePair<TNotificationType, CurrentSequenceNumber>(
                s.NotificationType, new CurrentSequenceNumber(s.SequenceNumber))));
    }

    public virtual int? UpdateSequenceNumber(TNotificationType notificationType, int sequenceNumber)
    {
        if (_currentByNotificationType == null)
        {
            _currentByNotificationType = new ConcurrentDictionary<TNotificationType, CurrentSequenceNumber>();
        }

        if (!_currentByNotificationType.ContainsKey(notificationType))
        {
            _currentByNotificationType.TryAdd(notificationType, new CurrentSequenceNumber(sequenceNumber));
            return null;
        }

        return _currentByNotificationType[notificationType].Update(sequenceNumber);
    }
}
