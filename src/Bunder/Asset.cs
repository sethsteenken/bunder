using System.Web;

namespace Bunder
{
    public sealed class Asset
    {
        public Asset(string value, bool isStatic, bool isBundle)
        {
            Value = isStatic ? value : HttpUtility.UrlEncode(value);
            IsStatic = IsStatic;
            IsBundle = isBundle;
        }

        public string Value { get; private set; }
        public bool IsStatic { get; private set; }
        public bool IsBundle { get; private set; }
    }
}
