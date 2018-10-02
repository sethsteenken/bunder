namespace Bunder
{
    public abstract class Asset
    {
        public Asset(string value)
            : this (value, false, false)
        {

        }

        public Asset(string value, bool isStatic, bool isBundle)
        {
            Value = value;
            IsStatic = isStatic;
            IsBundle = isBundle;
        }

        public string Value { get; private set; }
        public bool IsStatic { get; private set; } = false;
        public bool IsBundle { get; private set; } = false;

        public abstract string RenderedValue { get; }
    }
}
