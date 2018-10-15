using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Bunder.Tests
{
    public class AssetResolverTests
    {
        [Fact]
        public void Resolve_ThrowsException_WhenContextIsNull()
        {
            var assetResolver = AssetResolverTestHelper.BuildTestResolver();
            var exception = Record.Exception(() => assetResolver.Resolve(context: null));
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void Resolve_DoesNotReturnNull_WhenValidContextWithNullPathsIsSupplied()
        {
            var assetResolver = AssetResolverTestHelper.BuildTestResolver();
            var context = new AssetResolutionContext(null, useBundledOutput: true, includeVersioning: true);

            var assets = assetResolver.Resolve(context);

            Assert.NotNull(assets);
        }

        [Fact]
        public void Resolve_DoesNotReturnNull_WhenValidContextWithPathsIsSupplied()
        {
            var assetResolver = AssetResolverTestHelper.BuildTestResolver();
            var context = AssetResolverTestHelper.BuildValidResolutionContext();

            var assets = assetResolver.Resolve(context);

            Assert.NotNull(assets);
        }

        [Theory]
        [InlineData("my-bundle", "/my/source.js")]
        [InlineData()]
        [InlineData("my-bundle", "/my/source.js", "some-other-bundle", "another-bundle")]
        public void Resolve_ReturnsCorrectCount_WhenBundledOutputTrue(params string[] pathsOrBundles)
        {
            var pathFormatter = new Mock<IPathFormatter>();
            foreach (var path in pathsOrBundles)
            {
                pathFormatter.Setup(pf => pf.GetFullPath(path, true)).Returns(path);
            }
                                    
            var assetResolver = AssetResolverTestHelper.BuildTestResolver(pathFormatter: pathFormatter.Object);

            var context = new AssetResolutionContext(pathsOrBundles, useBundledOutput: true, includeVersioning: true);

            var assets = assetResolver.Resolve(context);

            Assert.Equal(pathsOrBundles?.Count() ?? 0, assets.Count);
        }

        [Theory]
        [InlineData("my-bundle", "one.js", "two.js")]
        [InlineData("my-other-bundle", "one-source-file.js")]
        public void Resolve_ReturnsBundledFiles_WhenValidBundleIsFoundAndUseBundledOutputFalse(string bundleName, params string[] files)
        {
            var bundle = new Bundle(bundleName, "js", "/my/output-directory", files);
            var bundleLookup = new Mock<IBundleLookup>();
            bundleLookup.Setup(bl => bl.TryGetBundle(bundleName, out bundle)).Returns(true);

            var pathFormatter = new Mock<IPathFormatter>();
            foreach (var path in files)
            {
                pathFormatter.Setup(pf => pf.GetFullPath(path, true)).Returns(path);
            }

            var assetResolver = AssetResolverTestHelper.BuildTestResolver(bundleLookup: bundleLookup.Object, pathFormatter: pathFormatter.Object);

            var context = new AssetResolutionContext(new[] { bundleName }, useBundledOutput: false, includeVersioning: true);

            var assets = assetResolver.Resolve(context);

            Assert.Equal(bundle.Files, assets.Select(a => a.Value).ToArray());
        }
    }
}
