using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bunder
{
    internal class BunderResolutionMemoryCache : IBunderCache
    {
        private readonly BunderCacheSettings _cacheSettings;
        private readonly IMemoryCache _memoryCache;

        public BunderResolutionMemoryCache(BunderCacheSettings cacheSettings, IMemoryCache memoryCache)
        {
            _cacheSettings = cacheSettings;
            _memoryCache = memoryCache;
        }

        public void Add(string key, IEnumerable<Asset> assets)
        {
            if (!_cacheSettings.Enabled)
                return;

            if (_cacheSettings.Duration == default)
                _memoryCache.Set(key, assets);
            else
                _memoryCache.Set(key, assets, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(_cacheSettings.Duration.TotalMinutes)));
        }

        public bool TryGet(string key, out IEnumerable<Asset> assets)
        {
            if (!_cacheSettings.Enabled)
            {
                assets = Enumerable.Empty<Asset>();
                return false;
            }

            return _memoryCache.TryGetValue(key, out assets);
        }
    }
}
