using System.Collections.Generic;

namespace Bunder
{
    public class BundleConfig
    {
        public string Name { get; set; }
        public string OutputFileName { get; set; }
        public string SubPath { get; set; }
        public IReadOnlyList<string> Files { get; set; }
        public string OutputDirectory { get; set; }
    }
}
