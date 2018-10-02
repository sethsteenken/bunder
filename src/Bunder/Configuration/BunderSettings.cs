using System;
using System.Collections.Generic;
using System.Text;

namespace Bunder
{
    public sealed class BunderSettings
    {
        internal const string DefaultSectionName = "Bunder";

        public bool UseBundledOutput { get; set; } = false;
        public bool UseVersioning { get; set; } = true;
        public string BundlesConfigFilePath { get; set; } = "bundles.json";
        public CacheSettings MarkupCache { get; set; } = new CacheSettings();
    }
}
