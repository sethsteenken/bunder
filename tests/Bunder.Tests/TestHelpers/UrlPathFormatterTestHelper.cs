using Moq;

namespace Bunder.Tests
{
    internal static class UrlPathFormatterTestHelper
    {
        public static UrlPathFormatter BuildFormatter(IVersioningFormatter versioningFormatter = null)
        {
            if (versioningFormatter == null)
            {
                var versioningFormatterMock = new Mock<IVersioningFormatter>();

                versioningFormatter = versioningFormatterMock.Object;
            }

            return new UrlPathFormatter(versioningFormatter);
        }
    }
}
