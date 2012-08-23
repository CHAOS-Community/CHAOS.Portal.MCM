using System;
using System.Linq;
using System.Collections.Generic;
using CHAOS.Extensions;
using CHAOS.Index;
using CHAOS.MCM.Data.EF;
using CHAOS.MCM.Module.Rights;
using CHAOS.Portal.Core;
using CHAOS.Portal.Core.Module;
using CHAOS.Portal.DTO;
using CHAOS.Portal.DTO.Standard;
using CHAOS.Portal.Exception;

namespace CHAOS.MCM.Module
{
    [Module("MCM")]
    public class ObjectModule : AMCMModule
    {
        [Datatype("Object", "Get")]
		public IPagedResult<IResult> Get( ICallContext callContext, IQuery query, UUID accessPointGUID, bool? includeMetadata, bool? includeFiles, bool? includeObjectRelations, bool? includeAccessPoints )
		{ // TODO: Implement AccessPointGUID for queries when user isnt logged in
			using( var db = DefaultMCMEntities )
			{
			    if( query != null )
			    {
			        var metadataSchemas = new List<MetadataSchema>();

                    if( accessPointGUID != null )
                        query.Query = string.Format( "({0})+AND+(PubStart:[*+TO+NOW]+AND+PubEnd:[NOW+TO+*])", query.Query );
                    else
                    {
						if( callContext.IsAnonymousUser )
							throw new InsufficientPermissionsException("User must be logged in or use accessPointGUID" );

                        //TODO: Implement Folder Permissions Enum Flags (GET OBJECT FLAG)
                        var folders = PermissionManager.GetFolders( callContext.User.GUID.ToGuid(), callContext.Groups.Select(group => group.GUID.ToGuid() ), FolderPermissions.Read ).ToList();
  
						if( folders.Count == 0 )
							throw new InsufficientPermissionsException("User does not have access to any folders" );

                        query.Query = string.Format( "({0})+AND+({1})", query.Query, string.Join( "+OR+", folders.Select( folder => string.Format( "FolderTree:{0}", folder.ID ) ) ) );
                        
                        metadataSchemas = db.MetadataSchema_Get( callContext.User.GUID.ToByteArray(), string.Join( ",", callContext.Groups.Select( group => group.GUID.ToString().Replace("-","") ) ), null, 0x1 ).ToList();
                    }

					var indexResult = callContext.IndexManager.GetIndex<ObjectModule>().Get<UUIDResult>( query );
					var resultPage  = indexResult.QueryResult.Results.Select(result => ((UUIDResult)result).Guid);

					// if solr doesnt return anything there is no need to continue, so just return an empty list
					if( !resultPage.Any() )
                        return new PagedResult<IResult>( indexResult.QueryResult.FoundCount, 0, new List<Data.DTO.Object>() );

                    var objects = db.Object_Get(resultPage, includeMetadata ?? false, includeFiles ?? false, includeObjectRelations ?? false, false, includeAccessPoints ?? false, metadataSchemas.ToDTO() ).ToDTO( callContext.GetSessionFromDatabase() == null ? null : callContext.Session.GUID ).ToList();

					return new PagedResult<IResult>( indexResult.QueryResult.FoundCount, query.PageIndex, objects );
				}
			}

			throw new NotImplementedException("No implmentation for Object Get without solr parameters");
		}

		[Datatype("Object","Create")]
		public Data.DTO.Object Create( ICallContext callContext, UUID GUID, uint objectTypeID, uint folderID )
		{
		    using( var db = DefaultMCMEntities )
		    {
				if( !PermissionManager.GetFolder( folderID ).DoesUserOrGroupHavePersmission( callContext.User.GUID.ToGuid(), callContext.Groups.Select( item => item.GUID.ToGuid() ), FolderPermissions.CreateUpdateObjects ) )
					throw new InsufficientPermissionsException( "User does not have permissions to create object" );

				var guid = GUID ?? new UUID();

		        int result = db.Object_Create( guid.ToByteArray(), (int) objectTypeID, (int) folderID ).First().Value;

				if( result == -200 )
					throw new UnhandledException("Unhandled exception, Create was rolled back");

		        var newObject = db.Object_Get( guid, true, true, true, true, true ).ToDTO().ToList();

		        PutObjectInIndex( callContext.IndexManager.GetIndex<ObjectModule>(), newObject );

		        return newObject.First();
		    }
		}

        [Datatype("Object","SetPublishSettings")]
        public ScalarResult SetPublishSettings( ICallContext callContext, UUID objectGUID, UUID accessPointGUID, DateTime? startDate, DateTime? endDate )
        {
            using( var db = DefaultMCMEntities )
            {

                   // throw new InsufficientPermissionsException( "User does not have permission to set publish settings for object in accessPoint" );

                var result = db.AccessPoint_Object_Join_Set( accessPointGUID.ToByteArray(), objectGUID.ToByteArray(), startDate, endDate ).First();
                
                PutObjectInIndex( callContext.IndexManager.GetIndex<ObjectModule>(), new []{ db.Object_Get( objectGUID, true, true, true, true, true ).First().ToDTO() } );

                return new ScalarResult( result.Value );
            }
        }

        [Datatype("Object", "Delete")]
        public ScalarResult Delete( ICallContext callContext, UUID GUID )
        {
            using( MCMEntities db = DefaultMCMEntities )
            {
                var delObject = db.Object_Get( GUID, false, false, false, true, false ).ToDTO().First();

                if( !PermissionManager.DoesUserOrGroupHavePersmissionToFolders( delObject.Folders.Select( folder => folder.FolderID ), callContext.User.GUID.ToGuid(), callContext.Groups.Select( group => group.GUID.ToGuid() ), FolderPermissions.DeleteObject ) )
                    throw new InsufficientPermissionsException( "User does not have permissions to remove object" );

                var result = db.Object_Delete( GUID.ToByteArray() ).FirstOrDefault();

                if( !result.HasValue || result.Value == -200 )
                    throw new UnhandledException( "Object was not deleted, database rolled back" );

                RemoveObjectFromIndex( callContext.IndexManager.GetIndex<MCMModule>(), delObject );

                return new ScalarResult( result.Value );
            }
        }

		//[Datatype("Object", "PutInFolder")]
		//public ScalarResult Object_PutInFolder(CallContext callContext, Guid GUID, int folderID, int objectFolderTypeID)
		//{
		//    using( MCMEntities db = DefaultMCMEntities )
		//    {
		//        int result = db.Object_PutInFolder( callContext.Groups.Select( group => group.GUID ).ToList(), callContext.User.GUID, GUID, folderID, objectFolderTypeID );

		//        if( result == -100 )
		//            throw new InsufficientPermissionsException( "User does not have permissions to put object into folder" );

		//        PutObjectInIndex( callContext.IndexManager.GetIndex<MCMModule>(), db.Get( new []{ GUID }, true, false, true, true ) );

		//        return new ScalarResult( result );
		//    }
		//}
    }
}
