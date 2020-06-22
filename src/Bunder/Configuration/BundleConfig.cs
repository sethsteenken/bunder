using System.Collections.Generic;
using System.IO;

namespace Bunder
{
    /// <summary>
    /// Simple POCO designed to be built/deserialized and transitioned to <see cref="Bundle"/> via <see cref="BundlingConfigurationBase"/>.
    /// </summary>
    public class BundleConfig
    {
        public static string DefaultExtension = "js";

        public string Name { get; set; }
        public string OutputFileName { get; set; }
        public string SubPath { get; set; }
        public IEnumerable<string> Files { get; set; }
        public string OutputDirectory { get; set; }

        public string OutputFileExtension
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(OutputFileName))
                    return Path.GetExtension(OutputFileName).Replace(".", "");

                string ext = null;

                foreach (var filePath in Files)
                {
                    ext = Path.GetExtension(filePath);
                    if (!string.IsNullOrEmpty(ext))
                        break;
                }

                if (string.IsNullOrWhiteSpace(ext))
                    return DefaultExtension;

                return ext.Replace(".", "");
            }
        }
    }
}
