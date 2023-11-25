using System.Net.Http;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace IocLists
{
    internal static class API
    {
        public static async Task<HttpResponseMessage> Request
        (
            this HttpClient cl,
            HttpMethod method,
            string path)
        => await Request(cl, method, path, null);

        public static async Task<HttpResponseMessage> Request
        (
            this HttpClient cl,
            HttpMethod method,
            string path,
            object obj,
            JsonSerializerOptions options = null)
        => await Request(cl, method, path, await obj.Serialize(options ?? Constants.JsonOptions));

        public static async Task<HttpResponseMessage> Request
        (
            this HttpClient cl,
            HttpMethod method,
            string path,
            HttpContent content)
        {
            using HttpRequestMessage req = new(method, path)
            {
                Content = content
            };

            HttpResponseMessage res = await cl.SendAsync(req);
            content?.Dispose();

            if (res.StatusCode == HttpStatusCode.OK) return res;

            if (res.StatusCode == HttpStatusCode.Unauthorized)
                throw new IocListsException($"You are unauthorized, check your API key.", res);
            else if (res.StatusCode == HttpStatusCode.TooManyRequests)
                throw new IocListsException($"You are sending too many requests to the API.", res);
            else if (res.StatusCode == HttpStatusCode.BadRequest)
            {
                IocListsError err = await res.Deseralize<IocListsError>();

                throw new IocListsException($"Failed to request {method} {path}, received status {err.Status} and message \"{err.Message}\"", res);
            }
            else
            {
                throw new IocListsException($"Failed to request {req.RequestUri}, received status code {res.StatusCode}\nPreview: {await res.GetPreview()}", res);
            }
        }
    }
}