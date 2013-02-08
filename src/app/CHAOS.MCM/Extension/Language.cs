using System.Collections.Generic;
using System.Linq;
using Chaos.Mcm.Data.EF;
using Chaos.Portal;

namespace Chaos.Mcm.Extension
{
	public class Language : AMcmExtension
	{
		public IEnumerable<Data.Dto.Standard.Language> Get(ICallContext callContext, string name, string languageCode)
		{
			using (MCMEntities db = DefaultMCMEntities)
			{
				return db.Language_Get(name, languageCode).ToDto().ToList();
			}
		}

		//[Datatype("Language", "Create")]
		//public Language Language_Create( CallContext callContext, string name, string languageCode )
		//{
		//    using( MCMEntities db = DefaultMCMEntities )
		//    {
		//        int result = db.Language_Create( name, languageCode, callContext.User.SystemPermission );

		//        if( result == -100 )
		//            throw new Portal.Core.Exception.InsufficientPermissionsException( "User does not have permission to delete an Object Type" );

		//        return db.Language_Get( name, languageCode ).First();
		//    }
		//}

		//[Datatype("Language", "Update")]
		//public ScalarResult Language_Update( CallContext callContext, string languageCode, string newName )
		//{
		//    using( MCMEntities db = DefaultMCMEntities )
		//    {
		//        int result = db.Language_Update( newName, languageCode, callContext.User.SystemPermission );

		//        if( result == -100 )
		//            throw new Portal.Core.Exception.InsufficientPermissionsException( "User does not have permission to delete an Object Type" );

		//        return new ScalarResult( result );
		//    }
		//}

		//[Datatype("Language", "Delete")]
		//public ScalarResult Language_Delete( CallContext callContext, string languageCode )
		//{
		//    using( MCMEntities db = DefaultMCMEntities )
		//    {
		//        int result = db.Language_Delete( languageCode, callContext.User.SystemPermission );

		//        if( result == -100 )
		//            throw new Portal.Core.Exception.InsufficientPermissionsException( "User does not have permission to delete an Object Type" );

		//        return new ScalarResult( result );
		//    }
		//}
	}
}