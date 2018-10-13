using System.Web;

namespace Bunder
{
    public sealed class Asset
    {
        public Asset(string value)
            : this (value, isStatic: false)
        {

        }

        public Asset(string value, bool isStatic)
            : this (value, isStatic, bundle: null)
        {

        }

        public Asset(string value, Bundle bundle)
            : this(value, isStatic: false, bundle: bundle)
        {

        }

        private Asset(string value, bool isStatic, Bundle bundle)
        {
            Value = isStatic ? value : HttpUtility.UrlEncode(value);
            IsStatic = isStatic;
            Bundle = bundle;
        }

        public string Value { get; private set; }
        public bool IsStatic { get; private set; }
        public bool IsBundle => Bundle != null;
        public Bundle Bundle { get; private set; }

        public override string ToString()
        {
            return Value;
        }
    }
}
