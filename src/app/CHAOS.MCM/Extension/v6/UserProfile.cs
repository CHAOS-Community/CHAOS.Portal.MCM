using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using CHAOS.Extensions;
using Chaos.Mcm.Data;
using Chaos.Mcm.Permission;
using Chaos.Portal.Core;
using Chaos.Portal.Core.Data.Model;

namespace Chaos.Mcm.Extension.v6
{
    using Domain;

    public class UserProfile : AMcmExtension
	{
        private UserProfileController UserProfileController { get; set; }

		public UserProfile(IPortalApplication portalApplication, IMcmRepository mcmRepository, IPermissionManager permissionManager) : base(portalApplication, mcmRepository, permissionManager)
		{
            UserProfileController = new UserProfileController(mcmRepository);
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
		#region Set

		public ScalarResult Set(Guid metadataSchemaGuid, XDocument metadata, Guid? userGuid = null)
		{
			if (!userGuid.HasValue)
				userGuid = Request.User.Guid;

            UserProfileController.Set(userGuid.Value, metadataSchemaGuid, metadata, Request.User.Guid);

			return new ScalarResult(1);
		}

		#endregion
	}
}