namespace Bunder
{
    public interface IBundleLookup
    {
        bool TryGetBundle(string name, out Bundle bundle);
    }
}
