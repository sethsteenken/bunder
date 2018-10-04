using System;
using System.Collections.Generic;
using System.Linq;

namespace Bunder
{
    public class BundleLookup : IBundleLookup
    {
        private readonly IEnumerable<Bundle> _bundles;

        public BundleLookup(IEnumerable<Bundle> bundles)
        {
            Guard.IsNotNull(bundles, nameof(bundles));

            _bundles = bundles;
        }

        public virtual bool TryGetBundle(string name, out Bundle bundle)
        {
            bundle = FindBundle(name);
            return bundle != null;
        }

        protected virtual Bundle FindBundle(string pathOrName)
        {
            if (string.IsNullOrWhiteSpace(pathOrName))
                return null;

            return _bundles.FirstOrDefault(x => string.Compare(x.Name, pathOrName, StringComparison.InvariantCultureIgnoreCase) == 0);
        }
    }
}
