using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Bunder.Tests
{
    public class BundlingConfigurationBaseTests
    {
        [Fact]
        public void Build_DoesNotReturnNull()
        {
            var configuration = new Mock<MockBundlingConfiguration>();
            Assert.NotNull(configuration.Object.Build());
        }

        [Fact]
        public void Build_ReturnsBundleList_FromBundleConfigs()
        {
            var testValidBundleConfigs = new List<BundleConfig>()
            {
                new BundleConfig() {
                    Name = "my-bundle-one",
                    Files = new string[] { "/my-files/one.js", "/my-files/two.js" },
                },
                new BundleConfig() {
                    Name = "my-bundle-two",
                    Files = new string[] { "/my-files/test-two.js" },
                }
            };

            var configuration = new MockBundlingConfiguration(() => testValidBundleConfigs);
            var bundles = configuration.Build();

            Assert.Equal(testValidBundleConfigs.Count, bundles.Count());

            foreach (var testConfig in testValidBundleConfigs)
            {
                Assert.Contains(bundles, b => b.Name == testConfig.Name);
            }
        }

        [Theory]
        [InlineData(null)]
        [InlineData(new object[] { new string[] { } })] // syntax required to pass empty array
        public void Build_ThrowsException_IfBundleConfigDoesNotHaveAnyFiles(string[] files)
        {
            var configuration = new MockBundlingConfiguration(() =>
            {
                return new List<BundleConfig>() {
                    new BundleConfig() {  Name = "bad-bundle-with-no-files", Files = files },
                };
            });

            Assert.Throws<BundleConfigurationException>(() => configuration.Build());
        }

        [Fact]
        public void Build_ThrowsException_IfDuplicateBundleConfigsItemsAreSupplied()
        {
            var configuration = new MockBundlingConfiguration(() =>
            {
                return new List<BundleConfig>() {
                    new BundleConfig() {  Name = "duplicate-bundle-name", Files = new string [] { "myfile.js", "myotherfile.js" } },
                    new BundleConfig() {  Name = "Duplicate-Bundle-Name", Files = new string [] { "myfile_again.js", "myotherfile_again.js" } }
                };
            });

            Assert.Throws<BundleConfigurationException>(() => configuration.Build());
        }
    }
}
