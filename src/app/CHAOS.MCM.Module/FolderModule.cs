using System.Collections.Generic;
using System.Linq;
using CHAOS.Extensions;
using CHAOS.MCM.Data.DTO;
using CHAOS.MCM.Data.EF;
using CHAOS.MCM.Module.Rights;
using CHAOS.Portal.Core;
using CHAOS.Portal.Core.Module;

namespace CHAOS.MCM.Module
{
    [Module("MCM")]
    public class FolderModule : AMCMModule
    {
        #region Permission

        [Datatype("Folder","GetPermissions")]
        public FolderPermission GetPermissions( ICallContext callContext, uint folderID )
        {
            return new FolderPermission{ AccumulatedPermission = (uint) PermissionManager.GetFolder( folderID ).GetUserFolderPermission( callContext.User.GUID.ToGuid() ) | (uint) PermissionManager.GetFolder( folderID ).GetGroupFolderPermission( callContext.Groups.Select( group => group.GUID.ToGuid() ).ToList() ) };
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
