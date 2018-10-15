using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System;

namespace Bunder
{
    internal static class HttpRequestExtensions
    {
        /// <summary>
        /// Construct a new Uri based off of the given <paramref name="request"/> and <see cref="UriHelper.GetDisplayUrl"/>.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static Uri GetUri(this HttpRequest request)
        {
            return new Uri(request.GetDisplayUrl());
        }

        /// <summary>
        /// Get the base "domain" url (scheme and authority) of the given <paramref name="request"/>.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetBaseUrl(this HttpRequest request)
        {
            return GetBaseUrl(request, includeTrailingSlash: true);
        }

        /// <summary>
        /// Get the base "domain" url (scheme and authority) of the given <paramref name="request"/>.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="includeTrailingSlash"></param>
        /// <returns></returns>
        public static string GetBaseUrl(this HttpRequest request, bool includeTrailingSlash)
        {
            Guard.IsNotNull(request, nameof(request));

            var uri = request.GetUri();
            return string.Concat(
                uri.Scheme,
                "://",
                uri.Authority,
                (includeTrailingSlash ? "/" : string.Empty));
        }
    }
}
