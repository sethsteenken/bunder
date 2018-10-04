namespace Bunder
{
    public interface IVersioningFormatter
    {
        string GetVersionedPath(string virtualPath);
    }
}
