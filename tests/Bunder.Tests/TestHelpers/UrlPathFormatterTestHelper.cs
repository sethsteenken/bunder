using Moq;

namespace Bunder.Tests
{
    internal static class UrlPathFormatterTestHelper
    {
        public static UrlPathFormatter BuildFormatter(string baseUrl = null, IVersioningFormatter versioningFormatter = null)
        {
            if (string.IsNullOrWhiteSpace(baseUrl))
                baseUrl = "https://www.google.com/";

            if (versioningFormatter == null)
            {
                var versioningFormatterMock = new Mock<IVersioningFormatter>();

                versioningFormatter = versioningFormatterMock.Object;
            }

            return new UrlPathFormatter(baseUrl, versioningFormatter);
        }
    }
}
