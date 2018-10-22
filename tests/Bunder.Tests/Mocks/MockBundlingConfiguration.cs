using System;
using System.Collections.Generic;

namespace Bunder.Tests
{
    // NOTE: Using public declaration to allow Moq to mock this class for testing.
    public class MockBundlingConfiguration : BundlingConfigurationBase
    {
        private readonly Func<IReadOnlyList<BundleConfig>> _getBundleConfigurations;

        public MockBundlingConfiguration()
            : this (getBundleConfigurations: null)
        {

        }

        public MockBundlingConfiguration(IDictionary<string, string> outputDirectoryLookup) 
            : base(outputDirectoryLookup)
        {
        }

        public MockBundlingConfiguration(Func<IReadOnlyList<BundleConfig>> getBundleConfigurations)
            : base(new Dictionary<string, string>())
        {
            _getBundleConfigurations = getBundleConfigurations ?? (() => new List<BundleConfig>());
        }

        protected override IReadOnlyList<BundleConfig> GetBundleConfiguration()
        {
            return _getBundleConfigurations.Invoke();
        }
    }
}
