using System.IO;
using Xunit;

namespace Bunder.Tests
{
    public class BundleTests
    {
        [Fact]
        public void Files_DoesNotReturnNull_WhenNullIsSupplied()
        {
            var bundle = new Bundle("bundlename", "js", "output", files: null);
            Assert.NotNull(bundle.Files);
        }

        [Theory]
        [InlineData("my-bundle", "js")]
        [InlineData("another bundle", "js")]
        [InlineData("my-bundle", "css")]
        [InlineData("Bundle Time", "js")]
        public void OutputFileName_ReturnsDefaultFileName_WhenOutputFileNameNotSupplied(string bundleName, string ext)
        {
            var expected = $"{bundleName.Replace(" ", "_")}.min.{ext}";
            var bundle = new Bundle(bundleName, ext, "/my/output", files: null);
            Assert.Equal(expected, bundle.OutputFileName);
        }

        [Theory]
        [InlineData("my-bundle", "js", "my-awesome-bundle.min.js")]
        [InlineData("another bundle", "js", "another-great-bundle.min.js")]
        [InlineData("my-bundle", "css", "my-great-bundle.min.css")]
        [InlineData("Bundle Time", "js", "bundle-time-is-fun.min.js")]
        public void OutputFileName_ReturnsCorrectValue_WhenExplicitOutputFileNameIsSupplied(string bundleName, string ext, string outputFileName)
        {
            var bundle = new Bundle(bundleName, ext, "/my/output", files: null, outputFileName: outputFileName, subPath: null);
            Assert.Equal(outputFileName, bundle.OutputFileName);
        }


        [Theory]
        [InlineData("my-bundle", "/some/output", "my-awesome-bundle.min.js", null)]
        [InlineData("another bundle", "/some/output/", "another-great-bundle.min.js", "a/great/subpath")]
        [InlineData("my-bundle", @"\some\output", "my-great-bundle.min.css", null)]
        [InlineData("Bundle Time", "/some/output", "bundle-time-is-fun.min.js", "subpath")]
        public void OutputPath_ReturnsCorrectFullPathValue(string bundleName, string outputDir, string outputFileName, string subPath)
        {
            string expected = Path.Combine(outputDir ?? string.Empty, subPath ?? string.Empty, outputFileName ?? string.Empty);
            if (expected.Contains(Path.AltDirectorySeparatorChar))
                expected = expected.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

            var bundle = new Bundle(bundleName, "js", outputDir, files: null, outputFileName: outputFileName, subPath: subPath);
            Assert.Equal(expected, bundle.OutputPath);
        }
    }
}
