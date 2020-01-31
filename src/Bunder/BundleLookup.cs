using System;
using System.Collections.Generic;
using System.Linq;

namespace Bunder
{
    /// <summary>
    /// Lookup provider that will try to find a registered <see cref="Bundle"/> based off a given name value.
    /// Registration of <see cref="Bundle"/> will be looked up through a dictionary.
    /// </summary>
    public class BundleLookup : IBundleLookup
    {
        private readonly IDictionary<string, Bundle> _bundleDictionary;

        public BundleLookup(IEnumerable<Bundle> bundles)
        {
            Guard.IsNotNull(bundles, nameof(bundles));

            _bundleDictionary = bundles.ToDictionary((bundle) => bundle.Name, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Try to find a registered <paramref name="bundle"/> based on <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The unique name of the registered <see cref="Bundle"/>.</param>
        /// <param name="bundle">The registered bundle found matching <paramref name="name"/>.</param>
        /// <returns>True if bundle is found. False if not found.</returns>
        public bool TryGetBundle(string name, out Bundle bundle)
        {
            Guard.IsNotNull(name, nameof(name));

            return _bundleDictionary.TryGetValue(name, out bundle);
        }
    }
}
