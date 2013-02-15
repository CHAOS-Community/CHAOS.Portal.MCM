namespace Chaos.Mcm.Extension
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Chaos.Mcm.Data.Dto;
    using Chaos.Mcm.Data.Dto.Standard;
    using Chaos.Portal;
    using Chaos.Portal.Data.Dto.Standard;
    using Chaos.Portal.Exceptions;
    using FolderPermission = Chaos.Mcm.Data.Dto.FolderPermission;
    using IFolder = Chaos.Mcm.Permission.IFolder;
    
    public class Folder : AMcmExtension
    {
        #region Permission

        public FolderPermission GetPermission( ICallContext callContext, uint folderID )
        {
            var folder           = PermissionManager.GetFolders(folderID);
            var userPermissions  = folder.UserPermissions.Select(item => new EntityPermission
                                                                            {
                                                                                Guid       = item.Key,
                                                                                Permission = item.Value
                                                                            });
            var groupPermissions = folder.GroupPermissions.Select(item => new EntityPermission
                                                                              {
                                                                                  Guid       = item.Key,
                                                                                  Permission = item.Value
                                                                              });

            return new FolderPermission( userPermissions, groupPermissions );
        }

        public ScalarResult SetPermission(ICallContext callContext, Guid? userGuid, Guid? groupGuid, uint folderID, uint permission)
        {
            if (!userGuid.HasValue && !groupGuid.HasValue)
                throw new ArgumentException("Both userGUID and groupGUID can't be null at the same time");
            
            var result = 0;
            var folder = PermissionManager.GetFolders(folderID);

            // REVIEW: What permissions are required to remove a permission?
            if (!folder.DoesUserOrGroupHavePermission(callContext.User.Guid, callContext.Groups.Select(item => item.Guid), (Chaos.Mcm.Permission.FolderPermission)permission))
                throw new InsufficientPermissionsException( "User does not have permission to give the requested permissions" );

            if (userGuid.HasValue)
                result += (int) McmRepository.FolderUserJoinSet(userGuid.Value, folderID, permission);
            if (groupGuid.HasValue)
                result += (int) McmRepository.FolderGroupJoinSet(groupGuid.Value, folderID, permission);

            return new ScalarResult( result );
        }

        #endregion
        
		public IEnumerable<IFolderInfo> Get( ICallContext callContext, uint? id, uint? folderTypeID, uint? parentID, uint? permission )
		{
            if (parentID.HasValue && id.HasValue)
                throw new ArgumentException("It does not make sense to specficy both ID and ParentID in the same query");

            var permissionEnum = (Chaos.Mcm.Permission.FolderPermission)(permission ?? (uint)Chaos.Mcm.Permission.FolderPermission.Read) | Chaos.Mcm.Permission.FolderPermission.Read;
            var userGuid       = callContext.User.Guid;
            var groupGuids     = callContext.Groups.Select( group => group.Guid ).ToList();

            IEnumerable<IFolder> folderResults;

			if( !parentID.HasValue && !id.HasValue )
                folderResults = PermissionManager.GetFolders(permissionEnum, userGuid, groupGuids);
            else
            if( parentID.HasValue )
                folderResults = PermissionManager.GetFolders(parentID.Value).GetSubFolders().Where(item => item.DoesUserOrGroupHavePermission(userGuid, groupGuids, permissionEnum));
            else
                folderResults = new[] { PermissionManager.GetFolders(id.Value) }.Where(item => item.DoesUserOrGroupHavePermission(userGuid, groupGuids, permissionEnum));
            
            return RetrieveFolderInfos(folderResults);
		}

        private IEnumerable<IFolderInfo> RetrieveFolderInfos(IEnumerable<IFolder> folders)
        {
            var folderIDs = folders.Select(folder => folder.ID).ToList();

            return McmRepository.GetFolderInfo(folderIDs);
        }

        public ScalarResult Delete(ICallContext callContext, uint id)
        {
            var userGuid   = callContext.User.Guid;
            var groupGuids = callContext.Groups.Select(group => group.Guid).ToList();

            if(!PermissionManager.GetFolders(id).DoesUserOrGroupHavePermission(userGuid, groupGuids, Chaos.Mcm.Permission.FolderPermission.Delete))
                throw new InsufficientPermissionsException("User does not have permission to delete the folder");

            var result = McmRepository.FolderDelete(id);

            return new ScalarResult(result);
        }

		public ScalarResult Update( ICallContext callContext, uint id, string newTitle, uint? newFolderTypeID, uint? newParentID )
		{
            if (!PermissionManager.GetFolders(id).DoesUserOrGroupHavePermission(callContext.User.Guid, callContext.Groups.Select(item => item.Guid), Chaos.Mcm.Permission.FolderPermission.Update))
				throw new InsufficientPermissionsException( "User does not have permission to give the requested permissions" );

			var result = McmRepository.FolderUpdate(id, newTitle, newFolderTypeID, newParentID);

			return new ScalarResult( (int) result );
		}

        public IFolderInfo Create(ICallContext callContext, Guid? subscriptionGuid, string title, uint? parentID, uint folderTypeID)
		{
            if( !subscriptionGuid.HasValue == null && !parentID.HasValue )
                throw new ArgumentException( "Both parentID and subscriptionGuid can't be null" );

		    var userGuid     = callContext.User.Guid;
		    var groupGuids   = callContext.Groups.Select(item => item.Guid);
            var subscription = callContext.Subscriptions.FirstOrDefault( sub => sub.Guid.ToString() == subscriptionGuid.ToString() );

		    if( subscription != null && subscription.Permission != SubscriptionPermission.CreateFolder )
		        throw new InsufficientPermissionsException( "User does not have permission to create topfolders with the subscriptionGuid" );
		    
            if(parentID.HasValue &&!PermissionManager.GetFolders((uint) parentID).DoesUserOrGroupHavePermission(userGuid, groupGuids, Chaos.Mcm.Permission.FolderPermission.Write))
                throw new InsufficientPermissionsException("User does not have permission to create subfolders");

            var result = McmRepository.FolderCreate(userGuid, subscription == null ? (Guid?) null : subscription.Guid, title, parentID, folderTypeID);

            return McmRepository.GetFolderInfo(new[] { result }).First();
		}
    }
}
