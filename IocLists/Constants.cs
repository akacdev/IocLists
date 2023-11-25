using System;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace IocLists
{
    internal static class Constants
    {
        /// <summary>
        /// The version of the API to send requests to.
        /// </summary>
        public const int Version = 1;
        /// <summary>
        /// The base URI to send requests to.
        /// </summary>
        public static readonly Uri BaseUri = new($"https://api.ioclists.com/v{Version}/");
        /// <summary>
        /// The preferred HTTP request version to use.
        /// </summary>
        public static readonly Version HttpVersion = new(2, 0);
        /// <summary>
        /// The <c>User-Agent</c> header value to send along requests.
        /// </summary>
        public const string UserAgent = "IocLists C# Library - actually-akac/IocLists";
        /// <summary>
        /// The maximum string length when displaying a preview of a response body.
        /// </summary>
        public const int PreviewMaxLength = 500;

        /// <summary>
        /// The default JSON options object with an uppercase naming policy.
        /// </summary>
        public static readonly JsonSerializerOptions JsonOptions = new()
        {
            Converters =
            {
                new JsonStringEnumConverter(new EnumNamingPolicy())
            }
        };
    }
}