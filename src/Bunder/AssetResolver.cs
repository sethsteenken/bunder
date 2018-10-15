using System.Collections.Generic;
using System.Linq;

namespace Bunder
{
    /// <summary>
    /// Resolve values to a list of <see cref="Asset"/> based on list of paths or bundles and other various settings defined in <see cref="AssetResolutionContext"/>.
    /// </summary>
    public class AssetResolver : IAssetResolver
    {
        private readonly IBundleLookup _bundleLookup;
        private readonly IPathFormatter _pathFormatter;

        public AssetResolver(IBundleLookup bundleLookup, IPathFormatter pathFormatter)
        {
            _bundleLookup = bundleLookup;
            _pathFormatter = pathFormatter;
        }

        /// <summary>
        /// Resolves values defined in <paramref name="context"/> to a list of renderable <see cref="Asset"/>.
        /// Paths or bundles defined at <see cref="AssetResolutionContext.PathsOrBundles"/> will be resolved to a list of <see cref="Bundle"/> and then to a list of <see cref="Asset"/>.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public virtual IReadOnlyList<Asset> Resolve(AssetResolutionContext context)
        {
            Guard.IsNotNull(context, nameof(context));

            var assets = new List<Asset>();
            BuildAssets(assets, context.PathsOrBundles, context.UseBundledOutput, context.IncludeVersioning);
            return EliminateDuplicates(assets, context.IncludeVersioning).ToList();
        }

        /// <summary>
        /// Recursively build list of assets based on list of string / path reference alues.
        /// </summary>
        /// <param name="assets">List of assets being built recursively.</param>
        /// <param name="references">List of paths or bundles to be resolved.</param>
        /// <param name="useBundledOutput">Use bundled output over list of files in bundle.</param>
        /// <param name="useVersioning">Apply versioning to asset output.</param>
        protected virtual void BuildAssets(List<Asset> assets, IEnumerable<string> references, bool useBundledOutput, bool useVersioning)
        {
            foreach (string contentReference in references)
            {
                if (_bundleLookup.TryGetBundle(contentReference, out Bundle bundle))
                {
                    if (useBundledOutput)
                        assets.Add(new Asset(_pathFormatter.GetFullPath(bundle.OutputPath, useVersioning), bundle));
                    else
                        BuildAssets(assets, bundle.Files, useBundledOutput, useVersioning);
                }
                else
                {
                    assets.Add(new Asset(_pathFormatter.GetFullPath(contentReference, useVersioning)));
                }
            }
        }

        /// <summary>
        /// Assess and remove any duplicates found in <paramref name="assets"/>.
        /// </summary>
        /// <param name="assets"></param>
        /// <param name="useVersioning"></param>
        /// <returns></returns>
        protected virtual IEnumerable<Asset> EliminateDuplicates(IEnumerable<Asset> assets, bool useVersioning)
        {
            var finalAssetList = new List<Asset>();
            var pathMap = new HashSet<string>();
            var bundledContents = new HashSet<string>();

            foreach (Asset asset in assets)
            {
                if (asset.IsStatic)
                {
                    finalAssetList.Add(asset);
                    continue;
                }

                if (!pathMap.Contains(asset.Value))
                {
                    // open bundles file list to look at its contents
                    if (asset.IsBundle && asset.Bundle != null)
                    {
                        // add the bundled contents to list to check so those scripts are not duplicated in the resulting list
                        foreach (string filePath in asset.Bundle.Files)
                        {
                            bundledContents.Add(_pathFormatter.GetFullPath(filePath, useVersioning));
                        }
                    }

                    finalAssetList.Add(asset);
                    pathMap.Add(asset.Value);
                }
            }

            return finalAssetList.Where(asset => asset.IsStatic || !bundledContents.Contains(asset.Value));
        }
    }
}
