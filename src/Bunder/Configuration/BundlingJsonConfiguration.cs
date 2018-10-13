using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Bunder
{
    /// <summary>
    /// Bundling configuration that reads a local json file to build list of <see cref="BundleConfig"/> to be transitioned to list of <see cref="Bundle"/>.
    /// </summary>
    public class BundlingJsonConfiguration : BundlingConfigurationBase
    {
        private readonly JsonSerializer _serializer;
        private readonly string _filePath;

        public BundlingJsonConfiguration(IDictionary<string, string> outputDirectoryLookup, JsonSerializer serializer, string filePath)
            : base(outputDirectoryLookup)
        {
            Guard.IsNotNull(serializer, nameof(serializer));
            Guard.IsNotNull(filePath, nameof(filePath));

            _serializer = serializer;
            _filePath = filePath;
        }

        protected override IReadOnlyList<BundleConfig> GetBundleConfiguration()
        {
            using (StreamReader file = File.OpenText(_filePath))
            {
                return (List<BundleConfig>)_serializer.Deserialize(file, typeof(List<BundleConfig>));
            }
        } 
    }
}
