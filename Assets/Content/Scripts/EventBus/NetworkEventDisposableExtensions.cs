namespace Content.Scripts.EventBus
{
    public static class NetworkEventDisposableExtensions
    {
        public static void AddDisposable(this NetworkEventHandler handler, NetworkEventDisposable disposable)
        {
            disposable.Add(handler);
        }
    }
}