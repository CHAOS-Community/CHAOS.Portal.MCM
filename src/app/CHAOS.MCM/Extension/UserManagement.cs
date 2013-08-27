using System;
using System.Collections.Generic;
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

		public IList<Data.Dto.Standard.Folder> GetUserFolder(Guid? userGuid = null, bool createIfMissing = true)
		{
			if (!userGuid.HasValue)
				userGuid = Request.User.Guid;

			var userFolder = GetFolderFromPath(false, Configuration.UsersFolderName, userGuid.ToString());

			if (userFolder != null)
				return new List<Data.Dto.Standard.Folder>{userFolder};
			if(!createIfMissing)
				return new List<Data.Dto.Standard.Folder>();

			var usersFolder = GetFolderFromPath(false, Configuration.UsersFolderName);

			var userFolderId = McmRepository.FolderCreate(Request.User.Guid, null, userGuid.ToString(), usersFolder.ID, Configuration.UserFolderTypeId);

			if (McmRepository.ObjectCreate(userGuid.Value, Configuration.UserObjectTypeId, userFolderId) != 1)
				throw new System.Exception("Failed to create user object");

			return McmRepository.FolderGet(userFolderId);
		}

		#endregion
	}
}