using System;
using System.Web;

namespace Bunder
{
    /// <summary>
    /// Path formatting provider to resolve virtual paths. Intended to utilize a base url based on the current web request.
    /// </summary>
    public class UrlPathFormatter : IPathFormatter
    {
        private readonly IVersioningFormatter _versioningFormatter;

        public UrlPathFormatter(IVersioningFormatter versioningFormatter)
        {
            _versioningFormatter = versioningFormatter;
        }

        /// <summary>
        /// Get the fully qualified path of a <paramref name="virtualPath"/>. Path must resolve to a valid <see cref="Uri"/>.
        /// </summary>
        /// <param name="virtualPath">Virtual path to resolve.</param>
        /// <param name="includeVersioning">Whether or not to append versioning to the full path result.</param>
        /// <returns></returns>
        public virtual string GetFullPath(string virtualPath, bool includeVersioning)
        {
            Guard.IsNotNull(virtualPath, nameof(virtualPath));

            if (includeVersioning)
                virtualPath = _versioningFormatter.GetVersionedPath(virtualPath);

            if (!IsValidUri(virtualPath, out Uri uri))
                throw new FormatException($"Could not create Uri from virtual path '{virtualPath}'.");

            return GetPath(uri);
        }

        protected virtual string GetPath(Uri uri)
        {
            return PathHelper.PrependSlash(uri.ToString());
        }

        private static bool IsValidUri(string path, out Uri uri)
        {
            uri = null;

            if (!Uri.IsWellFormedUriString(path, UriKind.RelativeOrAbsolute))
                return false;

            return Uri.TryCreate(path, UriKind.RelativeOrAbsolute, out uri);
        }
    }
}
