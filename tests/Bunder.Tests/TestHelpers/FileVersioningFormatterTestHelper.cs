using Microsoft.AspNetCore.Mvc.ViewFeatures;
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
        public static FileVersioningFormatter BuildFormatter(IFileVersionProvider fileVersionProvider = null)
        {
            if (fileVersionProvider == null)
                fileVersionProvider = new Mock<IFileVersionProvider>().Object;
            return new FileVersioningFormatter(fileVersionProvider);
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
