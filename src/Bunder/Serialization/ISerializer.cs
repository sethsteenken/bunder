namespace Bunder
{
    /// <summary>
    /// Serializing abstraction for reading JSON configuration files.
    /// </summary>
    public interface ISerializer
    {
        /// <summary>
        /// Convert raw JSON string to generic class type.
        /// </summary>
        /// <typeparam name="T">Generic type class.</typeparam>
        /// <param name="json">Raw JSON string.</param>
        /// <returns></returns>
        T Deserialize<T>(string json) where T : class;
    }
}
