namespace Bunder
{
    public interface IPathFormatter
    {
        string GetFullPath(string virtualPath, bool includeVersioning);
    }
}
