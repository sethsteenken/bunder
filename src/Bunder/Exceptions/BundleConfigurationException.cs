using System;

namespace Bunder
{
    [Serializable]
    public class BundleConfigurationException : Exception
    {
        public BundleConfigurationException(string message) : base(message.Sanitize()) { }
        public BundleConfigurationException(string message, Exception inner) : base(message.Sanitize(), inner) { }
        protected BundleConfigurationException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

}
