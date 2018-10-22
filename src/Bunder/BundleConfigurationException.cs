using System;

namespace Bunder
{
    [Serializable]
    public class BundleConfigurationException : Exception
    {
        public BundleConfigurationException() { }
        public BundleConfigurationException(string message) : base(message) { }
        public BundleConfigurationException(string message, Exception inner) : base(message, inner) { }
        protected BundleConfigurationException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

}
