using System;

namespace Bunder
{
    internal static class UriExtensions
    {
        public static string ToRelativeUrl(this Uri uri)
        {
            Guard.IsNotNull(uri, nameof(uri));

            return uri.IsAbsoluteUri ? uri.PathAndQuery : uri.OriginalString;
        }

        public static string ToAbsoluteUrl(this Uri uri, string baseUrl)
        {
            return ToAbsoluteUrl(uri, new Uri(baseUrl));
        }

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
