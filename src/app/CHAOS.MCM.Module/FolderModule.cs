using System;
using System.Collections.Generic;
using System.Linq;
using CHAOS.Extensions;
using CHAOS.MCM.Core.Exception;
using CHAOS.MCM.Data.Dto;
using CHAOS.MCM.Data.Dto.Standard;
using CHAOS.MCM.Data.EF;
using CHAOS.MCM.Permission;
using CHAOS.Portal.Core;
using CHAOS.Portal.Core.Module;
using CHAOS.Portal.DTO.Standard;
using CHAOS.Portal.Exception;
using FolderInfo       = CHAOS.MCM.Data.Dto.Standard.FolderInfo;
using FolderPermission = CHAOS.MCM.Data.Dto.Standard.FolderPermission;
using IFolder          = CHAOS.MCM.Permission.IFolder;

namespace CHAOS.MCM.Module
{
    [Module("MCM")]
    public class FolderModule : AMCMModule
    {
        #region Permission

        [Datatype("Folder","GetPermission")]
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

        [Datatype("Folder","SetPermission")]
        public ScalarResult SetPermission( ICallContext callContext, UUID userGUID, UUID groupGUID, uint folderID, uint permission )
        {
            if (userGUID == null && groupGUID == null)
                throw new ArgumentException("Both userGUID and groupGUID can't be null at the same time");
            
            var result = 0;
            var folder = PermissionManager.GetFolders(folderID);

            // REVIEW: What permissions are required to remove a permission?
            if (!folder.DoesUserOrGroupHavePermission(callContext.User.GUID.ToGuid(), callContext.Groups.Select(item => item.GUID.ToGuid()), (Permission.FolderPermission)permission))
                throw new InsufficientPermissionsException( "User does not have permission to give the requested permissions" );

            if (userGUID != null)
                result += (int) McmRepository.SetFolderUserJoin(userGUID.ToGuid(), folderID, permission);
            if (groupGUID != null)
                result += (int) McmRepository.SetFolderGroupJoin(groupGUID.ToGuid(), folderID, permission);

            return new ScalarResult( result );
        }

        #endregion
        
        [Datatype("Folder", "Get")]
		public IEnumerable<FolderInfo> Get( ICallContext callContext, uint? id, uint? folderTypeID, uint? parentID, uint? permission )
		{
            if (parentID.HasValue && id.HasValue)
                throw new ArgumentException("It does not make sense to specficy both ID and ParentID in the same query");

            var permissionEnum = (Permission.FolderPermission) ( permission ?? (uint) Permission.FolderPermission.Read ) | Permission.FolderPermission.Read;
            var userGuid       = callContext.User.GUID.ToGuid();
            var groupGuids     = callContext.Groups.Select( group => group.GUID.ToGuid() ).ToList();

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

        private IEnumerable<FolderInfo> RetrieveFolderInfos(IEnumerable<IFolder> folders)
        {
            var folderIDs = folders.Select(folder => folder.ID).ToList();

            return (IEnumerable<FolderInfo>) McmRepository.GetFolderInfo(folderIDs);
        }

        [Datatype("Folder", "Delete")]
        public ScalarResult Delete(ICallContext callContext, uint id)
        {
            var userGuid   = callContext.User.GUID.ToGuid();
            var groupGuids = callContext.Groups.Select(group => group.GUID.ToGuid()).ToList();

            if(!PermissionManager.GetFolders(id).DoesUserOrGroupHavePermission(userGuid, groupGuids, Permission.FolderPermission.Delete))
                throw new InsufficientPermissionsException("User does not have permission to delete the folder");

            var result = McmRepository.DeleteFolder(id);

            if (result == -50)
                throw new FolderNotEmptyException("You cannot delete non empty folder");

            return new ScalarResult((int) result);
        }

		[Datatype("Folder", "Update")]
		public ScalarResult Update( ICallContext callContext, uint id, string newTitle, uint? newFolderTypeID, uint? newParentID )
		{
            if (!PermissionManager.GetFolders(id).DoesUserOrGroupHavePermission(callContext.User.GUID.ToGuid(), callContext.Groups.Select(item => item.GUID.ToGuid()), Permission.FolderPermission.Update ) )
				throw new InsufficientPermissionsException( "User does not have permission to give the requested permissions" );

			var result = McmRepository.UpdateFolder(id, newTitle, newFolderTypeID, newParentID);

			return new ScalarResult( (int) result );
		}

		[Datatype("Folder", "Create")]
        public IFolderInfo Create(ICallContext callContext, UUID subscriptionGUID, string title, uint? parentID, uint folderTypeID)
		{
            if( subscriptionGUID == null && !parentID.HasValue )
                throw new ArgumentException( "Both parentID and subscriptionGUID can't be null" );

		    var userGuid     = callContext.User.GUID.ToGuid();
		    var groupGuids   = callContext.Groups.Select(item => item.GUID.ToGuid());
            var subscription = callContext.Subscriptions.FirstOrDefault( sub => sub.GUID.ToString() == subscriptionGUID.ToString() );

		    if( subscription != null && subscription.Permission != SubscriptionPermission.CreateFolder )
		        throw new InsufficientPermissionsException( "User does not have permission to create topfolders with the subscriptionGUID" );
		    
            if(parentID.HasValue &&!PermissionManager.GetFolders((uint) parentID).DoesUserOrGroupHavePermission(userGuid, groupGuids, Permission.FolderPermission.Write))
                throw new InsufficientPermissionsException("User does not have permission to create subfolders");

            var result = McmRepository.CreateFolder(userGuid, subscription == null ? (Guid?) null : subscription.GUID.ToGuid(), title, parentID, folderTypeID);

		    if( result == -100 )
		        throw new InsufficientPermissionsException( "User does not have permission to Create the folder" );

            return McmRepository.GetFolderInfo(new[] { result }).First();
		}
    }
}
