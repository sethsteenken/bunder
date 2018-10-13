using System.Collections.Generic;

namespace Bunder
{
    /// <summary>
    /// Simple POCO designed to be built/deserialized and transitioned to <see cref="Bundle"/> via <see cref="BundlingConfigurationBase"/>.
    /// </summary>
    public class BundleConfig
    {
        public string Name { get; set; }
        public string OutputFileName { get; set; }
        public string SubPath { get; set; }
        public IReadOnlyList<string> Files { get; set; }
        public string OutputDirectory { get; set; }
    }
}
