using Earth.Shared.Data.Broadcasts.Base;
namespace Earth.Shared.Data.Broadcasts;

public partial class ConfirmationExpectation : BaseConfirmationExpectation
{
    public ConfirmationExpectation(string notificationId, bool confirmAsync) : base(notificationId, confirmAsync) { }
}
