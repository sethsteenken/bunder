using System;

namespace Bunder
{
    public sealed class CacheSettings
    {
        public bool Enabled { get; set; } = true;
        public TimeSpan Duration { get; set; } = TimeSpan.MaxValue;
    }
}
