using System;
using System.Collections.Generic;
using System.IO;

namespace Bunder
{
    public abstract class BundlingConfigurationBase : IBundlingConfiguration
    {
        protected BundlingConfigurationBase(IDictionary<string, string> outputDirectoryLookup)
        {
            Guard.IsNotNull(outputDirectoryLookup, nameof(outputDirectoryLookup));

            OutputDirectoryLookup = outputDirectoryLookup;
        }

        protected IDictionary<string, string> OutputDirectoryLookup { get; private set; }

        protected abstract IReadOnlyList<BundleConfig> GetBundleConfiguration();

        public virtual IEnumerable<Bundle> Build()
        {
            var bundleConfigs = GetBundleConfiguration();

            foreach (var bundleConfig in bundleConfigs)
            {
                if (bundleConfig.Files == null || bundleConfig.Files.Count == 0)
                    throw new InvalidOperationException("Bundle must have at least one file under Files reference.");

                string fileExtension = Path.GetExtension(string.IsNullOrWhiteSpace(bundleConfig.OutputFileName) ? bundleConfig.Files[0] : bundleConfig.OutputFileName);
                string outputDirectory = bundleConfig.OutputDirectory;

                if (string.IsNullOrWhiteSpace(outputDirectory) && OutputDirectoryLookup.TryGetValue(fileExtension, out string outputDir))
                    outputDirectory = outputDir;

                // TODO - add validation here?

                yield return new Bundle(
                            bundleConfig.Name,
                            fileExtension,
                            outputDirectory,
                            bundleConfig.Files,
                            bundleConfig.OutputFileName,
                            bundleConfig.SubPath);
            }
        }
    }
}
