using System.Runtime.Serialization;

namespace Geckon.MCM.Core.Exception
{
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
