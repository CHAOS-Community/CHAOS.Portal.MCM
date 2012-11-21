using System;
using System.Linq;
using System.Collections.Generic;
using CHAOS.Extensions;
using CHAOS.Index;
using CHAOS.MCM.Data.EF;
using CHAOS.MCM.Permission;
using CHAOS.Portal.Core;
using CHAOS.Portal.Core.Module;
using CHAOS.Portal.DTO;
using CHAOS.Portal.DTO.Standard;
using CHAOS.Portal.Exception;
using FolderPermission = CHAOS.MCM.Permission.FolderPermission;
using MetadataSchema = CHAOS.MCM.Data.EF.MetadataSchema;
using Object = CHAOS.MCM.Data.Dto.Standard.Object;

namespace CHAOS.MCM.Module
{
    [Module("MCM")]
    public class ObjectModule : AMCMModule
    {
        [Datatype("Object", "Get")]
		public IPagedResult<IResult> Get( ICallContext callContext, IQuery query, UUID accessPointGUID, bool? includeMetadata, bool? includeFiles, bool? includeObjectRelations, bool? includeAccessPoints )
		{ // TODO: Implement AccessPointGUID for queries when user isnt logged in
			callContext.Log.Debug("using db");
            using( var db = DefaultMCMEntities )
			{
			    if( query != null )
			    {
			        var metadataSchemas = new List<MetadataSchema>();

                    if( accessPointGUID != null )
                        query.Query = string.Format( "({0})+AND+(PubStart:[*+TO+NOW]+AND+PubEnd:[NOW+TO+*])", query.Query );
                    else
                    {
                        callContext.Log.Debug("is anonymous user");
						if( callContext.IsAnonymousUser )
							throw new InsufficientPermissionsException("User must be logged in or use accessPointGUID" );

                        var userGuid   = callContext.User.GUID.ToGuid();
                        var groupGuids = callContext.Groups.Select(group => group.GUID.ToGuid()).ToList();

                        callContext.Log.Debug("get folders");
                        var folders = PermissionManager.GetFolders(FolderPermission.Read, userGuid, groupGuids).ToList();

						if( folders.Count == 0 )
							throw new InsufficientPermissionsException("User does not have access to any folders" );

                        callContext.Log.Debug("generate folders in query");
                        query.Query = string.Format( "({0})+AND+({1})", query.Query, string.Join( "+OR+", folders.Select( folder => string.Format( "FolderTree:{0}", folder.ID ) ) ) );

                        callContext.Log.Debug("get metadata schemas");
                        metadataSchemas = db.MetadataSchema_Get( callContext.User.GUID.ToByteArray(), string.Join( ",", callContext.Groups.Select( group => group.GUID.ToString().Replace("-","") ) ), null, 0x1 ).ToList();
                    }

                    callContext.Log.Debug("query solr");
					var indexResult = callContext.IndexManager.GetIndex<ObjectModule>().Get<UUIDResult>( query );
                    callContext.Log.Debug("convert to guid list");
					var resultPage  = indexResult.QueryResult.Results.Select(result => ((UUIDResult)result).Guid);

					// if solr doesnt return anything there is no need to continue, so just return an empty list
                    callContext.Log.Debug("check any results");
					if( !resultPage.Any() )
                        return new PagedResult<IResult>( indexResult.QueryResult.FoundCount, 0, new List<Object>() );

                    callContext.Log.Debug("get from database");
                    var objects = db.Object_Get(resultPage, includeMetadata ?? false, includeFiles ?? false, includeObjectRelations ?? false, false, includeAccessPoints ?? false, metadataSchemas.ToDTO() ).ToDTO( callContext.GetSessionFromDatabase() == null ? null : callContext.Session.GUID ).ToList();

                    callContext.Log.Debug("sort result");
                    var sortedResult = ReArrange( objects, resultPage );
                    callContext.Log.Debug("return object get");
					return new PagedResult<IResult>( indexResult.QueryResult.FoundCount, query.PageIndex, sortedResult );
				}
			}

			throw new NotImplementedException("No implmentation for Object Get without solr parameters");
		}

        private static IEnumerable<Object> ReArrange(IEnumerable<Object> objects, IEnumerable<UUID> resultPage)
        {
            return resultPage.Select(uuid => objects.First(item => item.GUID.ToString() == uuid.ToString()));
        }

        [Datatype("Object","Create")]
		public Object Create( ICallContext callContext, UUID GUID, uint objectTypeID, uint folderID )
		{
		    using( var db = DefaultMCMEntities )
		    {
				if( !PermissionManager.GetFolders( folderID ).DoesUserOrGroupHavePermission( callContext.User.GUID.ToGuid(), callContext.Groups.Select( item => item.GUID.ToGuid() ), FolderPermission.CreateUpdateObjects ) )
					throw new InsufficientPermissionsException( "User does not have permissions to create object" );

				var guid   = GUID ?? new UUID();
		        var result = db.Object_Create( guid.ToByteArray(), (int) objectTypeID, (int) folderID ).FirstOrDefault();

				if( result.HasValue && result.Value == -200 )
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
                var objectGuid      = objectGUID.ToGuid();
                var accessPointGuid = accessPointGUID.ToGuid();
                var userGuid        = callContext.User.GUID.ToGuid();
                var groupGuids      = callContext.Groups.Select(item => item.GUID.ToGuid());

                if (McmRepository.GetAccessPoint(accessPointGuid, userGuid, groupGuids, (uint) AccessPointPermission.Write).FirstOrDefault() == null)
                   throw new InsufficientPermissionsException( "User does not have permission to set publish settings for object in accessPoint" );

                var result = McmRepository.SetAccessPointPublishSettings(accessPointGuid, objectGuid, startDate, endDate);
                
                PutObjectInIndex( callContext.IndexManager.GetIndex<ObjectModule>(), McmRepository.GetObject(objectGuid, true, true, true, true, true) );

                return new ScalarResult( (int) result );
            }
        }

        [Datatype("Object", "Delete")]
        public ScalarResult Delete( ICallContext callContext, UUID GUID )
        {
            using( var db = DefaultMCMEntities )
            {
                var delObject = db.Object_Get( GUID, false, false, false, true, false ).ToDTO().First();

                if( !PermissionManager.DoesUserOrGroupHavePermissionToFolders( callContext.User.GUID.ToGuid(), callContext.Groups.Select( group => group.GUID.ToGuid() ), Permission.FolderPermission.DeleteObject,delObject.Folders.Select( folder => PermissionManager.GetFolders(folder.FolderID) ) ) )
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
