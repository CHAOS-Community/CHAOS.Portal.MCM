using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Chaos.Mcm.Data;
using Chaos.Mcm.Permission;
using Chaos.Portal.Core;
using Chaos.Portal.Core.Data.Model;

namespace Chaos.Mcm.Extension.v6
{
    using Domain;

    public class UserProfile : AMcmExtension
    {
        private IUserProfileController UserProfileController { get; set; }

		public UserProfile(IPortalApplication portalApplication, IMcmRepository mcmRepository, IPermissionManager permissionManager) : base(portalApplication, mcmRepository, permissionManager)
		{
            UserProfileController = new UserProfileController(mcmRepository);
		}

		#region Get

		public IList<Data.Dto.UserProfile> Get(Guid metadataSchemaGuid, Guid? userGuid = null)
		{
			if (!userGuid.HasValue)
				userGuid = Request.User.Guid;

            return UserProfileController.Get(userGuid.Value, metadataSchemaGuid);
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