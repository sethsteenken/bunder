using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Bunder
{
    /// <summary>
    /// Adds versioning to file path. Wrapper around <see cref="IFileVersionProvider"/>.
    /// </summary>
    public class FileVersioningFormatter : IVersioningFormatter
    {
        private readonly IFileVersionProvider _fileVersionProvider;

        public FileVersioningFormatter(IFileVersionProvider fileVersionProvider)
        {
            _fileVersionProvider = fileVersionProvider;
        }

        /// <summary>
        /// Apply a versioning value to the <paramref name="virtualPath"/> that is unique to that path.
        /// Versioning will be applied and cached. File will be watched for changes in order to bust cache and apply a new version.
        /// </summary>
        /// <param name="virtualPath">Path to be versioned.</param>
        /// <returns></returns>
        public string GetVersionedPath(string virtualPath)
        {
            Guard.IsNotNull(virtualPath, nameof(virtualPath));

            return _fileVersionProvider.AddFileVersionToPath(PathString.FromUriComponent("/"), virtualPath);
        }
    }
}
