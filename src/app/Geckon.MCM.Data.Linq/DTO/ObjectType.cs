using System.Collections.Generic;
using Geckon.Serialization.Xml;

namespace Geckon.MCM.Data.Linq.DTO
{
    [Document("Geckon.MCM.Data.Linq.DTO.ObjectType")]
    public class ObjectType : XmlSerialize
    {
        #region Properties

        [Element]
        public int    ID { get; set; }
        
        [Element]
        public string Value { get; set; }

        #endregion
        #region Construction

        public static ObjectType Create( Linq.ObjectType obj )
        {
            ObjectType objectType = new ObjectType();

            objectType.ID    = obj.ID;
            objectType.Value = obj.Value;

            return objectType;
        }

        public static IEnumerable<ObjectType> Create( IEnumerable<Linq.ObjectType> objects )
        {
            if( objects == null )
                yield break;

            foreach( Linq.ObjectType objectType in objects )
            {
                yield return Create( objectType );
            }
        }

        #endregion
    }
}
