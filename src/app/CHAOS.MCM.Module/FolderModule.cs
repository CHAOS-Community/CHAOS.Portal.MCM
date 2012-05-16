using System.Collections.Generic;
using System.Linq;
using CHAOS.Extensions;
using CHAOS.MCM.Data.DTO;
using CHAOS.MCM.Data.EF;
using CHAOS.MCM.Module.Rights;
using CHAOS.Portal.Core;
using CHAOS.Portal.Core.Module;
using CHAOS.Portal.DTO.Standard;
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
            var perm = PermissionManager.GetFolder( folderID ).GetUserFolderPermission( callContext.User.GUID.ToGuid() ) | PermissionManager.GetFolder( folderID ).GetGroupFolderPermission( callContext.Groups.Select( group => group.GUID.ToGuid() ).ToList() );

            IList<Permission> permissions = new List<Permission>();

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

            using( var db = DefaultMCMEntities )
            {
                if( !userGUID.IsNull() )
                    result += db.Folder_User_Join_Set( userGUID.ToByteArray(), (int?) folderID, (int?) permission ).First().Value;    
                if( !groupGUID.IsNull() )
                    result += db.Folder_Group_Join_Set( groupGUID.ToByteArray(), (int?) folderID, (int?) permission ).First().Value;
            }

            return new ScalarResult( result );
        }

        #endregion
        
        [Datatype("Folder", "Get")]
		public IEnumerable<Data.DTO.FolderInfo> Get( ICallContext callContext, uint? id, uint? folderTypeID, uint? parentID )
		{
			var folderIDs = new List<long>();

			if( !parentID.HasValue && !id.HasValue )
				folderIDs = PermissionManager.GetFolders( callContext.User.GUID.ToGuid(), callContext.Groups.Select( group => group.GUID.ToGuid() ).ToList() ).Select( folder => (long) folder.ID ).ToList();
            else
			if( parentID.HasValue )
				folderIDs = PermissionManager.GetFolders( callContext.User.GUID.ToGuid(), callContext.Groups.Select( group => group.GUID.ToGuid() ).ToList(), parentID.Value).Select(folder => (long) folder.ID).ToList();
            else
			if( id.HasValue )
			{
				var folder = PermissionManager.GetFolder( id.Value );

				if( folder.DoesUserOrGroupHavePersmission( callContext.User.GUID.ToGuid(), callContext.Groups.Select( group => group.GUID.ToGuid() ).ToList(), FolderPermissions.Read, true ) )
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

		//[Datatype("Folder", "Create")]
		//public FolderInfo Folder_Create( CallContext callContext, string subscriptionGUID, string title, int? parentID, int folderTypeID )
		//{
		//    using( MCMEntities db = DefaultMCMEntities )
		//    {
		//        Guid? subGUID       = null;
		//        int?  subPermission = 0;

		//        if( !string.IsNullOrEmpty( subscriptionGUID ) )
		//        {
		//            SubscriptionInfo subscription = callContext.Subscriptions.FirstOrDefault( sub => sub.GUID.CompareTo( Guid.Parse( subscriptionGUID ) ) == 0 );

		//            subGUID       = subscription.GUID;
		//            subPermission = subscription.Permission;
		//        }

		//        int result = db.Folder_Create( callContext.Groups.Select( group => group.GUID ).ToList(), 
		//                                       callContext.User.GUID,
		//                                       subGUID,
		//                                       subPermission,
		//                                       title, 
		//                                       parentID, 
		//                                       folderTypeID);

		//        if( result == -100 )
		//            throw new Portal.Core.Exception.InsufficientPermissionsException( "User does not have permission to Create the folder" );

		//        return db.Get( callContext.Groups.Select(group => group.GUID).ToList(),
		//                              callContext.User.GUID, 
		//                              result, 
		//                              null, 
		//                              null ).First();

		//    //    FolderInfo folder = Get( callContext, result, null, null ).First();

		//     //   return folder;
		//    }
		//}
    }
}
