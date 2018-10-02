using System;
using System.Collections.Generic;
using System.Text;

namespace Bunder
{
    public abstract class Bundle
    {
        private string _fileName;

        public string Name { get; set; }

        public virtual string Filename
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_fileName))
                    _fileName = $"{Name}.min.{FileExtension}";

                return _fileName;
            }
            set { _fileName = value; }
        }

        public string SubPath { get; set; }
        public IReadOnlyList<string> Files { get; set; }
        public virtual string Type => GetType().Name.Replace("Bundle", "");
        public abstract string FileExtension { get; }

        public virtual string GetFullOutputPath(string outputDirectory)
        {
            var hasSubPath = !string.IsNullOrWhiteSpace(SubPath);
            return string.Concat(outputDirectory,
                hasSubPath ? (string.Concat(SubPath, SubPath.EndsWith("/") ? "" : "/")) : string.Empty,
                Filename);
        }
    }
}
