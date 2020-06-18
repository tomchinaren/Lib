namespace Tom.Lib.Loggers
{

    public interface ILogger<T>: ILogger
    {
        void Log(T message);
    }

    public interface ILogger
    {
        void Log();
    }
}
