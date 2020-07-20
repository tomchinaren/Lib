namespace Tom.Lib.Observers
{
    public interface IDequeuable<T>
    {
        bool TryDequeue(out T message);
    }
}
