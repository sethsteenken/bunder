using System.Collections.Generic;

namespace Bunder
{
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
            OutputFileName = string.IsNullOrWhiteSpace(outputFileName) ? $"{Name}.min.{FileExtension}" : outputFileName?.Trim();
            SubPath = subPath?.Trim();

            OutputPath = string.Concat(outputDirectory, 
                            !string.IsNullOrWhiteSpace(SubPath) ? (string.Concat(SubPath, SubPath.EndsWith("/") ? "" : "/")) : string.Empty,
                            OutputFileName);
        }

        public string Name { get; private set; }
        public string OutputFileName { get; private set; }
        public string SubPath { get; private set; }
        public IReadOnlyList<string> Files { get; private set; }
        public string FileExtension { get; private set; }
        public string OutputPath { get; private set; }
    }
}
