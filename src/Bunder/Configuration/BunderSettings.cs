﻿using System.Collections.Generic;

namespace Bunder
{
    /// <summary>
    /// Settings required for Bunder. By default, these settings will be read from appsettings.json "Bunder" section.
    /// </summary>
    public sealed class BunderSettings
    {
        /// <summary>
        /// Default section name for <see cref="BunderSettings"/> found in appsettings.json.
        /// </summary>
        public const string DefaultSectionName = "Bunder";

        /// <summary>
        /// Default lookup for output directories for given file extensions.
        /// </summary>
        public static readonly IDictionary<string, string> DefaultOutputDirectoryLookup = new Dictionary<string, string>()
        {
            { "js", "/content/js" },
            { "css", "/content/css" }
        };

        /// <summary>
        /// Flag on whether the bundled output should be rendered or a bundle's list of source files. Default is false.
        /// This value will typically be false in development, but true in production.
        /// </summary>
        public bool UseBundledOutput { get; set; } = false;

        /// <summary>
        /// Flag on whether to append versioning to the outputs generated by Bunder. Default is true.
        /// Appending versioning will allow for busting browser cache when the contents of the file(s) change.
        /// </summary>
        public bool UseVersioning { get; set; } = true;

        /// <summary>
        /// Path / filename of the Bundle configuration json file. Default value is "bundles.json".
        /// </summary>
        public string BundlesConfigFilePath { get; set; } = "bundles.json";

        /// <summary>
        /// Dictionary lookup of output directories based on a file's extension. Default value is <see cref="BunderSettings.DefaultOutputDirectoryLookup"/>.
        /// Default example: JavaScript (js) files will be output to "/content/js/".
        /// </summary>
        public IDictionary<string, string> OutputDirectories { get; set; } = DefaultOutputDirectoryLookup;

        /// <summary>
        /// Settings that control the cache of the markup that is generated by Bunder.
        /// </summary>
        public CacheSettings MarkupCache { get; set; } = new CacheSettings();
    }
}
