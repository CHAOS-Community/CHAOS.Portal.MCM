using System;
using System.Linq;
using System.Collections.Generic;
using CHAOS;

using Chaos.Mcm.Permission;
using Chaos.Portal;
using Chaos.Portal.Data.Dto.Standard;
using Chaos.Portal.Exceptions;

namespace Chaos.Mcm.Extension
{
    using Chaos.Mcm.Data.Dto;

    using FolderPermission = Chaos.Mcm.Permission.FolderPermission;

    public class Object : AMcmExtension
    {
//		public IPagedResult<IResult> Get( ICallContext callContext, IQuery query, UUID accessPointGuid, bool? includeMetadata, bool? includeFiles, bool? includeObjectRelations, bool? includeAccessPoints )
//		{ 
//            // TODO: Implement AccessPointGUID for queries when user isnt logged in
//            using( var db = DefaultMCMEntities )
//			{
//			    if( query != null )
//			    {
//			        var metadataSchemas = new List<Chaos.Mcm.Data.EF.MetadataSchema>();
//
//                    if( accessPointGuid != null )
//                        query.Query = string.Format( "({0})+AND+(PubStart:[*+TO+NOW]+AND+PubEnd:[NOW+TO+*])", query.Query );
//                    else
//                    {
//						if( callContext.IsAnonymousUser )
//							throw new InsufficientPermissionsException("User must be logged in or use accessPointGuid" );
//
//                        var userGuid   = callContext.User.guid;
//                        var groupGuids = callContext.Groups.Select(group => group.guid).ToList();
//                        var folders    = PermissionManager.GetFolders(FolderPermission.Read, userGuid, groupGuids).ToList();
//
//						if( folders.Count == 0 )
//							throw new InsufficientPermissionsException("User does not have access to any folders" );
//
//                        query.Query = string.Format( "({0})+AND+({1})", query.Query, string.Join( "+OR+", folders.Select( folder => string.Format( "FolderTree:{0}", folder.ID ) ) ) );
//
//                        metadataSchemas = db.MetadataSchema_Get( callContext.User.guid.ToByteArray(), string.Join( ",", callContext.Groups.Select( group => group.guid.ToString().Replace("-","") ) ), null, 0x1 ).ToList();
//                    }
//
//					var indexResult = callContext.IndexManager.GetIndex<Object>().Get<UUIDResult>( query );
//					var resultPage  = indexResult.QueryResult.Results.Select(result => ((UUIDResult)result).guid);
//
//					// if solr doesnt return anything there is no need to continue, so just return an empty list
//					if( !resultPage.Any() )
//                        return new PagedResult<IResult>( indexResult.QueryResult.FoundCount, 0, new List<Data.Dto.Standard.Object>() );
//
//                    var objects      = db.Object_Get(resultPage, includeMetadata ?? false, includeFiles ?? false, includeObjectRelations ?? false, false, includeAccessPoints ?? false, metadataSchemas.ToDto() ).ToDto( callContext.GetSessionFromDatabase() == null ? null : callContext.Session.guid ).ToList();
//                    var sortedResult = ReArrange( objects, resultPage );
//
//					return new PagedResult<IResult>( indexResult.QueryResult.FoundCount, query.PageIndex, sortedResult );
//				}
//			}
//
//			throw new NotImplementedException("No implmentation for Object Get without solr parameters");
//		}

//        private static IEnumerable<Data.Dto.Standard.Object> ReArrange(IEnumerable<Data.Dto.Standard.Object> objects, IEnumerable<UUID> resultPage)
//        {
//            return resultPage.Select(uuid => objects.First(item => item.GUID.ToString() == uuid.ToString()));
//        }

		public Data.Dto.Object Create( ICallContext callContext, Guid? guid, uint objectTypeID, uint folderID )
		{
            var userGuid   = callContext.User.Guid;
            var groupGuids = callContext.Groups.Select(group => group.Guid);

            if (!PermissionManager.GetFolders(folderID).DoesUserOrGroupHavePermission(userGuid, groupGuids, FolderPermission.CreateUpdateObjects))
                throw new InsufficientPermissionsException("User does not have permissions to create object");

		    guid = guid.HasValue ? guid : Guid.NewGuid();

		    McmRepository.ObjectCreate(guid.Value, objectTypeID, folderID);

		    return McmRepository.ObjectGet(guid.Value);

            //    PutObjectInIndex( callContext.IndexManager.GetIndex<Object>(), newObject );
		}

        public ScalarResult SetPublishSettings(ICallContext callContext, Guid objectGuid, Guid accessPointGuid, DateTime? startDate, DateTime? endDate)
        {
            var userGuid   = callContext.User.Guid;
            var groupGuids = callContext.Groups.Select(item => item.Guid);

            if (McmRepository.GetAccessPoint(accessPointGuid, userGuid, groupGuids, (uint) AccessPointPermission.Write).FirstOrDefault() == null)
                throw new InsufficientPermissionsException( "User does not have permission to set publish settings for object in accessPoint" );

            var result = McmRepository.SetAccessPointPublishSettings(accessPointGuid, objectGuid, startDate, endDate);
                
        //    PutObjectInIndex( callContext.IndexManager.GetIndex<Object>(), McmRepository.GetObject(objectGuid, true, true, true, true, true) );

            return new ScalarResult( (int) result );
        }

        public ScalarResult Delete( ICallContext callContext, Guid guid )
        {
            var objToDel   = McmRepository.ObjectGet(guid, includeFolders: true);
            var userGuid   = callContext.User.Guid;
            var groupGuids = callContext.Groups.Select(group => group.Guid);
            var folders    = objToDel.ObjectFolders.Select(folder => PermissionManager.GetFolders(folder.ID));

            if (!PermissionManager.DoesUserOrGroupHavePermissionToFolders(userGuid, groupGuids, FolderPermission.DeleteObject, folders))
                throw new InsufficientPermissionsException("User does not have permissions to remove object");

            var result = McmRepository.ObjectDelete(guid);

            return new ScalarResult((int)result);
            //RemoveObjectFromIndex( callContext.IndexManager.GetIndex<Mcm>(), delObject );
        }

        public IEnumerable<Data.Dto.Object> Get(ICallContext callContext, IEnumerable<Guid> objectGuids, bool includeAccessPoints = false, bool includeMetadata = false, bool includeFiles = false, bool includeObjectRelations = false, bool includeFolders = false)
        {
            //todo: uncomment this
           // var objectsWithPermission = objectGuids.Where(item => HasPermissionToObject(callContext, item.ToUUID(), FolderPermission.Read));
            
            return McmRepository.ObjectGet(objectGuids, includeMetadata, includeFiles, includeObjectRelations, includeFolders, includeAccessPoints);
        }
    }
}
