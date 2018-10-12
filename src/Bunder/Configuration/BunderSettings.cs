using System.Collections.Generic;

namespace Bunder
{
    public sealed class BunderSettings
    {
        internal const string DefaultSectionName = "Bunder";
        internal static readonly IDictionary<string, string> DefaultOutputDirectoryLookup = new Dictionary<string, string>()
        {
            { "js", "/content/js" },
            { "css", "/content/css" }
        };

        public bool UseBundledOutput { get; set; } = false;
        public bool UseVersioning { get; set; } = true;
        public string BundlesConfigFilePath { get; set; } = "bundles.json";
        public IDictionary<string, string> OutputDirectories { get; set; } = DefaultOutputDirectoryLookup;
        public CacheSettings MarkupCache { get; set; } = new CacheSettings();
    }
}
