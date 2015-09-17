namespace Chaos.Mcm.Extension.v6
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using Domain;
    using Domain.Object;
    using Permission;
    using Portal.Core;
    using Portal.Core.Data.Model;
    using Portal.Core.Exceptions;
    using Portal.Core.Indexing.Solr.Request;

    public class Object : AMcmExtension
    {
        #region Properties

        private IObjectCreate ObjectCreate { get; set; }
        private IObjectDelete ObjectDelete { get; set; }
        private IObjectSetPublishSettings ObjectSetPublishSettings { get; set; }
        private ObjectQueryHelper ObjectQueryHelper { get; set; }

        #endregion
        #region Initialization

        public Object(IPortalApplication portalApplication, IMcmRepository mcmRepository, IPermissionManager permissionManager) : base(portalApplication, mcmRepository, permissionManager)
        {
            ObjectCreate = new ObjectCreate(mcmRepository, permissionManager, portalApplication.ViewManager);
            ObjectDelete = new ObjectDelete(mcmRepository, permissionManager, portalApplication.ViewManager);
            ObjectSetPublishSettings = new ObjectSetPublishSettings(mcmRepository, permissionManager, portalApplication.ViewManager);
            ObjectQueryHelper = new ObjectQueryHelper(portalApplication);
        }

        #endregion

		public Data.Dto.Object Create(Guid? guid, uint objectTypeID, uint folderID )
		{
            var userId   = Request.User.Guid;
            var groupIds = Request.Groups.Select(group => group.Guid);

            return ObjectCreate.Create(guid, objectTypeID, folderID, userId, groupIds);
		}

        public uint SetPublishSettings(Guid objectGuid, Guid accessPointGuid, DateTime? startDate, DateTime? endDate)
        {
            var userId   = Request.User.Guid;
            var groupIds = Request.Groups.Select(item => item.Guid);

            return ObjectSetPublishSettings.SetPublishSettings(objectGuid, accessPointGuid, startDate, endDate, userId, groupIds);
        }

        public uint Delete( Guid guid )
        {
            var userGuid   = Request.User.Guid;
            var groupGuids = Request.Groups.Select(group => group.Guid);

            return ObjectDelete.Delete(guid, userGuid, groupGuids);
        }

        public IPagedResult<IResult> Get(IEnumerable<Guid> objectGuids, uint? folderId = null, Guid? accessPointGuid = null, bool includeAccessPoints = false, bool includeMetadata = false, bool includeFiles = false, bool includeObjectRelations = false, bool includeFolders = false, uint pageSize = 10, uint pageIndex = 0)
        {
            var query = new SolrQuery
                {
                    Query = "*:*", 
                    PageIndex = pageIndex, 
                    PageSize = pageSize
                };

            if (objectGuids.Any()) query.Query = string.Format("Id:{0}", string.Join(" ", objectGuids));

            var folderFilter = GetFoldersWithAccess(folderId);

            if(accessPointGuid == null && !folderFilter.Any()) throw new InsufficientPermissionsException("No folders with access");

            return ObjectQueryHelper.GetObjects(query, accessPointGuid, folderFilter,
                                                                       includeAccessPoints, includeMetadata,
                                                                       includeFiles, includeObjectRelations,
                                                                       includeFolders);
        }
    }
}
