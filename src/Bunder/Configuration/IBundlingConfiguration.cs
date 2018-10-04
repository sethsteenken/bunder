using System.Collections.Generic;

namespace Bunder
{
    public interface IBundlingConfiguration
    {
        IEnumerable<Bundle> Build();
    }
}
