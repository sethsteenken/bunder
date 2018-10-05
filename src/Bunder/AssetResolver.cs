using System.Collections.Generic;
using System.Linq;

namespace Bunder
{
    public class AssetResolver : IAssetResolver
    {
        private readonly IBundleLookup _bundleLookup;
        private readonly IPathFormatter _pathFormatter;

        public AssetResolver(IBundleLookup bundleLookup, IPathFormatter pathFormatter)
        {
            _bundleLookup = bundleLookup;
            _pathFormatter = pathFormatter;
        }

        public virtual IEnumerable<Asset> Resolve(AssetResolutionContext context)
        {
            Guard.IsNotNull(context, nameof(context));

            var assets = new List<Asset>();
            BuildAssets(assets, context.PathsOrBundles, context.UseBundledOutput, context.IncludeVersioning);
            return EliminateDuplicates(assets, context.IncludeVersioning);
        }

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
