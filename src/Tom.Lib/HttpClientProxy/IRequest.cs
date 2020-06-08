namespace Tom.Lib.HttpClientProxy
{
    /// <summary>
    /// 请求
    /// </summary>
    public interface IRequest<T> where T : Response
    {
        /// <summary>
        /// 获取API名称(目前为路由地址)。
        /// </summary>
        string GetApiName();

        /// <summary>
        /// Get/Post
        /// </summary>
        /// <returns></returns>
        string GetMethod();
    }

    public  static class HttpMethod
    {
        public static string GET = "GET";
        public static string POST = "POST";
    }
}
