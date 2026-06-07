namespace SavingTracker.UI.Services
{
    public class AppCancellationService
    {
        private CancellationTokenSource _cts = new CancellationTokenSource();

        public CancellationToken Token => _cts.Token;

        public void CancelAll()
        {
            _cts.Cancel();
            _cts = new CancellationTokenSource();
        }
    }
}
