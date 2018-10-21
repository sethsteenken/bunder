using Microsoft.AspNetCore.Http;

namespace Bunder
{
    /// <summary>
    /// Provider for handling unique versioning to paths.
    /// </summary>
    public interface IVersioningFormatter
    {
        /// <summary>
        /// Apply a versioning value to the <paramref name="virtualPath"/> that is unique to that path.
        /// </summary>
        /// <param name="basePath">Base url/path. Typically the base url of the current request.</param>
        /// <param name="virtualPath">Path to be versioned.</param>
        /// <returns></returns>
        string GetVersionedPath(string basePath, string virtualPath);
    }
}
