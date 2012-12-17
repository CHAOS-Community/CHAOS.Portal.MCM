using System;
using System.Collections.Generic;
using System.Xml.Linq;
using CHAOS;
using CHAOS.Portal.Exception;
using Chaos.Mcm.Data.Dto.Standard;
using Chaos.Portal;
using Chaos.Portal.Data.Dto.Standard;

namespace Chaos.Mcm.Extension
{
	public class UserProfile : AMcmExtension
	{
		public IEnumerable<Metadata> Get(ICallContext callContext, UUID userGUID, UUID metadataSchemaGUID)
		{
			if (callContext.IsAnonymousUser)
				throw new InsufficientPermissionsException("User must be logged in");

			throw new NotImplementedException();
		}

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

		}
	}
}