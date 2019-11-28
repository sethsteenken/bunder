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
            Assert.Throws<ArgumentNullException>(() => formatter.GetVersionedPath(null));
        }
    }
}
