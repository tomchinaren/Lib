namespace Tom.Lib.Observers
{
    public interface IReader<TData>
    {
        TData ReadNext();
    }
}
