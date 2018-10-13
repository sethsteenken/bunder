using System.Collections.Generic;

namespace Bunder
{
    /// <summary>
    /// Configuration that is used to build list of <see cref="Bundle"/> for Bunder.
    /// </summary>
    public interface IBundlingConfiguration
    {
        IEnumerable<Bundle> Build();
    }
}
