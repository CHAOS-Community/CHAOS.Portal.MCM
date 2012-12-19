using System;
using System.Linq;
using System.Collections.Generic;
using CHAOS;
using CHAOS.Extensions;
using CHAOS.Index;
using Chaos.Mcm.Data.EF;
using Chaos.Mcm.Permission;
using CHAOS.Portal.Exception;
using Chaos.Portal;
using Chaos.Portal.Data.Dto;
using Chaos.Portal.Data.Dto.Standard;
using Object = Chaos.Mcm.Data.Dto.Standard.Object;

namespace Chaos.Mcm.Extension
{
    public class Object : AMcmExtension
    {
		public IPagedResult<IResult> Get( ICallContext callContext, IQuery query, UUID accessPointGUID, bool? includeMetadata, bool? includeFiles, bool? includeObjectRelations, bool? includeAccessPoints )
		{ 
            // TODO: Implement AccessPointGUID for queries when user isnt logged in
            using( var db = DefaultMCMEntities )
			{
			    if( query != null )
			    {
			        var metadataSchemas = new List<Chaos.Mcm.Data.EF.MetadataSchema>();

                    if( accessPointGUID != null )
                        query.Query = string.Format( "({0})+AND+(PubStart:[*+TO+NOW]+AND+PubEnd:[NOW+TO+*])", query.Query );
                    else
                    {
						if( callContext.IsAnonymousUser )
							throw new InsufficientPermissionsException("User must be logged in or use accessPointGUID" );

                        var userGuid   = callContext.User.GUID.ToGuid();
                        var groupGuids = callContext.Groups.Select(group => group.GUID.ToGuid()).ToList();
                        var folders    = PermissionManager.GetFolders(FolderPermission.Read, userGuid, groupGuids).ToList();

						if( folders.Count == 0 )
							throw new InsufficientPermissionsException("User does not have access to any folders" );

                        query.Query = string.Format( "({0})+AND+({1})", query.Query, string.Join( "+OR+", folders.Select( folder => string.Format( "FolderTree:{0}", folder.ID ) ) ) );

                        metadataSchemas = db.MetadataSchema_Get( callContext.User.GUID.ToByteArray(), string.Join( ",", callContext.Groups.Select( group => group.GUID.ToString().Replace("-","") ) ), null, 0x1 ).ToList();
                    }

					var indexResult = callContext.IndexManager.GetIndex<Object>().Get<UUIDResult>( query );
					var resultPage  = indexResult.QueryResult.Results.Select(result => ((UUIDResult)result).Guid);

					// if solr doesnt return anything there is no need to continue, so just return an empty list
					if( !resultPage.Any() )
                        return new PagedResult<IResult>( indexResult.QueryResult.FoundCount, 0, new List<Data.Dto.Standard.Object>() );

                    var objects      = db.Object_Get(resultPage, includeMetadata ?? false, includeFiles ?? false, includeObjectRelations ?? false, false, includeAccessPoints ?? false, metadataSchemas.ToDto() ).ToDto( callContext.GetSessionFromDatabase() == null ? null : callContext.Session.GUID ).ToList();
                    var sortedResult = ReArrange( objects, resultPage );

					return new PagedResult<IResult>( indexResult.QueryResult.FoundCount, query.PageIndex, sortedResult );
				}
			}

			throw new NotImplementedException("No implmentation for Object Get without solr parameters");
		}

        private static IEnumerable<Data.Dto.Standard.Object> ReArrange(IEnumerable<Data.Dto.Standard.Object> objects, IEnumerable<UUID> resultPage)
        {
            return resultPage.Select(uuid => objects.First(item => item.GUID.ToString() == uuid.ToString()));
        }

		public Data.Dto.Standard.Object Create( ICallContext callContext, UUID GUID, uint objectTypeID, uint folderID )
		{
		    using( var db = DefaultMCMEntities )
		    {
				if( !PermissionManager.GetFolders( folderID ).DoesUserOrGroupHavePermission( callContext.User.GUID.ToGuid(), callContext.Groups.Select( item => item.GUID.ToGuid() ), FolderPermission.CreateUpdateObjects ) )
					throw new InsufficientPermissionsException( "User does not have permissions to create object" );

				var guid   = GUID ?? new UUID();
		        var result = db.Object_Create( guid.ToByteArray(), (int) objectTypeID, (int) folderID ).FirstOrDefault();

				if( result.HasValue && result.Value == -200 )
					throw new UnhandledException("Unhandled exception, Create was rolled back");

		        var newObject = db.Object_Get( guid, true, true, true, true, true ).ToDto().ToList();

		        PutObjectInIndex( callContext.IndexManager.GetIndex<Object>(), newObject );

		        return newObject.First();
		    }
		}

        public ScalarResult SetPublishSettings( ICallContext callContext, UUID objectGUID, UUID accessPointGUID, DateTime? startDate, DateTime? endDate )
        {
            var objectGuid      = objectGUID.ToGuid();
            var accessPointGuid = accessPointGUID.ToGuid();
            var userGuid        = callContext.User.GUID.ToGuid();
            var groupGuids      = callContext.Groups.Select(item => item.GUID.ToGuid());

            if (McmRepository.GetAccessPoint(accessPointGuid, userGuid, groupGuids, (uint) AccessPointPermission.Write).FirstOrDefault() == null)
                throw new InsufficientPermissionsException( "User does not have permission to set publish settings for object in accessPoint" );

            var result = McmRepository.SetAccessPointPublishSettings(accessPointGuid, objectGuid, startDate, endDate);
                
            PutObjectInIndex( callContext.IndexManager.GetIndex<Object>(), McmRepository.GetObject(objectGuid, true, true, true, true, true) );

            return new ScalarResult( (int) result );
        }

        public ScalarResult Delete( ICallContext callContext, UUID GUID )
        {
            using( var db = DefaultMCMEntities )
            {
                var delObject = db.Object_Get( GUID, false, false, false, true, false ).ToDto().First();

                if( !PermissionManager.DoesUserOrGroupHavePermissionToFolders( callContext.User.GUID.ToGuid(), callContext.Groups.Select( group => group.GUID.ToGuid() ), Chaos.Mcm.Permission.FolderPermission.DeleteObject,delObject.Folders.Select( folder => PermissionManager.GetFolders(folder.FolderID) ) ) )
                    throw new InsufficientPermissionsException( "User does not have permissions to remove object" );

                var result = db.Object_Delete( GUID.ToByteArray() ).FirstOrDefault();

                if( !result.HasValue || result.Value == -200 )
                    throw new UnhandledException( "Object was not deleted, database rolled back" );

                RemoveObjectFromIndex( callContext.IndexManager.GetIndex<Mcm>(), delObject );

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
