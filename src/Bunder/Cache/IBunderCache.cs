using System.Collections.Generic;

namespace Bunder
{
    /// <summary>
    /// Cache for bunder services, particularly for storing resolved assets.
    /// </summary>
    public interface IBunderCache
    {
        /// <summary>
        /// Try to get a list of assets from cache based on key.
        /// </summary>
        /// <param name="key">Key in the cache. This is typically the bundle/paths and the Bunder settings concatenated.</param>
        /// <param name="assets"></param>
        /// <returns></returns>
        bool TryGet(string key, out IEnumerable<Asset> assets);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="assets"></param>
        void Add(string key, IEnumerable<Asset> assets);
    }
}
