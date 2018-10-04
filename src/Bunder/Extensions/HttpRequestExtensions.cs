using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System;

namespace Bunder
{
    internal static class HttpRequestExtensions
    {
        public static Uri GetUri(this HttpRequest request)
        {
            return new Uri(request.GetDisplayUrl());
        }

        public static string GetBaseUrl(this HttpRequest request)
        {
            return GetBaseUrl(request, includeTrailingSlash: true);
        }

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
