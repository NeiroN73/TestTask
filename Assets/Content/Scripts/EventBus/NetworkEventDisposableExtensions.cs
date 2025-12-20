namespace Content.Scripts.EventBus
{
    public static class NetworkEventDisposableExtensions
    {
        public static void AddTo(this NetworkEventHandler handler, NetworkEventDisposable disposable)
        {
            disposable.Add(handler);
        }
    }
}