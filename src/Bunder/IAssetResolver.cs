using System.Collections.Generic;

namespace Bunder
{
    public interface IAssetResolver
    {
        IEnumerable<Asset> Resolve(AssetResolutionContext context);
    }
}
