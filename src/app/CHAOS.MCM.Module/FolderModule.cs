using System;
using System.Collections.Generic;
using System.Linq;
using CHAOS.Extensions;
using CHAOS.MCM.Data.Dto.Standard;
using CHAOS.MCM.Data.EF;
using CHAOS.Portal.Core;
using CHAOS.Portal.Core.Module;
using CHAOS.Portal.DTO.Standard;
using CHAOS.Portal.Exception;
using FolderInfo       = CHAOS.MCM.Data.Dto.Standard.FolderInfo;
using FolderPermission = CHAOS.MCM.Data.Dto.Standard.FolderPermission;
using IFolder = CHAOS.MCM.Permission.IFolder;

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
			var permissionEnum = (Permission.FolderPermission) ( permission ?? (uint) Permission.FolderPermission.Read ) | Permission.FolderPermission.Read;
            var userGuid       = callContext.User.GUID.ToGuid();
            var groupGuids     = callContext.Groups.Select( group => group.GUID.ToGuid() ).ToList();

			if( !parentID.HasValue && !id.HasValue )
                return RetrieveFolderInfos( PermissionManager.GetFolders(permissionEnum, userGuid, groupGuids) );
            if( parentID.HasValue && !id.HasValue )
                return RetrieveFolderInfos( PermissionManager.GetFolders(permissionEnum, userGuid, groupGuids).Where(f => f.ParentFolder != null && f.ParentFolder.ID == parentID.Value) );
            if( !parentID.HasValue )
            {
                var folder = PermissionManager.GetFolders( id.Value );

                if( folder.DoesUserOrGroupHavePermission( userGuid, groupGuids, permissionEnum ) )
                    return RetrieveFolderInfos(new[] {folder});
            }
            
            throw new ArgumentException("It does not make sense to specficy both ID and ParentID in the same query");
		}

        private IEnumerable<FolderInfo> RetrieveFolderInfos(IEnumerable<IFolder> folders)
        {
            var folderIDs = folders.Select(folder => folder.ID).ToList();

            return (IEnumerable<FolderInfo>) McmRepository.GetFolderInfo(folderIDs);
        }

        //[Datatype("Folder","Delete")]
		//public ScalarResult Folder_Delete( CallContext callContext, int id )
		//{
		//    using( MCMEntities db = DefaultMCMEntities )
		//    {
		//        int result = db.Folder_Delete( callContext.Groups.Select(group => group.GUID).ToList(), callContext.User.GUID, id );

		//        if( result == -50 )
		//            throw new FolderNotEmptyException( "You cannot delete non empty folder" );
 
		//        if( result == -100 )
		//            throw new Portal.Core.Exception.InsufficientPermissionsException( "User does not have permission to delete the folder" );

		//        return new ScalarResult( result );
		//    }
		//}

		[Datatype("Folder", "Update")]
		public ScalarResult Update( ICallContext callContext, uint id, string newTitle, uint? newFolderTypeID )
		{
			using( var db = DefaultMCMEntities )
			{
                if (!PermissionManager.GetFolders(id).DoesUserOrGroupHavePermission(callContext.User.GUID.ToGuid(), callContext.Groups.Select(item => item.GUID.ToGuid()), Permission.FolderPermission.Update ) )
					throw new InsufficientPermissionsException( "User does not have permission to give the requested permissions" );

				var result = db.Folder_Update( (int?) id, newTitle, null, (int?) newFolderTypeID ).FirstOrDefault();

				if( !result.HasValue )
					throw new UnhandledException( "Procedure finished without a value" );

				return new ScalarResult( result.Value );
			}
		}

		[Datatype("Folder", "Create")]
        public FolderInfo Create(ICallContext callContext, UUID subscriptionGUID, string title, uint? parentID, uint folderTypeID)
		{
            if( subscriptionGUID == null && !parentID.HasValue )
                throw new ArgumentException( "Both parentID and subscriptionGUID can't be null" );

		    using( var db = DefaultMCMEntities )
		    {
		        if( subscriptionGUID != null )
		        {
                    throw new NotImplementedException( "Creating top folders has not been implemented" );
		            var subscription = callContext.Subscriptions.FirstOrDefault( sub => sub.GUID.ToString() == sub.ToString() );

                    // TODO: Check actual subscription permissions

		            if( subscription == null )
		                throw new InsufficientPermissionsException( "User does not have permission to create topfolders with the subscriptionGUID" );
		        }

		        var result = db.Folder_Create( callContext.User.GUID.ToByteArray(),
                                               subscriptionGUID == null ? null : subscriptionGUID.ToByteArray(),
		                                       title, 
		                                       (int?) parentID, 
		                                       (int?) folderTypeID ).First();

		        if( result == -100 )
		            throw new InsufficientPermissionsException( "User does not have permission to Create the folder" );

                return db.FolderInfo.First( fi => result == fi.ID ).ToDTO();
		    }
		}
    }
}
