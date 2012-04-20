using System.Runtime.Serialization;

namespace CHAOS.MCM.Core.Exception
{
    public class ObjectRelationAlreadyExistException : System.Exception
    {
        public ObjectRelationAlreadyExistException()
        {
        }

        public ObjectRelationAlreadyExistException(string message) : base(message)
        {
        }

        public ObjectRelationAlreadyExistException(string message, System.Exception innerException) : base(message, innerException)
        {
        }

        protected ObjectRelationAlreadyExistException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
