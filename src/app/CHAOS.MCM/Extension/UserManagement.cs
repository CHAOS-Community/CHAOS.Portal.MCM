using System;
using Chaos.Mcm.Data;
using Chaos.Mcm.Data.Configuration;
using Chaos.Mcm.Permission;
using Chaos.Portal.Core;
using System.Linq;

namespace Chaos.Mcm.Extension
{
	public class UserManagement : AMcmExtensionWithConfiguration<UserManagementConfiguration>
	{
		#region Constructor

		public UserManagement(IPortalApplication portalApplication) : base(portalApplication)
		{
		}

		public UserManagement(IPortalApplication portalApplication, IMcmRepository mcmRepository, IPermissionManager permissionManager) : base(portalApplication, mcmRepository, permissionManager)
		{
		}

		#endregion
		#region GetUserFolder

		public Data.Dto.Standard.Folder GetUserFolder(Guid? userGuid = null, bool createIfMissing = true)
		{
			if (!userGuid.HasValue)
				userGuid = Request.User.Guid;

			var userFolder = GetFolderFromPath(false, Configuration.UsersFolderName, userGuid.ToString());

			if (userFolder != null || !createIfMissing)
				return userFolder;

			var usersFolder = GetFolderFromPath(false, Configuration.UsersFolderName);

			var id = McmRepository.FolderCreate(Request.User.Guid, null, userGuid.ToString(), usersFolder.ID, Configuration.UserFolderTypeId);

			return McmRepository.FolderGet(id).First();
		}

		#endregion
	}
}