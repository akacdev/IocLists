using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace IocLists
{
    public class EnumNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name) => name.ToUpper();
    }

    public class DoubleUnixConverter : JsonConverter<DateTime>
    {
        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
            => writer.WriteNumberValue(value.ToUnix());

        public override DateTime Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
        {
            return reader.TokenType switch
            {
                JsonTokenType.Number => reader.GetDouble().ToDate(),
                _ => throw new JsonException(),
            };
        }

    }
}