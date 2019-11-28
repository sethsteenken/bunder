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
        /// <param name="virtualPath">Path to be versioned.</param>
        /// <returns></returns>
        string GetVersionedPath(string virtualPath);
    }
}
