using System.Collections.Generic;
using System.Linq;

namespace Bunder
{
    public class BundleLookup : IBundleLookup
    {
        private readonly IDictionary<string, Bundle> _bundleDictionary;

        public BundleLookup(IEnumerable<Bundle> bundles)
        {
            Guard.IsNotNull(bundles, nameof(bundles));

            _bundleDictionary = bundles.ToDictionary((bundle) => bundle.Name?.ToLower());
        }

        public bool TryGetBundle(string name, out Bundle bundle)
        {
            return _bundleDictionary.TryGetValue(name?.ToLower(), out bundle);
        }
    }
}
