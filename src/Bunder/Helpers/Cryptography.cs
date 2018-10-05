using System.Security.Cryptography;

namespace Bunder
{
    internal static class Cryptography
    {
        /// <summary>
        /// Recreation of CryptographyAlgorithms - https://github.com/aspnet/Mvc/blob/4b83f7b510f22ea4acd7e383fd483301c77f5fec/src/Microsoft.AspNetCore.Mvc.Razor/Infrastructure/CryptographyAlgorithms.cs
        /// </summary>
        /// <returns></returns>
        public static SHA256 CreateSHA256()
        {
            try
            {
                return SHA256.Create();
            }
            // SHA256.Create is documented to throw this exception on FIPS compliant machines.
            // See: https://msdn.microsoft.com/en-us/library/z08hz7ad%28v=vs.110%29.aspx?f=255&MSPPError=-2147217396
            catch (System.Reflection.TargetInvocationException)
            {
                // Fallback to a FIPS compliant SHA256 algorithm.
                return new SHA256CryptoServiceProvider();
            }
        }
    }
}
