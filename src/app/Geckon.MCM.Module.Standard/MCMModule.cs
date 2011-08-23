using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Geckon.MCM.Data.Linq;
using Geckon.Portal.Core;
using Geckon.Portal.Core.Standard.Extension;
using Geckon.Portal.Core.Standard.Module;
using Geckon.Portal.Data.Dto;
using ObjectType = Geckon.MCM.Data.Linq.DTO.ObjectType;

namespace Geckon.MCM.Module.Standard
{
    public class MCMModule : AModule
    {
        #region Properties

        private String ConnectionString { get; set; }
        public MCMDataContext DefaultMCMDataContext { get { return new MCMDataContext(ConnectionString); } }

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
        public ObjectType ObjectType_Create( CallContext callContext, string value  )
        {
            using( MCMDataContext db = DefaultMCMDataContext )
            {
                int result = db.ObjectType_Insert( value, callContext.User.SystemPermission ); 

                if( result == -100 )
                    throw new Portal.Core.Exception.InsufficientPermissionsExcention( "User does not have permission to create an Object Type" );

                return ObjectType.Create( db.ObjectType_Get( result, null ).First() );
            }
        }

        [Datatype("ObjectType", "Get")]
        public IEnumerable<ObjectType> ObjectType_Get( CallContext callContext )
        {
            using( MCMDataContext db = DefaultMCMDataContext )
            {
                IEnumerable<Data.Linq.ObjectType> results = db.ObjectType_Get( null, null );

                return ObjectType.Create( results ).ToList();
            }
        }

        [Datatype("ObjectType","Update")]
        public ScalarResult ObjectType_Update( CallContext callContext, int id, string newName )
        {
            using( MCMDataContext db = DefaultMCMDataContext )
            {
                int result = db.ObjectType_Update(id, newName, callContext.User.SystemPermission);

                if( result == -100 )
                    throw new Portal.Core.Exception.InsufficientPermissionsExcention( "User does not have permission to update an Object Type" );

                return new ScalarResult( result );
            }
        }

        [Datatype("ObjectType","Delete")]
        public ScalarResult ObjectType_Delete( CallContext callContext, int id )
        {
            using( MCMDataContext db = DefaultMCMDataContext )
            {
                int result = db.ObjectType_Delete( id, null, callContext.User.SystemPermission );

                if( result == -100 )
                    throw new Portal.Core.Exception.InsufficientPermissionsExcention( "User does not have permission to delete an Object Type" );

                return new ScalarResult( result );
            }
        }

        #endregion

        #endregion
    }
}
