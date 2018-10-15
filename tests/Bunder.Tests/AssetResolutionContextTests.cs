using Xunit;

namespace Bunder.Tests
{
    public class AssetResolutionContextTests
    {
        [Fact]
        public void PathsOrBundles_ReturnsEmpty_WhenNullIsSupplied()
        {
            var context = new AssetResolutionContext(pathsOrBundles: null, useBundledOutput: false, includeVersioning: false);
            Assert.NotNull(context.PathsOrBundles);
            Assert.Empty(context.PathsOrBundles);
        }
    }
}
