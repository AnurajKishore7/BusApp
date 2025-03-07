using System.Text.Json;
using System.Text.Json.Serialization;

namespace BusApp.Converters
{
    public class TimeSpanConverter : JsonConverter<TimeSpan>
    {
        public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Parse the JSON object { "hours": x, "minutes": y }
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException("Expected StartObject token");
            }

            int hours = 0;
            int minutes = 0;

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    break;
                }

                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException("Expected PropertyName token");
                }

                string propertyName = reader.GetString();
                reader.Read();

                switch (propertyName)
                {
                    case "hours":
                        hours = reader.GetInt32();
                        break;
                    case "minutes":
                        minutes = reader.GetInt32();
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }

            return new TimeSpan(hours, minutes, 0); // Assuming seconds are 0
        }

        public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteNumber("hours", value.Hours);
            writer.WriteNumber("minutes", value.Minutes);
            writer.WriteEndObject();
        }
    }
}