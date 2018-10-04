using System.Collections.Generic;

namespace Bunder
{
    public interface IAssetResolver
    {
        IEnumerable<Asset> Resolve(IEnumerable<string> pathsOrBundles, bool useBundledOutput, bool includeVersioning);
    }
}
