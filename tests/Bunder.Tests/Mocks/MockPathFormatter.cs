namespace Bunder.Tests
{
    internal class MockPathFormatter : IPathFormatter
    {
        public string GetFullPath(string virtualPath, bool includeVersioning)
        {
            return virtualPath;
        }
    }
}
