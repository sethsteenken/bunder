using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Bunder
{
    /// <summary>
    /// Base class for handling bundling configuration, specifically from transitioning the basic POCOs <see cref="BundleConfig"/> to the final <see cref="Bundle"/>.
    /// </summary>
    public abstract class BundlingConfigurationBase : IBundlingConfiguration
    {
        protected BundlingConfigurationBase(IDictionary<string, string> outputDirectoryLookup)
        {
            Guard.IsNotNull(outputDirectoryLookup, nameof(outputDirectoryLookup));

            OutputDirectoryLookup = outputDirectoryLookup;
        }

        /// <summary>
        /// Lookup dictionary to get the output directories for each file extension.
        /// </summary>
        protected IDictionary<string, string> OutputDirectoryLookup { get; private set; }

        protected abstract IReadOnlyList<BundleConfig> GetBundleConfiguration();

        public virtual IEnumerable<Bundle> Build()
        {
            var bundleConfigs = GetBundleConfiguration();
            var bundles = new List<Bundle>();

            foreach (var bundleConfig in bundleConfigs)
            {
                if (bundleConfig.Files == null || bundleConfig.Files.Count == 0)
                    throw new BundleConfigurationException("Bundle must have at least one file under Files reference.");

                if (bundles.Any(b => string.Compare(b.Name, bundleConfig.Name, StringComparison.InvariantCultureIgnoreCase) == 0))
                    throw new BundleConfigurationException($"A bundle with the name '{bundleConfig.Name}' has already been registered. " +
                        $"Ensure all Bundle Name values created under {typeof(BundleConfig).FullName} " +
                        $"created from {typeof(IBundlingConfiguration).FullName} implementation are unique.");

                string fileExtension = Path.GetExtension(string.IsNullOrWhiteSpace(bundleConfig.OutputFileName) 
                                            ? bundleConfig.Files[0] 
                                            : bundleConfig.OutputFileName).Replace(".", "");
                string outputDirectory = bundleConfig.OutputDirectory;

                if (string.IsNullOrWhiteSpace(outputDirectory) && OutputDirectoryLookup.TryGetValue(fileExtension, out string outputDir))
                    outputDirectory = outputDir;

                bundles.Add(new Bundle(
                            bundleConfig.Name,
                            fileExtension,
                            outputDirectory,
                            bundleConfig.Files,
                            bundleConfig.OutputFileName,
                            bundleConfig.SubPath));
            }

            return bundles;
        }
    }
}
