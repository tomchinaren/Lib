using System.Net.Http;
using System.Text;

namespace Tom.Lib.HttpClientProxy
{
    public class DefaultAPIClient : IAPIClient
    {
        #region
        private static HttpClient httpClient = new HttpClient();
        #endregion

        public T Execute<T>(IRequest<T> request) where T : Response
        {
            return Post(request);
        }

        protected T Post<T>(IRequest<T> request) where T : Response
        {
            var requestString = Newtonsoft.Json.JsonConvert.SerializeObject(request);
            var requestContent = new StringContent(requestString, Encoding.UTF8, "application/json");
            var task = httpClient.PostAsync(request.GetApiName(), requestContent);
            
            var response = task.ConfigureAwait(false).GetAwaiter().GetResult();
            var responseContent = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();

            return Parse<T>(responseContent);
        }

        protected T Parse<T>(string json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }

    }
}
