using System.Collections.Generic;
using System.Linq;
using Chaos.Mcm.Data.EF;
using Chaos.Portal;

namespace Chaos.Mcm.Extension
{
	public class FolderType : AMcmExtension
	{
		public IEnumerable<Data.Dto.Standard.FolderType> Get(ICallContext callContext, string name)
		{
			using (MCMEntities db = DefaultMCMEntities)
			{
				return db.FolderType_Get(null, name).ToDto().ToList();
			}
		}

		//[Datatype("FolderType", "Create")]
		//public FolderType FolderType_Create(CallContext callContext, string name)
		//{
		//    using (MCMEntities db = DefaultMCMEntities)
		//    {
		//        int result = db.FolderType_Create(name, callContext.User.SystemPermission);

		//        if (result == -100)
		//            throw new Portal.Core.Exception.InsufficientPermissionsException("User does not have permission to delete an Object Type");

		//        return db.FolderType_Get(result, null).First();
		//    }
		//}

		//[Datatype("FolderType", "Update")]
		//public ScalarResult FolderType_Update(CallContext callContext, int? id, string name)
		//{
		//    using (MCMEntities db = DefaultMCMEntities)
		//    {
		//        int result = db.FolderType_Update(id, name, callContext.User.SystemPermission);

		//        if (result == -100)
		//            throw new Portal.Core.Exception.InsufficientPermissionsException("User does not have permission to delete an Object Type");

		//        return new ScalarResult(result);
		//    }
		//}

		//[Datatype("FolderType", "Delete")]
		//public ScalarResult FolderType_Delete(CallContext callContext, int? id)
		//{
		//    using (MCMEntities db = DefaultMCMEntities)
		//    {
		//        int result = db.FolderType_Delete(id, callContext.User.SystemPermission);

		//        if (result == -100)
		//            throw new Portal.Core.Exception.InsufficientPermissionsException("User does not have permission to delete an Object Type");

		//        return new ScalarResult(result);
		//    }
		//}
	}
}