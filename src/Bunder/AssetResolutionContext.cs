using System.Collections.Generic;

namespace Bunder
{
    public sealed class AssetResolutionContext
    {
        public AssetResolutionContext(IEnumerable<string> pathsOrBundles, bool useBundledOutput, bool includeVersioning)
        {
            PathsOrBundles = pathsOrBundles ?? new List<string>();
            UseBundledOutput = useBundledOutput;
            IncludeVersioning = includeVersioning;
        }

        public IEnumerable<string> PathsOrBundles { get; private set; }
        public bool UseBundledOutput { get; private set; }
        public bool IncludeVersioning { get; private set; }
    }
}
