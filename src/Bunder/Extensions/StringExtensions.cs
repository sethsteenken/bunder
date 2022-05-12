namespace Bunder
{
    internal static class StringExtensions
    {
        public static string Sanitize(this string value, bool encode = true)
        {
            if (string.IsNullOrWhiteSpace(value))
                return value;

            // add any other sanitation steps needed for logging string values here
            return encode ? System.Web.HttpUtility.HtmlEncode(value) : value;
        }
    }
}
