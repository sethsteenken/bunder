using System.Collections.Generic;
using System.IO;

namespace Bunder
{
    /// <summary>
    /// An identifiable registration of a group of files that should be "bundled" together.
    /// This model's information intended to aid in generating a renderable list of <see cref="Asset"/>.
    /// </summary>
    public sealed class Bundle
    {
        public Bundle(string name, string extension, string outputDirectory, IReadOnlyList<string> files)
            : this (name, extension, outputDirectory, files, null)
        {

        }

        public Bundle(string name, string extension, string outputDirectory, IReadOnlyList<string> files, string outputFileName)
            : this(name, extension, outputDirectory, files, outputFileName, null)
        {

        }

        public Bundle(string name, string extension, string outputDirectory, IReadOnlyList<string> files, string outputFileName, string subPath)
        {
            Name = name?.Trim();
            FileExtension = extension?.Trim();
            Files = files ?? new List<string>();
            OutputFileName = string.IsNullOrWhiteSpace(outputFileName) ? $"{Name.Replace(" ", "_")}.min.{FileExtension}" : outputFileName?.Trim();
            SubPath = subPath?.Trim();

            OutputPath = PathHelper.Combine(outputDirectory, 
                            !string.IsNullOrWhiteSpace(SubPath) ? (string.Concat(SubPath, SubPath.EndsWith("/") ? "" : "/")) : string.Empty,
                            OutputFileName);
        }

        /// <summary>
        /// A unique identifying name for the bundle. If no <see cref="OutputFileName"/> is provided, this value will be used for the output filename.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Optional filename for the output file that represents the final bundled output of the list if <see cref="Files"/>.
        /// </summary>
        public string OutputFileName { get; private set; }

        /// <summary>
        /// Optional sub directory path for the output.
        /// </summary>
        public string SubPath { get; private set; }

        /// <summary>
        /// List of file paths that represent the content of the bundle.
        /// </summary>
        public IReadOnlyList<string> Files { get; private set; }

        /// <summary>
        /// The file extension intended for both all the list of <see cref="Files"/> and the <see cref="OutputFileName"/>.
        /// This value is used to group all bundle output files to a common directory based on said file's extension.
        /// </summary>
        public string FileExtension { get; private set; }

        /// <summary>
        /// The full output path to this bundle's final output file.
        /// </summary>
        public string OutputPath { get; private set; }
    }
}
