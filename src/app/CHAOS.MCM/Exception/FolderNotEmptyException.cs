using System.Runtime.Serialization;

namespace Chaos.Mcm.Exception
{
    public class FolderNotEmptyException : System.Exception
    {
        public FolderNotEmptyException()
        {
        }

        public FolderNotEmptyException(string message) : base(message)
        {
        }

        public FolderNotEmptyException(string message, System.Exception innerException) : base(message, innerException)
        {
        }

        protected FolderNotEmptyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
