using System.Collections.Generic;

namespace Bunder
{
    /// <summary>
    /// Context model containing information to resolve a list of paths or bundles to a processable list of <see cref="Bunder.Bundle"/> and then list to a list of <see cref="Asset"/>.
    /// This model is supplied to <see cref="IAssetResolver.Resolve(AssetResolutionContext)"/> to resolve a list of bundles for processing.
    /// </summary>
    public sealed class AssetResolutionContext
    {
        public AssetResolutionContext(IEnumerable<string> pathsOrBundles, bool useBundledOutput, bool includeVersioning)
        {
            PathsOrBundles = pathsOrBundles ?? new List<string>();
            UseBundledOutput = useBundledOutput;
            IncludeVersioning = includeVersioning;
        }

        /// <summary>
        /// List of paths or bundle references that should be resolved to <see cref="Bundle"/> and later to <see cref="Asset"/>.
        /// </summary>
        public IEnumerable<string> PathsOrBundles { get; private set; }

        /// <summary>
        /// Whether or not a bundle's output should be provided over it's list of files.
        /// </summary>
        public bool UseBundledOutput { get; private set; }

        /// <summary>
        /// Whether or not a unique version number should applied when generating list of <see cref="Asset"/>.
        /// </summary>
        public bool IncludeVersioning { get; private set; }
    }
}
