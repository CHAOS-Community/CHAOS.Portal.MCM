using System.Collections.Generic;
using System.Linq;
using Chaos.Mcm.Data.EF;
using Chaos.Portal;

namespace Chaos.Mcm.Extension
{
	public class ObjectRelationType : AMcmExtension
	{
		public IEnumerable<Data.Dto.Standard.ObjectRelationType> Get(ICallContext callContext, string value)
		{
			using (MCMEntities db = DefaultMCMEntities)
			{
				return db.ObjectRelationType_Get(null, value).ToDto().ToList();
			}
		}

		//[Datatype("ObjectRelationType", "Create")]
		//public ObjectRelationType ObjectRelationType_Create(CallContext callContext, string value)
		//{
		//    using( MCMEntities db = DefaultMCMEntities )
		//    {
		//        int result = db.ObjectRelationType_Create(value, callContext.User.SystemPermission);

		//        if( result == -100 )
		//            throw new Portal.Core.Exception.InsufficientPermissionsException( "User does not have permission to delete an Object Type" );

		//        return db.ObjectRelationType_Get(result, null).First();
		//    }
		//}

		//[Datatype("ObjectRelationType", "Update")]
		//public ScalarResult ObjectRelationType_Update(CallContext callContext, int? id, string value)
		//{
		//    using( MCMEntities db = DefaultMCMEntities )
		//    {
		//        int result = db.ObjectRelationType_Update(id, value, callContext.User.SystemPermission);

		//        if( result == -100 )
		//            throw new Portal.Core.Exception.InsufficientPermissionsException( "User does not have permission to delete an Object Type" );

		//        return new ScalarResult( result );
		//    }
		//}

		//[Datatype("ObjectRelationType", "Delete")]
		//public ScalarResult ObjectRelationType_Delete( CallContext callContext, int? id )
		//{
		//    using( MCMEntities db = DefaultMCMEntities )
		//    {
		//        int result = db.ObjectRelationType_Delete(id, callContext.User.SystemPermission);

		//        if( result == -100 )
		//            throw new Portal.Core.Exception.InsufficientPermissionsException( "User does not have permission to delete an Object Type" );

		//        return new ScalarResult( result );
		//    }
		//}
	}
}