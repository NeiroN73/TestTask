namespace Game.Components
{
    public abstract class ControllerElement : ClientNetworkElement
    {
        public Observer DebugPerformed { get; } = new();
        public Observer SpawnPerformed { get; } = new();
    }
}