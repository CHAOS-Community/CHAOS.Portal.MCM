using System;
using System.Collections.Generic;
using Chaos.Mcm.Data;
using Chaos.Mcm.Data.Configuration;
using Chaos.Mcm.Permission;
using Chaos.Portal.Core;

namespace Chaos.Mcm.Extension.v6
{
    using Domain;

    public class UserManagement : AMcmExtension
	{
		private readonly UserManagementConfiguration _configuration;
        private IUserManagementController UserManagementController { get; set; }

		#region Constructor

		public UserManagement(IPortalApplication portalApplication, IMcmRepository mcmRepository, IPermissionManager permissionManager, UserManagementConfiguration configuration) : base(portalApplication, mcmRepository, permissionManager)
		{
			_configuration = configuration;
            UserManagementController = new UserManagementController(mcmRepository, _configuration.UsersFolderName, configuration.UserFolderTypeId, configuration.UserObjectTypeId);
		}

		#endregion
		#region GetUserFolder

		public IList<Data.Dto.Standard.Folder> GetUserFolder(Guid? userGuid = null, bool createIfMissing = true)
		{
			if (!userGuid.HasValue)
				userGuid = Request.User.Guid;

            return UserManagementController.GetUserFolder(userGuid.Value, Request.User.Guid, createIfMissing);
		}

		#endregion
		#region GetUserObject

		public IList<Data.Dto.Object> GetUserObject(Guid? userGuid = null, bool createIfMissing = true, bool includeMetata = false, bool includeFiles = false)
		{
			if (!userGuid.HasValue)
				userGuid = Request.User.Guid;

		    return UserManagementController.GetUserObject(userGuid.Value, Request.User.Guid, createIfMissing, includeMetata, includeFiles);
		}

		#endregion
	}
}