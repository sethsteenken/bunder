using Microsoft.AspNetCore.Http;

namespace Bunder
{
    public interface IVersioningFormatter
    {
        string GetVersionedPath(PathString basePath, string virtualPath);
    }
}
