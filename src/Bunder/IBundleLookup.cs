namespace Bunder
{
    /// <summary>
    /// Lookup provider that will try to find a registered <see cref="Bundle"/> based off a given name value.
    /// </summary>
    public interface IBundleLookup
    {
        /// <summary>
        /// Try to find a registered <paramref name="bundle"/> based on <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The unique name of the registered <see cref="Bundle"/>.</param>
        /// <param name="bundle">The registered bundle found matching <paramref name="name"/>.</param>
        /// <returns>True if bundle is found. False if not found.</returns>
        bool TryGetBundle(string name, out Bundle? bundle);
    }
}
