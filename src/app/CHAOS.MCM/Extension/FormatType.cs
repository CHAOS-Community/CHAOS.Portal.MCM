using System.Collections.Generic;
using System.Linq;
using Chaos.Mcm.Data.EF;
using Chaos.Portal;

namespace Chaos.Mcm.Extension
{
	public class FormatType : AMcmExtension
	{
		public IEnumerable<Data.Dto.Standard.FormatType> Get(ICallContext callContext, string name)
		{
			using (MCMEntities db = DefaultMCMEntities)
			{
				return db.FormatType_Get(null, name).ToDto().ToList();
			}
		}

		//[Datatype("FormatType", "Create")]
		//public FormatType FormatType_Create(CallContext callContext, string name)
		//{
		//    using( MCMEntities db = DefaultMCMEntities )
		//    {
		//        int result = db.FormatType_Create(name, callContext.User.SystemPermission);

		//        if( result == -100 )
		//            throw new Portal.Core.Exception.InsufficientPermissionsException("User does not have permission to delete an Object Type");

		//        return db.FormatType_Get(result, null).First();
		//    }
		//}

		//[Datatype("FormatType", "Update")]
		//public ScalarResult FormatType_Update(CallContext callContext, int? id, string name)
		//{
		//    using( MCMEntities db = DefaultMCMEntities )
		//    {
		//        int result = db.FormatType_Update( id, name, callContext.User.SystemPermission );

		//        if( result == -100 )
		//            throw new Portal.Core.Exception.InsufficientPermissionsException("User does not have permission to delete an Object Type");

		//        return new ScalarResult(result);
		//    }
		//}

		//[Datatype("FormatType", "Delete")]
		//public ScalarResult FormatType_Delete(CallContext callContext, int? id)
		//{
		//    using( MCMEntities db = DefaultMCMEntities )
		//    {
		//        int result = db.FormatType_Delete(id, callContext.User.SystemPermission);

		//        if( result == -100 )
		//            throw new Portal.Core.Exception.InsufficientPermissionsException("User does not have permission to delete an Object Type");

		//        return new ScalarResult(result);
		//    }
		//}
	}
}