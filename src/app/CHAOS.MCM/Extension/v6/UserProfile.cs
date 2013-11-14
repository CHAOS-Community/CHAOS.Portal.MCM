using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using CHAOS.Extensions;
using Chaos.Mcm.Data;
using Chaos.Mcm.Data.Configuration;
using Chaos.Mcm.Permission;
using Chaos.Portal.Core;
using Chaos.Portal.Core.Data.Model;

namespace Chaos.Mcm.Extension.v6
{
	public class UserProfile : AMcmExtensionWithConfiguration<UserProfileConfiguration>
	{
		public UserProfile(IPortalApplication portalApplication, IMcmRepository mcmRepository, IPermissionManager permissionManager) : base(portalApplication, mcmRepository, permissionManager)
		{
		}

		#region Get

		public IList<Data.Dto.UserProfile> Get(Guid metadataSchemaGuid, Guid? userGuid = null)
		{
			if (!userGuid.HasValue)
				userGuid = Request.User.Guid;

			var userObject = McmRepository.ObjectGet(userGuid.Value, true);

			var result = new List<Data.Dto.UserProfile>();

			if (userObject == null || userObject.Metadatas == null)
				return result;

			var metadata = userObject.Metadatas.FirstOrDefault(m => m.MetadataSchemaGuid == metadataSchemaGuid);

			if(metadata != null)
				result.Add(new Data.Dto.UserProfile(metadata));

			return result;
		}

		#endregion
		#region Get

		public ScalarResult Set(Guid metadataSchemaGuid, XDocument metadata, Guid? userGuid = null)
		{
			if (!userGuid.HasValue)
				userGuid = Request.User.Guid;

			var userObject = McmRepository.ObjectGet(userGuid.Value, true);

			if(userObject == null)
				throw new NotImplementedException("User does not have an user object");

			var existingMetadata = userObject.Metadatas.DoIfIsNotNull( ms => Enumerable.FirstOrDefault<Data.Dto.Metadata>(ms, m => m.MetadataSchemaGuid == metadataSchemaGuid));
			var metadataGuid = existingMetadata == null ? Guid.NewGuid() : existingMetadata.Guid;
			var revision = existingMetadata == null ? 0 : existingMetadata.RevisionID;

			if(McmRepository.MetadataSet(userObject.Guid, metadataGuid, metadataSchemaGuid, null, revision, metadata, Request.User.Guid) != 1)
				throw new System.Exception("Failed to set user profile");

			return new ScalarResult(1);
		}

		#endregion
	}
}