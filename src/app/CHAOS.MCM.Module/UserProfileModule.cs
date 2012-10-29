using System.Collections.Generic;
using CHAOS.MCM.Data.DTO;
using CHAOS.Portal.Core;
using CHAOS.Portal.Core.Module;
using CHAOS.Portal.DTO.Standard;
using CHAOS.Portal.Exception;

namespace CHAOS.MCM.Module
{
	[Module("MCM")]
	public class UserProfileModule : AMCMModule
	{
		[Datatype("UserProfile", "Get")]
		public IEnumerable<Metadata> Get(ICallContext callContext, UUID userGUID, UUID schemaGUID)
		{
			if (callContext.IsAnonymousUser)
				throw new InsufficientPermissionsException("User must be logged in");


		}

		[Datatype("UserProfile", "Set")]
		public ScalarResult Set(ICallContext callContext, UUID userGUID, UUID schemaGUID, string profileXML)
		{
			if (callContext.IsAnonymousUser)
				throw new InsufficientPermissionsException("User must be logged in");



			return new ScalarResult(1);
		}
	}
}