using System;
using System.Net;
using System.Net.Http;

namespace IocLists
{
    /// <summary>
    /// An exception specific to IOC Lists. When caused by a HTTP request, you can access the exception's properties to get the context.
    /// </summary>
    public class IocListsException : Exception
    {
        /// <summary>
        /// The HTTP request method that triggered this exception.
        /// </summary>
        public HttpMethod Method { get; set; }
        /// <summary>
        /// The HTTP path that triggered this exception.
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// The HTTP status code that triggered this exception.
        /// </summary>
        public HttpStatusCode? StatusCode { get; set; }

        public IocListsException(string message) : base(message) { }
        public IocListsException(string message, HttpResponseMessage res) : base(message)
        {
            Method = res.RequestMessage.Method;
            Path = res.RequestMessage.RequestUri.AbsolutePath;
            StatusCode = res.StatusCode;
        }
        public IocListsException(string message, HttpMethod method, string path, HttpStatusCode status) : base(message)
        {
            Method = method;
            Path = path;
            StatusCode = status;
        }
    }
}