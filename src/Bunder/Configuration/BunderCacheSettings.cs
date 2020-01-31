using System;

namespace Bunder
{
    /// <summary>
    /// Represent resolution cache settings.
    /// </summary>
    public class BunderCacheSettings
    {
        /// <summary>
        /// Whether cache should be enabled during the asset resolution process.
        /// </summary>
        public bool Enabled { get; set; } = false;

        /// <summary>
        /// How long cache should be retained after being created. Leave empty to have no cache expiration.
        /// </summary>
        public TimeSpan Duration { get; set; } = default;
    }
}
