using System.Text.Json;

namespace Bunder
{
    /// <summary>
    /// Serializer implementation that uses Microsofts System.Text.Json serializer.
    /// </summary>
    internal class SystemTextJsonSerializer : ISerializer
    {
        private readonly JsonSerializerOptions _options;

        public SystemTextJsonSerializer(JsonSerializerOptions options)
        {
            _options = options;
        }

        public T? Deserialize<T>(string json) where T : class
        {
            return JsonSerializer.Deserialize<T>(json, _options);
        }
    }
}
