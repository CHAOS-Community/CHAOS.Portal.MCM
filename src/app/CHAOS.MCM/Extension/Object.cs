namespace Chaos.Mcm.Extension
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using Chaos.Mcm.Data;
    using Chaos.Mcm.Permission;
    using Chaos.Portal.Core;
    using Chaos.Portal.Core.Data.Model;
    using Chaos.Portal.Core.Exceptions;
    using Chaos.Portal.Core.Indexing.Solr.Request;

    using FolderPermission = Chaos.Mcm.Permission.FolderPermission;

    public class Object : AMcmExtension
    {
        #region Initialization

        public Object(IPortalApplication portalApplication, IMcmRepository mcmRepository, IPermissionManager permissionManager) : base(portalApplication, mcmRepository, permissionManager)
        {
        }

        public Object(IPortalApplication portalApplication)
            : base(portalApplication)
        {
        }

        #endregion

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
//                        query.Query = string.Format("({0})+AND+({1}_PubStart:[*+TO+NOW]+AND+{1}_PubEnd:[NOW+TO+*])", query.Query, accessPointGUID);            
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

		public Data.Dto.Object Create(Guid? guid, uint objectTypeID, uint folderID )
		{
            var userGuid   = Request.User.Guid;
            var groupGuids = Request.Groups.Select(group => group.Guid);

            if (!PermissionManager.GetFolders(folderID).DoesUserOrGroupHavePermission(userGuid, groupGuids, FolderPermission.CreateUpdateObjects)) 
                throw new InsufficientPermissionsException("User does not have permissions to create object");

		    guid = guid.HasValue ? guid : Guid.NewGuid();

		    McmRepository.ObjectCreate(guid.Value, objectTypeID, folderID);

		    var result = McmRepository.ObjectGet(guid.Value);
            
            ViewManager.Index(result);

		    return result;

            //    PutObjectInIndex( callContext.IndexManager.GetIndex<Object>(), newObject );
		}

        public ScalarResult SetPublishSettings(Guid objectGuid, Guid accessPointGuid, DateTime? startDate, DateTime? endDate)
        {
            var userGuid   = Request.User.Guid;
            var groupGuids = Request.Groups.Select(item => item.Guid);

            if (McmRepository.AccessPointGet(accessPointGuid, userGuid, groupGuids, (uint) AccessPointPermission.Write).FirstOrDefault() == null)
                throw new InsufficientPermissionsException( "User does not have permission to set publish settings for object in accessPoint" );

            var result = McmRepository.AccessPointPublishSettingsSet(accessPointGuid, objectGuid, startDate, endDate);
                
        //    PutObjectInIndex( callContext.IndexManager.GetIndex<Object>(), McmRepository.GetObject(objectGuid, true, true, true, true, true) );

            return new ScalarResult( (int) result );
        }

        public ScalarResult Delete( Guid guid )
        {
            var objToDel   = McmRepository.ObjectGet(guid, includeFolders: true);
            var userGuid   = Request.User.Guid;
            var groupGuids = Request.Groups.Select(group => group.Guid);
            var folders    = objToDel.ObjectFolders.Select(folder => PermissionManager.GetFolders(folder.ID));

            if (!PermissionManager.DoesUserOrGroupHavePermissionToFolders(userGuid, groupGuids, FolderPermission.DeleteObject, folders))
                throw new InsufficientPermissionsException("User does not have permissions to remove object");

            var result = McmRepository.ObjectDelete(guid);

            return new ScalarResult((int)result);
            //RemoveObjectFromIndex( callContext.IndexManager.GetIndex<Mcm>(), delObject );
        }

        public IPagedResult<IResult> Get(IEnumerable<Guid> objectGuids, Guid? accessPointGuid, bool includeAccessPoints = false, bool includeMetadata = false, bool includeFiles = false, bool includeObjectRelations = false, bool includeFolders = false, uint pageSize = 10, uint pageIndex = 0)
        {
            var query = new SolrQuery{Query = "*:*"};

            query.PageIndex = pageIndex;
            query.PageSize  = pageSize;

            if (objectGuids.Any()) query.Query = string.Format("Id:{0}", string.Join(" ", objectGuids));

            if (accessPointGuid == null)
            {
                var userGuid    = Request.User.Guid;
                var groupGuids  = Request.Groups.Select(group => group.Guid).ToList();
                var folders     = PermissionManager.GetFolders(FolderPermission.Read, userGuid, groupGuids).ToList();
                var folderQuery = string.Format("FolderAncestors:{0}", string.Join(" ", folders.Select(item => item.ID)));

                if(!folders.Any()) throw new InsufficientPermissionsException("User does not have access to any folders");

                query.Query = string.Format( "({0})AND({1})", query.Query, folderQuery );
                
                // todo remove metadata schemas the user doesnt have permission to read

                return ViewManager.GetView("Object").Query(query);
            }
            else
            {
                query.Query = string.Format("({0})AND({1}_PubStart:[*+TO+NOW]+AND+{1}_PubEnd:[NOW+TO+*])", query.Query, accessPointGuid);

                return ViewManager.GetView("Object").Query(query);
            }
        }
    }
}
