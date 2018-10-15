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

        /// <summary>
        /// Returns the absolute url for a given <paramref name="uri"/> based on the supplied <paramref name="baseUrl"/>.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="baseUrl"></param>
        /// <returns></returns>
        public static string ToAbsoluteUrl(this Uri uri, string baseUrl)
        {
            return ToAbsoluteUrl(uri, new Uri(baseUrl));
        }

        /// <summary>
        /// Returns the absolute url for a given <paramref name="uri"/> based on the supplied <paramref name="baseUri"/>.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="baseUri"></param>
        /// <returns></returns>
        public static string ToAbsoluteUrl(this Uri uri, Uri baseUri)
        {
            Guard.IsNotNull(uri, nameof(uri));
            Guard.IsNotNull(baseUri, nameof(baseUri));

            if (Uri.TryCreate(baseUri, ToRelativeUrl(uri), out Uri absolute))
                return absolute.ToString();

            return uri.IsAbsoluteUri ? uri.ToString() : null;
        }
    }
}
