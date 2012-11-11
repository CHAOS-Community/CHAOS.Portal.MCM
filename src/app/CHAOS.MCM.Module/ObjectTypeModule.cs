using System.Collections.Generic;
using System.Linq;
using CHAOS.MCM.Data.EF;
using CHAOS.Portal.Core.Module;
using CHAOS.Portal.Core;
using CHAOS.Portal.DTO.Standard;
using CHAOS.Portal.Exception;
using ObjectType = CHAOS.MCM.Data.Dto.Standard.ObjectType;

namespace CHAOS.MCM.Module
{
    [Module("MCM")]
    public class ObjectTypeModule : AMCMModule
    {
        #region Business Logic

		[Datatype("ObjectType","Create")]
        public ObjectType Create(ICallContext callContext, string name)
		{
            if( !callContext.User.SystemPermissonsEnum.HasFlag( SystemPermissons.Manage ) )
                throw new InsufficientPermissionsException( "User does not have permission to create an Object Type" );

		    using( var db = DefaultMCMEntities )
		    {
		        var result = db.ObjectType_Create( name ).First().Value; 

		        return db.ObjectType_Get( result, null ).ToDTO().First();
		    }
		}

		[Datatype("ObjectType", "Get")]
		public IEnumerable<ObjectType> Get( ICallContext callContext )
		{
			using( var db = DefaultMCMEntities )
			{
				return db.ObjectType_Get( null, null ).ToDTO().ToList();
			}
		}

		[Datatype("ObjectType","Update")]
		public ScalarResult Update(  ICallContext callContext, uint id, string newName )
		{
            if( !callContext.User.SystemPermissonsEnum.HasFlag( SystemPermissons.Manage ) )
                throw new InsufficientPermissionsException( "User does not have permission to create an Object Type" );

		    using( var db = DefaultMCMEntities )
		    {
		        var result = db.ObjectType_Update( (int?) id, newName ).First();

		        if( result.Value == -100 )
		            throw new InsufficientPermissionsException( "User does not have permission to update an Object Type" );

		        return new ScalarResult( result.Value );
		    }
		}

		[Datatype("ObjectType","Delete")]
		public ScalarResult Delete( ICallContext callContext, uint id )
		{
            if( !callContext.User.SystemPermissonsEnum.HasFlag( SystemPermissons.Manage ) )
                throw new InsufficientPermissionsException( "User does not have permission to create an Object Type" );

		    using( var db = DefaultMCMEntities )
		    {
		        var result = db.ObjectType_Delete( (int?) id, null ).First();

		        if( result.Value == -100 )
		            throw new InsufficientPermissionsException( "User does not have permission to delete an Object Type" );

		        return new ScalarResult( result.Value );
		    }
		}

		#endregion
    }

}
