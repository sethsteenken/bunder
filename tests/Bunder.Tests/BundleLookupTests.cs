using System;
using System.Collections.Generic;
using Xunit;

namespace Bunder.Tests
{
    public class BundleLookupTests
    {
        private const string _bundleNameOne = "bundle-one";
        private const string _bundleNameTwo = "bundle-two";
        private const string _bundleNameThree = "Bundle-Three";

        private static readonly IEnumerable<Bundle> _testBundles = new List<Bundle>()
        {
            new Bundle(_bundleNameOne, "js", "/my/output", new string[] { "/bundle/source/one.js", "/bundle/source/two.js" }),
            new Bundle(_bundleNameTwo, "js", "/my/output", new string[] { "/single/file.js" }),
            new Bundle(_bundleNameThree, "css", "/my/output", new string[] { "/source/one/file.css", "/source/two/file.css" })
        };

        [Fact]
        public void TryGetBundle_ThrowsException_WhenNameIsNull()
        {
            var bundleLookup = new BundleLookup(_testBundles);
            Assert.Throws<ArgumentNullException>(() => bundleLookup.TryGetBundle(null, out Bundle bundle));
        }

        [Theory]
        [InlineData(_bundleNameOne)]
        [InlineData(_bundleNameTwo)]
        [InlineData(_bundleNameThree)]
        public void TryGetBundle_ReturnsValidBundle_WhenBundleIsRegistered(string bundleName)
        {
            var bundleLookup = new BundleLookup(_testBundles);

            var result = bundleLookup.TryGetBundle(bundleName, out Bundle bundle);

            Assert.True(result);
            Assert.NotNull(bundle);
            Assert.Equal(bundleName, bundle.Name);
        }

        [Theory]
        [InlineData("nope-not-a-bundle")]
        [InlineData("Missing Bundle")]
        public void TryGetBundle_ReturnsFalse_WhenBundleIsNotRegistered(string bundleName)
        {
            var bundleLookup = new BundleLookup(_testBundles);

            var result = bundleLookup.TryGetBundle(bundleName, out Bundle bundle);

            Assert.False(result);
            Assert.Null(bundle);
        }
    }
}
