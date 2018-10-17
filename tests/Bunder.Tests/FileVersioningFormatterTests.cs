using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using System;
using Xunit;

namespace Bunder.Tests
{
    public class FileVersioningFormatterTests
    {
        [Fact]
        public void GetVersionedPath_ThrowsException_IfAnyParametersAreNull()
        {
            var formatter = FileVersioningFormatterTestHelper.BuildFormatter();

            Assert.Throws<ArgumentNullException>(() => formatter.GetVersionedPath(null, "/some/virtual/path.jpg"));
            Assert.Throws<ArgumentNullException>(() => formatter.GetVersionedPath(new PathString("/my/pathstring.jpg"), null));
        }

        [Theory]
        [InlineData("/test/myfile.js")]
        [InlineData("my-test-file.jpg")]
        [InlineData("/some/path/")]
        public void GetVersionedPath_ReturnsValueFromCache_WhenCacheValueExists(string path)
        {
            object expectedCacheValue = "value-from-cache.jpg";
            var cache = new Mock<IMemoryCache>();
            cache.Setup(m => m.TryGetValue((object)path, out expectedCacheValue)).Returns(true);

            var formatter = FileVersioningFormatterTestHelper.BuildFormatter(cache: cache.Object);

            var result = formatter.GetVersionedPath(new PathString("/test"), path);

            Assert.Equal(expectedCacheValue, result);
        }

        [Fact]
        public void GetVersionedPath_ReturnsVersionedValue_WhenFileExists()
        {
            var path = "/test/myfile.js";

            var fileProvider = FileVersioningFormatterTestHelper.BuildFileProvider(path);

            var formatter = FileVersioningFormatterTestHelper.BuildFormatter(fileProvider: fileProvider);

            var result = formatter.GetVersionedPath(new PathString("/test"), path);

            Assert.Contains("v=", result);
            Assert.Contains(path, result);
        }
    }
}
