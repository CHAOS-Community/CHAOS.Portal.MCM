using System.Collections.Generic;
using System.Linq;
using CHAOS.Portal.Exception;
using Chaos.Mcm.Data.EF;
using Chaos.Portal;
using Chaos.Portal.Data.Dto.Standard;
using ObjectType = Chaos.Mcm.Data.Dto.Standard.ObjectType;

namespace Chaos.Mcm.Extension
{
    public class ObjectType : AMcmExtension
    {
        #region Business Logic

        public Data.Dto.Standard.ObjectType Create(ICallContext callContext, string name)
		{
            if( !callContext.User.SystemPermissonsEnum.HasFlag( SystemPermissons.Manage ) )
                throw new InsufficientPermissionsException( "User does not have permission to create an Object Type" );

		    using( var db = DefaultMCMEntities )
		    {
		        var result = db.ObjectType_Create( name ).First().Value; 

		        return db.ObjectType_Get( result, null ).ToDTO().First();
		    }
		}

		public IEnumerable<Data.Dto.Standard.ObjectType> Get( ICallContext callContext )
		{
			using( var db = DefaultMCMEntities )
			{
				return db.ObjectType_Get( null, null ).ToDTO().ToList();
			}
		}

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
