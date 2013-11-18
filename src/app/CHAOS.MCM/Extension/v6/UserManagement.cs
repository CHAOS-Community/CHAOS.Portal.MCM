﻿using System;
using System.Collections.Generic;
using System.Linq;
using Chaos.Mcm.Data;
using Chaos.Mcm.Data.Configuration;
using Chaos.Mcm.Permission;
using Chaos.Portal.Core;

namespace Chaos.Mcm.Extension.v6
{
	public class UserManagement : AMcmExtension
	{
		private readonly UserManagementConfiguration _configuration;

		#region Constructor

		public UserManagement(IPortalApplication portalApplication, IMcmRepository mcmRepository, IPermissionManager permissionManager, UserManagementConfiguration configuration) : base(portalApplication, mcmRepository, permissionManager)
		{
			_configuration = configuration;
		}

		#endregion
		#region GetUserFolder

		public IList<Data.Dto.Standard.Folder> GetUserFolder(Guid? userGuid = null, bool createIfMissing = true)
		{
			if (!userGuid.HasValue)
				userGuid = Request.User.Guid;

			var userFolder = GetFolderFromPath(false, _configuration.UsersFolderName, userGuid.ToString());

			if (userFolder != null)
				return new List<Data.Dto.Standard.Folder>{userFolder};
			if(!createIfMissing)
				return new List<Data.Dto.Standard.Folder>();

			var usersFolder = GetFolderFromPath(false, _configuration.UsersFolderName);

			var userFolderId = McmRepository.FolderCreate(Request.User.Guid, null, userGuid.ToString(), usersFolder.ID, _configuration.UserFolderTypeId);

			return McmRepository.FolderGet(userFolderId);
		}

		#endregion
		#region GetUserObject

		public IList<Data.Dto.Object> GetUserObject(Guid? userGuid = null, bool createIfMissing = true, bool includeMetata = false, bool includeFiles = false)
		{
			if (!userGuid.HasValue)
				userGuid = Request.User.Guid;

			var @object = McmRepository.ObjectGet(userGuid.Value, includeMetata, includeFiles);

			if (@object != null)
				return new List<Data.Dto.Object> { @object };
			if (!createIfMissing)
				return new List<Data.Dto.Object>();

			var userFolder = GetUserFolder(userGuid).First();

			if (McmRepository.ObjectCreate(userGuid.Value, _configuration.UserObjectTypeId, userFolder.ID) != 1)
				throw new System.Exception("Failed to create user object");

			return new List<Data.Dto.Object> {McmRepository.ObjectGet(userGuid.Value, includeMetata, includeFiles)};
		}

		#endregion
	}
}