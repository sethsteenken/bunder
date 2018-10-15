namespace Bunder
{
    /// <summary>
    /// Path formatting provider to resolve virtual paths.
    /// </summary>
    public interface IPathFormatter
    {
        /// <summary>
        /// Get the fully qualified path of a <paramref name="virtualPath"/>.
        /// </summary>
        /// <param name="virtualPath">Virtual path to resolve.</param>
        /// <param name="includeVersioning">Whether or not to append versioning to the full path result.</param>
        /// <returns></returns>
        string GetFullPath(string virtualPath, bool includeVersioning);
    }
}
