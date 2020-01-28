using System.Collections.Generic;
using System.IO;

namespace Bunder
{
    /// <summary>
    /// Bundling configuration that reads a local json file to build list of <see cref="BundleConfig"/> to be transitioned to list of <see cref="Bundle"/>.
    /// </summary>
    public class BundlingJsonConfiguration : BundlingConfigurationBase
    {
        private readonly ISerializer _serializer;
        private readonly string _filePath;

        public BundlingJsonConfiguration(
            IDictionary<string, string> outputDirectoryLookup,
            ISerializer serializer,
            string filePath)
            : base(outputDirectoryLookup)
        {
            Guard.IsNotNull(serializer, nameof(serializer));
            Guard.IsNotNull(filePath, nameof(filePath));

            _serializer = serializer;
            _filePath = filePath;
        }

        protected override IEnumerable<BundleConfig> GetBundleConfiguration()
        {
            return _serializer.Deserialize<IEnumerable<BundleConfig>>(File.ReadAllText(_filePath));
        } 
    }
}
