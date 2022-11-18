using System;
using System.IO;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IocLists
{
    public static class API
    {
        public static async Task<HttpResponseMessage> Request
        (
            this HttpClient cl,
            HttpMethod method,
            string path,
            string expectedContentType = Constants.JsonContentType)
        => await Request(cl, method, path, null, expectedContentType);

        public static async Task<HttpResponseMessage> Request
        (
            this HttpClient cl,
            HttpMethod method,
            string path,
            object obj,
            JsonSerializerOptions options = null)
        => await Request(cl, method, path, new StringContent(JsonSerializer.Serialize(obj, options ?? Constants.JsonOptions), Encoding.UTF8, Constants.JsonContentType));

        public static async Task<HttpResponseMessage> Request
        (
            this HttpClient cl,
            HttpMethod method,
            string path,
            HttpContent content,
            string expectedContentType = Constants.JsonContentType)
        {
            HttpRequestMessage req = new(method, path)
            {
                Content = content
            };

            HttpResponseMessage res = await cl.SendAsync(req);

            if (res.StatusCode == HttpStatusCode.Unauthorized)
                throw new IocListsException($"You are unauthorized, check your API key.", method, path, res.StatusCode);
            else if (res.StatusCode == HttpStatusCode.TooManyRequests)
                throw new IocListsException($"You are sending too many requests to the API.", method, path, res.StatusCode);

            string contentType = res.Content.Headers.ContentType?.MediaType;
            if (string.IsNullOrEmpty(contentType))
                throw new IocListsException($"Response is missing a 'Content-Type' header.", method, path, res.StatusCode);
            if (contentType != expectedContentType)
                throw new IocListsException($"Response is not of Content-Type '{expectedContentType}'. Preview: {await res.GetPreview()}", method, path, res.StatusCode);

            if (res.StatusCode == HttpStatusCode.BadRequest)
            {
                IocListsError error = await res.Deseralize<IocListsError>();

                throw new IocListsException($"Response resulted in an error with status {error.Status} and message {error.Message}.", method, path, res.StatusCode);
            }
            else if (res.StatusCode != HttpStatusCode.OK)
                throw new IocListsException(
                    $"Failed to request {req.RequestUri}, received a failure status code: {res.StatusCode}\n" +
                    $"Preview: {await res.GetPreview()}",
                    method, path, res.StatusCode);

            return res;
        }

        public static async Task<T> Deseralize<T>(this HttpResponseMessage res, JsonSerializerOptions options = null)
        {
            Stream stream = await res.Content.ReadAsStreamAsync();
            if (stream.Length == 0)
                throw new IocListsException("Response content is empty, can't parse as JSON.",
                    res.RequestMessage.Method, res.RequestMessage.RequestUri.AbsolutePath, res.StatusCode);

            try
            {
                return await JsonSerializer.DeserializeAsync<T>(stream, options);
            }
            catch (Exception ex)
            {
                throw new IocListsException(
                    $"Exception while parsing JSON: {ex.GetType().Name} => {ex.Message}\n" +
                    $"Preview: {await res.GetPreview()}");
            }
        }

        public static async Task<string> GetPreview(this HttpResponseMessage res)
        {
            string text = await res.Content.ReadAsStringAsync();
            return text[..Math.Min(text.Length, Constants.MaxPreviewLength)];
        }
    }
}