using System;
using System.Xml.Linq;
using Chaos.Mcm.Data;
using Chaos.Mcm.Data.Configuration;
using Chaos.Mcm.Permission;
using Chaos.Portal.Core;
using Chaos.Portal.Core.Data.Model;
using System.Linq;
using CHAOS.Extensions;

namespace Chaos.Mcm.Extension
{
	public class UserProfile : AMcmExtensionWithConfiguration<UserProfileConfiguration>
	{
		public UserProfile(IPortalApplication portalApplication) : base(portalApplication)
		{
		}

		public UserProfile(IPortalApplication portalApplication, IMcmRepository mcmRepository, IPermissionManager permissionManager) : base(portalApplication, mcmRepository, permissionManager)
		{
		}

		#region Get

		public Data.Dto.UserProfile Get(Guid metadataSchemaGuid, Guid? userGuid = null)
		{
			if (!userGuid.HasValue)
				userGuid = Request.User.Guid;

			var userObject = McmRepository.ObjectGet(userGuid.Value, true);

			if (userObject == null || userObject.Metadatas == null)
				return null;

			var metadata = userObject.Metadatas.FirstOrDefault(m => m.MetadataSchemaGuid == metadataSchemaGuid);

			return metadata == null ? null : new Data.Dto.UserProfile(metadata);
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

			var existingMetadata = userObject.Metadatas.DoIfIsNotNull( ms => ms.FirstOrDefault(m => m.MetadataSchemaGuid == metadataSchemaGuid));
			var metadataGuid = existingMetadata == null ? Guid.NewGuid() : existingMetadata.Guid;
			var revision = existingMetadata == null ? 0 : existingMetadata.RevisionID;

			if(McmRepository.MetadataSet(userObject.Guid, metadataGuid, metadataSchemaGuid, null, revision, metadata, Request.User.Guid) != 1)
				throw new System.Exception("Failed to set user profile");

			return new ScalarResult(1);
		}

		#endregion
	}
}