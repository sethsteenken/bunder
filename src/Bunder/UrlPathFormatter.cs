using System;
using System.Web;

namespace Bunder
{
    public class UrlPathFormatter : IPathFormatter
    {
        private readonly string _baseUrl;
        private readonly IVersioningFormatter _versioningFormatter;

        public UrlPathFormatter(string baseUrl, IVersioningFormatter versioningFormatter)
        {
            // TODO - _httpContextAccessor.HttpContext.Request.GetBaseUrl()
            _baseUrl = baseUrl;
            _versioningFormatter = versioningFormatter;
        }

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
