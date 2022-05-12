using System.Collections.Generic;
using System.Linq;

namespace Bunder
{
    /// <summary>
    /// Resolve values to a list of <see cref="Asset"/> based on list of paths or bundles and other various settings defined in <see cref="AssetResolutionContext"/>.
    /// </summary>
    public class AssetResolver : IAssetResolver
    {
        public AssetResolver(
            IBundleLookup bundleLookup, 
            IPathFormatter pathFormatter,
            IBunderCache cache,
            BunderSettings settings)
        {
            BundleLookup = bundleLookup;
            PathFormatter = pathFormatter;
            Cache = cache;
            Settings = settings;
        }

        protected IBundleLookup BundleLookup { get; }
        protected IPathFormatter PathFormatter { get; }
        protected IBunderCache Cache { get; }
        protected BunderSettings Settings { get; }

        /// <summary>
        /// Resolves values defined in <paramref name="context"/> to a list of renderable <see cref="Asset"/>.
        /// Paths or bundles defined at <see cref="AssetResolutionContext.PathsOrBundles"/> will be resolved to a list of <see cref="Bundle"/> and then to a list of <see cref="Asset"/>.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public virtual IEnumerable<Asset> Resolve(AssetResolutionContext context)
        {
            Guard.IsNotNull(context, nameof(context));

            string cacheKey = context.ToCacheKey();
            if (Settings.Cache.Enabled && Cache.TryGet(cacheKey, out IEnumerable<Asset> cachedAssets))
                return cachedAssets;

            var assets = new List<Asset>();
            BuildAssets(assets, context.PathsOrBundles, context.UseBundledOutput, context.IncludeVersioning);
            var resultingAssets = EliminateDuplicates(assets, context.IncludeVersioning);

            if (Settings.Cache.Enabled)
                Cache.Add(cacheKey, resultingAssets);

            return resultingAssets;
        }

        /// <summary>
        /// List of paths or bundles will be resolved to a list of asset paths based on <see cref="Asset.Value"/>.
        /// </summary>
        /// <param name="pathsOrBundles">List of paths or bundles to resolve.</param>
        /// <returns></returns>
        public virtual IEnumerable<string> Resolve(params string[] pathsOrBundles)
        {
            if (pathsOrBundles == null || pathsOrBundles.Length == 0)
                return Enumerable.Empty<string>();

            var assets = Resolve(new AssetResolutionContext(pathsOrBundles,
                                                            useBundledOutput: Settings.UseBundledOutput,
                                                            includeVersioning: Settings.UseVersioning));

            if (assets == null)
                return Enumerable.Empty<string>();

            return assets.Select(a => a.Value);
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
                if (BundleLookup.TryGetBundle(contentReference, out Bundle? bundle) && bundle != null)
                {
                    if (useBundledOutput)
                        assets.Add(new Asset(PathFormatter.GetFullPath(bundle.OutputPath, useVersioning), bundle: bundle));
                    else
                        BuildAssets(assets, bundle.Files, useBundledOutput, useVersioning);
                }
                else
                {
                    assets.Add(new Asset(PathFormatter.GetFullPath(contentReference, useVersioning)));
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
                            bundledContents.Add(PathFormatter.GetFullPath(filePath, useVersioning));
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
