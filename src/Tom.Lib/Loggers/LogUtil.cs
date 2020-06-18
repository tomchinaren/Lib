using System;

namespace Tom.Lib.Loggers
{
    public static class LogUtil
    {
        public static void LogInfo<T>(ILogger<T> logger, T message)
        {
            try
            {
                logger.Log(message);
            }
            catch(Exception ex)
            {
                Console.WriteLine("LogUtil.LogInfo<T> error:" + ex);
            }
        }
    }
}
