using System.Collections.Generic;
using System.Linq;

namespace Bunder
{
    internal class EmptyCache : IBunderCache
    {
        public void Add(string key, IEnumerable<Asset> assets)
        {
            
        }

        public bool TryGet(string key, out IEnumerable<Asset> assets)
        {
            assets = Enumerable.Empty<Asset>();
            return false;
        }
    }
}
