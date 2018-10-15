using System.Collections.Generic;

namespace Bunder.Tests
{
    internal static class BundleTestHelper
    {
        public static Bundle GetSampleBundle(string name = null)
        {
            return new Bundle(string.IsNullOrWhiteSpace(name) ? "MyBundle" : name, 
                "js", 
                "/my/output/", 
                new List<string>()
                {
                    "/my/source/one.js",
                    "/my/source/two.js"
                }
            );
        }
    }
}
