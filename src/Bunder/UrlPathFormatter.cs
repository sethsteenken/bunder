using System;
using System.Web;

namespace Bunder
{
    /// <summary>
    /// Path formatting provider to resolve virtual paths. Intended to utilize a base url based on the current web request.
    /// </summary>
    public class UrlPathFormatter : IPathFormatter
    {
        private readonly string _baseUrl;
        private readonly IVersioningFormatter _versioningFormatter;

        public UrlPathFormatter(string baseUrl, IVersioningFormatter versioningFormatter)
        {
            _baseUrl = baseUrl;
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
                virtualPath = _versioningFormatter.GetVersionedPath(_baseUrl, virtualPath);

            if (!Uri.TryCreate(virtualPath, UriKind.RelativeOrAbsolute, out Uri uri))
                throw new InvalidCastException($"Could not create Uri from virtual path '{virtualPath}'.");

            return HttpUtility.UrlEncode(uri.ToAbsoluteUrl(_baseUrl));
        }
    }
}
