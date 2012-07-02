using System;
using System.Collections.Generic;
using System.Linq;
using CHAOS.Extensions;
using CHAOS.MCM.Data.DTO;
using CHAOS.MCM.Data.EF;
using CHAOS.MCM.Module.Rights;
using CHAOS.Portal.Core;
using CHAOS.Portal.Core.Module;
using CHAOS.Portal.DTO.Standard;
using CHAOS.Portal.Exception;
using Permission = CHAOS.MCM.Data.DTO.Permission;

namespace CHAOS.MCM.Module
{
    [Module("MCM")]
    public class FolderModule : AMCMModule
    {
        #region Permission

        [Datatype("Folder","GetPermission")]
        public FolderPermission GetPermission( ICallContext callContext, uint folderID )
        {
            var perm        = PermissionManager.GetFolder( folderID ).GetUserFolderPermission( callContext.User.GUID.ToGuid() ) | PermissionManager.GetFolder( folderID ).GetGroupFolderPermission( callContext.Groups.Select( group => group.GUID.ToGuid() ).ToList() );
            var permissions = new List<Permission>();

            for( int i = 1, shift = 1 << i; shift < (uint) FolderPermissions.All; i++, shift = 1 << i )
            {
                permissions.Add( new Permission( ( (FolderPermissions) shift).ToString(), (uint) shift ) );
            }

            return new FolderPermission( (uint) perm, permissions );
        }

        [Datatype("Folder","SetPermission")]
        public ScalarResult SetPermission( ICallContext callContext, UUID userGUID, UUID groupGUID, uint folderID, uint permission )
        {
            // TODO: Add permissions check before setting the new permissions
            int result = 0;
            
            if( !PermissionManager.DoesUserOrGroupHavePersmissionToFolders( new[] {folderID}, callContext.User.GUID.ToGuid(), callContext.Groups.Select( item => item.GUID.ToGuid() ), (FolderPermissions) permission ) )
                throw new InsufficientPermissionsException( "User does not have permission to give the requested permissions" );

            using( var db = DefaultMCMEntities )
            {
                if( userGUID != null )
                    result += db.Folder_User_Join_Set( userGUID.ToByteArray(), (int?) folderID, (int?) permission ).First().Value;    
                if( groupGUID != null )
                    result += db.Folder_Group_Join_Set( groupGUID.ToByteArray(), (int?) folderID, (int?) permission ).First().Value;
            }

            return new ScalarResult( result );
        }

        #endregion
        
        [Datatype("Folder", "Get")]
		public IEnumerable<Data.DTO.FolderInfo> Get( ICallContext callContext, uint? id, uint? folderTypeID, uint? parentID, uint? permission )
		{
			var folderIDs      = new List<long>();
			var permissionEnum = (FolderPermissions) ( permission ?? (uint) FolderPermissions.Read );

			permissionEnum = permissionEnum | FolderPermissions.Read;

			if( !parentID.HasValue && !id.HasValue )
				folderIDs = PermissionManager.GetFolders( callContext.User.GUID.ToGuid(), callContext.Groups.Select( group => group.GUID.ToGuid() ).ToList(), permissionEnum ).Select( folder => (long) folder.ID ).ToList();
            else
			if( parentID.HasValue )
				folderIDs = PermissionManager.GetFolders( callContext.User.GUID.ToGuid(), callContext.Groups.Select( group => group.GUID.ToGuid() ).ToList(), permissionEnum, parentID.Value).Select(folder => (long) folder.ID).ToList();
            else
			if( id.HasValue )
			{
				var folder = PermissionManager.GetFolder( id.Value );

				if( folder.DoesUserOrGroupHavePersmission( callContext.User.GUID.ToGuid(), callContext.Groups.Select( group => group.GUID.ToGuid() ).ToList(), permissionEnum, true ) )
					folderIDs.Add( folder.ID );
			}

			using( var db = DefaultMCMEntities )
			{
				return db.FolderInfo.Where( fi => folderIDs.Contains<long>( fi.ID ) ).ToDTO().ToList();
			}
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

		//[Datatype("Folder","Update")]
		//public ScalarResult Folder_Update( CallContext callContext, int id, string newTitle, int? newFolderTypeID )
		//{
		//    using( MCMEntities db = DefaultMCMEntities )
		//    {
		//        int result = db.Folder_Update( callContext.Groups.Select(group => group.GUID).ToList(), callContext.User.GUID, id, newTitle, null, newFolderTypeID );

		//        if( result == -10 )
		//            throw new InvalidProtocolException( "The parameters to update cant all be null" );

		//        if( result == -100 )
		//            throw new InsufficientPermissionsException( "User does not have permission to update the folder" );

		//        return new ScalarResult( result );
		//    }
		//}

		[Datatype("Folder", "Create")]
		public Data.DTO.FolderInfo Create( ICallContext callContext, UUID subscriptionGUID, string title, uint? parentID, uint folderTypeID )
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
