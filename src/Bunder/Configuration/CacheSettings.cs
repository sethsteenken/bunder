﻿using System;

namespace Bunder
{
    /// <summary>
    /// Settings that control the cache of the markup that is generated by Bunder.
    /// </summary>
    public sealed class CacheSettings
    {
        /// <summary>
        /// Flag on whether cache is enabled. Default is true.
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Duration of cache. Default is <see cref="TimeSpan.MaxValue"/>.
        /// </summary>
        public TimeSpan Duration { get; set; } = TimeSpan.MaxValue;
    }
}
