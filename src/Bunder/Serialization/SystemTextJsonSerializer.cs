using System.Text.Json;

namespace Bunder
{
    /// <summary>
    /// Serializer implementation that uses Microsofts System.Text.Json serializer.
    /// </summary>
    public class SystemTextJsonSerializer : ISerializer
    {
        public T Deserialize<T>(string json) where T : class
        {
            return JsonSerializer.Deserialize<T>(json);
        }
    }
}
