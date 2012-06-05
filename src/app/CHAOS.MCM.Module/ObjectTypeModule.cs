using System.Collections.Generic;
using System.Linq;
using CHAOS.MCM.Data.EF;
using CHAOS.Portal.Core.Module;
using CHAOS.Portal.Core;
using CHAOS.Portal.Exception;

namespace CHAOS.MCM.Module
{
    [Module("MCM")]
    public class ObjectTypeModule : AMCMModule
    {
        #region Business Logic

		#region ObjectType

		[Datatype("ObjectType","Create")]
		public ObjectType Create( ICallContext callContext, string value  )
		{
            if( !callContext.User.SystemPermissonsEnum.HasFlag( Portal.DTO.Standard.SystemPermissons.Manage ) )
                throw new InsufficientPermissionsException( "User does not have permission to create an Object Type" );

		    using( var db = DefaultMCMEntities )
		    {
		        var result = db.ObjectType_Create( value ).First().Value; 

		        return db.ObjectType_Get( result, null ).First();
		    }
		}

		[Datatype("ObjectType", "Get")]
		public IEnumerable<Data.DTO.ObjectType> ObjectType_Get( ICallContext callContext )
		{
			using( var db = DefaultMCMEntities )
			{
				return db.ObjectType_Get( null, null ).ToDTO().ToList();
			}
		}

		//[Datatype("ObjectType","Update")]
		//public ScalarResult ObjectType_Update(  CallContext callContext, int id, string newName )
		//{
		//    using( MCMEntities db = DefaultMCMEntities )
		//    {
		//        int result = db.ObjectType_Update(id, newName, callContext.User.SystemPermission);

		//        if( result == -100 )
		//            throw new Portal.Core.Exception.InsufficientPermissionsException( "User does not have permission to update an Object Type" );

		//        return new ScalarResult( result );
		//    }
		//}

		//[Datatype("ObjectType","Delete")]
		//public ScalarResult ObjectType_Delete( CallContext callContext, int id )
		//{
		//    using( MCMEntities db = DefaultMCMEntities )
		//    {
		//        int result = db.ObjectType_Delete( id, null, callContext.User.SystemPermission );

		//        if( result == -100 )
		//            throw new Portal.Core.Exception.InsufficientPermissionsException( "User does not have permission to delete an Object Type" );

		//        return new ScalarResult( result );
		//    }
		//}

		#endregion

		#endregion
    }

}
