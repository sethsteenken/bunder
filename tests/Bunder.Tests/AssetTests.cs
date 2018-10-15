using System.Web;
using Xunit;

namespace Bunder.Tests
{
    public class AssetTests
    {
        [Theory]
        [InlineData("http://www.google.com/something<>")]
        [InlineData("yahoo.com/something!$@#")]
        public void Value_IsUrlEncoded(string value)
        {
            var asset = new Asset(value);
            string encoded = HttpUtility.UrlEncode(value);
            Assert.Equal(encoded, asset.Value);
        }

        [Fact]
        public void IsBundle_ReturnsFalse_WhenNoBundleIsSupplied()
        {
            var asset = new Asset("/my/path", bundle: null);
            Assert.False(asset.IsBundle);
        }

        [Fact]
        public void IsBundle_ReturnsTrue_WhenBundleIsSupplied()
        {
            var asset = new Asset("/my/path", bundle: BundleTestHelper.GetSampleBundle());
            Assert.True(asset.IsBundle);
        }

        [Fact]
        public void ToString_ReturnsValue()
        {
            var asset = new Asset("/my/path");
            var expected = asset.ToString();
            Assert.Equal(expected, asset.Value);
        }
    }
}
