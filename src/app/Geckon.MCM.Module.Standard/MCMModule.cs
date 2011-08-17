using System;
using System.Linq;
using System.Xml.Linq;
using Geckon.MCM.Data.Linq;
using Geckon.Portal.Core;
using Geckon.Portal.Core.Extension;
using Geckon.Portal.Core.Standard.Module;
using ObjectType = Geckon.MCM.Data.Linq.DTO.ObjectType;

namespace Geckon.MCM.Module.Standard
{
    public class MCMModule : AModule
    {
        #region Properties

        private String ConnectionString { get; set; }

        #endregion
        #region Construction

        public override void Init( XElement config )
        {
            ConnectionString = config.Attribute( "ConnectionString" ).Value;
        }

        #endregion
        #region Business Logic

        #region ObjectType

        [Datatype("ObjectType","Create")]
        public ObjectType ObjectType_Create( ICallContext callContext, string value  )
        {
            using( MCMDataContext db = new MCMDataContext( ConnectionString ) )
            {
                int result = db.ObjectType_Insert( value, callContext.User.SystemPermission ); 

                if( result == -100 )
                    throw new Portal.Core.Exception.InsufficientPermissionsExcention( "User does not have permission to create an Object Type" );

                return ObjectType.Create( db.ObjectType_Get( result, null ).First() );
            }
        }

        #endregion

        #endregion
    }
}
