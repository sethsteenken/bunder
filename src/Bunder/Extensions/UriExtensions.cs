using System;

namespace Bunder
{
    internal static class UriExtensions
    {
        /// <summary>
        /// Returns <see cref="Uri.PathAndQuery"/> if the supplied <paramref name="uri"/> is absolute.
        /// </summary>
        /// <param name="uri"></param>
        public static string ToRelativeUrl(this Uri uri)
        {
            Guard.IsNotNull(uri, nameof(uri));

            return uri.IsAbsoluteUri ? uri.PathAndQuery : uri.OriginalString;
        }
    }
}
