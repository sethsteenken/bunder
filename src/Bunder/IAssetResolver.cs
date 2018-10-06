using System.Collections.Generic;

namespace Bunder
{
    public interface IAssetResolver
    {
        IReadOnlyList<Asset> Resolve(AssetResolutionContext context);
    }
}
