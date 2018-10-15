using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.FileProviders;
using System;

namespace Bunder
{
    /// <summary>
    /// Ref - https://github.com/aspnet/Mvc/blob/4b83f7b510f22ea4acd7e383fd483301c77f5fec/src/Microsoft.AspNetCore.Mvc.Razor/Infrastructure/DefaultFileVersionProvider.cs
    /// </summary>
    public class FileVersioningFormatter : IVersioningFormatter
    {
        private const string _versionKey = "v";
        private static readonly char[] _queryStringAndFragmentTokens = new[] { '?', '#' };

        private readonly IFileProvider _fileProvider;
        private readonly IMemoryCache _cache;

        public FileVersioningFormatter(IFileProvider fileProvider, IMemoryCache cache)
        {
            _fileProvider = fileProvider;
            _cache = cache;
        }

        public string GetVersionedPath(PathString basePath, string virtualPath)
        {
            Guard.IsNotNull(virtualPath, nameof(virtualPath));

            var resolvedPath = virtualPath;

            var queryStringOrFragmentStartIndex = virtualPath.IndexOfAny(_queryStringAndFragmentTokens);
            if (queryStringOrFragmentStartIndex != -1)
                resolvedPath = virtualPath.Substring(0, queryStringOrFragmentStartIndex);

            // do not append version if the path is absolute
            if (Uri.TryCreate(resolvedPath, UriKind.Absolute, out var uri) && !uri.IsFile)               
                return virtualPath;

            if (_cache != null && _cache.TryGetValue(virtualPath, out string value))
                return value;

            var cacheEntryOptions = new MemoryCacheEntryOptions();
            cacheEntryOptions.AddExpirationToken(_fileProvider.Watch(resolvedPath));
            var fileInfo = _fileProvider.GetFileInfo(resolvedPath);

            if (!fileInfo.Exists &&
                basePath.HasValue &&
                resolvedPath.StartsWith(basePath.Value, StringComparison.OrdinalIgnoreCase))
            {
                var requestPathBaseRelativePath = resolvedPath.Substring(basePath.Value.Length);
                cacheEntryOptions.AddExpirationToken(_fileProvider.Watch(requestPathBaseRelativePath));
                fileInfo = _fileProvider.GetFileInfo(requestPathBaseRelativePath);
            }

            if (fileInfo.Exists)
                value = QueryHelpers.AddQueryString(virtualPath, _versionKey, GetHashForFile(fileInfo));
            else
                value = virtualPath; // if the file is not in the current server.

            cacheEntryOptions.SetSize(value.Length * sizeof(char));
            value = _cache?.Set(virtualPath, value, cacheEntryOptions);

            return value;
        }

        private static string GetHashForFile(IFileInfo fileInfo)
        {
            using (var sha256 = Cryptography.CreateSHA256())
            {
                using (var readStream = fileInfo.CreateReadStream())
                {
                    var hash = sha256.ComputeHash(readStream);
                    return WebEncoders.Base64UrlEncode(hash);
                }
            }
        }
    }
}
