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

public abstract class BaseNotification<TNotificationType> where TNotificationType : Enum
{
    protected string _notificationId;
    protected string _method;
    protected object[] _args;
    protected TNotificationType _notificationType;

    public virtual string NotificationId => _notificationId;
    public virtual string Method => _method;
    public virtual object[] Args => _args;
    public virtual TNotificationType NotificationType => _notificationType;

    public BaseNotification(
        string notificationId,
        string method,
        object[] args,
        TNotificationType notificationType)
    {
        _notificationId = notificationId;
        _method = method;
        _args = args;
        _notificationType = notificationType;
    }
}
