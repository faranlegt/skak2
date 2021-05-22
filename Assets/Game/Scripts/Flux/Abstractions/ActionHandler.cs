namespace Game.Scripts.Flux.Abstractions
{
    public abstract class ActionHandler<T> where T : BoardAction
    {
        public abstract void Handle(T action);
    }
}