using System.Runtime.Serialization;

namespace Chaos.Mcm.Exception
{
  using System;

  [Serializable]
    public class InvalidRevisionException : System.Exception
    {
        public InvalidRevisionException()
        {
        }

        public InvalidRevisionException(string message) : base(message)
        {
        }

        public InvalidRevisionException(string message, System.Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidRevisionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
