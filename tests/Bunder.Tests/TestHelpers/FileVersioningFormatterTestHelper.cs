using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using Moq;
using System.Collections.Generic;
using System.IO;

namespace Bunder.Tests
{
    internal class FileVersioningFormatterTestHelper
    {
        public static FileVersioningFormatter BuildFormatter(IFileProvider fileProvider = null, IMemoryCache cache = null)
        {
            if (fileProvider == null)
                fileProvider = new Mock<IFileProvider>().Object;

            if (cache == null)
            {
                var entryMock = new Mock<ICacheEntry>();
                entryMock.Setup(e => e.ExpirationTokens).Returns(new List<IChangeToken>() { new Mock<IChangeToken>().Object });

                var cacheMock = new Mock<IMemoryCache>();
                cacheMock.Setup(c => c.CreateEntry(It.IsAny<object>())).Returns(entryMock.Object);

                cache = cacheMock.Object;
            }
              
            return new FileVersioningFormatter(fileProvider, cache);
        }

        public static IFileProvider BuildFileProvider(string path, bool fileExists = true)
        {
            var fileProvider = new Mock<IFileProvider>();
            var file = new Mock<IFileInfo>();
            file.SetupAllProperties()
                .Setup(f => f.Exists).Returns(fileExists);
            file.Setup(f => f.PhysicalPath).Returns(path);

            file.Setup(f => f.CreateReadStream()).Returns(new MemoryStream());

            fileProvider.Setup(f => f.GetFileInfo(path)).Returns(file.Object);

            var changeToken = new Mock<IChangeToken>();
            fileProvider.Setup(f => f.Watch(path)).Returns(changeToken.Object);

            return fileProvider.Object;
        }
    }
}
