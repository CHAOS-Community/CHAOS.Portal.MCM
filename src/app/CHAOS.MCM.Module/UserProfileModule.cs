using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using CHAOS.MCM.Data.Dto.Standard;
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
		public IEnumerable<Metadata> Get(ICallContext callContext, UUID userGUID, UUID metadataSchemaGUID)
		{
			if (callContext.IsAnonymousUser)
				throw new InsufficientPermissionsException("User must be logged in");

			throw new NotImplementedException();
		}

		[Datatype("UserProfile", "Set")]
		public ScalarResult Set(ICallContext callContext, UUID metadataSchemaGUID, string profileXML)
		{
			if (callContext.IsAnonymousUser)
				throw new InsufficientPermissionsException("User must be logged in");

			XDocument.Parse( profileXML ); //TODO: Real xml validation against schema

			throw new NotImplementedException();

			/*
			using (var db = DefaultMCMEntities)
			{
				var result = db.Metadata_Set(new UUID().ToByteArray(), callContext.User.GUID.ToByteArray(), metadataSchemaGUID.ToByteArray(), null, (int?)revisionID, profileXML, callContext.User.GUID.ToByteArray()).First().Value;

				if (result <= 0)
					throw new UnhandledException("UserProfile Set was rolledback due to an unhandled exception");
			}*/

			return new ScalarResult(1);
		}
	}
}