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
        public HttpMethod Method { get; set; }
        public string Path { get; set; }
        public HttpStatusCode? StatusCode { get; set; }

        public IocListsException(string message) : base(message) { }
        public IocListsException(string message, HttpMethod method, string path, HttpStatusCode status) : base(message)
        {
            Method = method;
            Path = path;
            StatusCode = status;
        }
    }
}