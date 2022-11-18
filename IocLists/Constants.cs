using System;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace IocLists
{
    internal static class Constants
    {
        public const int Version = 1;
        public static readonly Uri BaseUri = new($"https://api.ioclists.com/v{Version}/");
        public const string UserAgent = "IocLists C# Library - actually-akac/IocLists";
        public const string AcceptedEncoding = "gzip, deflate, br";

        public const string JsonContentType = "application/json";
        public const string PlainTextContentType = "text/plain";

        public const int MaxPreviewLength = 500;

        public static readonly JsonSerializerOptions JsonOptions = new()
        {
            Converters =
            {
                new JsonStringEnumConverter(new EnumNamingPolicy())
            }
        };
    }
}