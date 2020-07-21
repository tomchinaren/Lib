namespace Tom.Lib.Observers
{
    public interface IReader<TData>
    {
        bool ReadNext(out TData data);
    }
}
