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

        private IObjectCreator ObjectCreator { get; set; }

        #endregion
        #region Initialization

        public Object(IPortalApplication portalApplication, IMcmRepository mcmRepository, IPermissionManager permissionManager) : base(portalApplication, mcmRepository, permissionManager)
        {
            ObjectCreator = new ObjectCreator(mcmRepository, permissionManager, portalApplication.ViewManager);
        }

        #endregion

		public Data.Dto.Object Create(Guid? guid, uint objectTypeID, uint folderID )
		{
            var userId   = Request.User.Guid;
            var groupIds = Request.Groups.Select(group => group.Guid);

            return ObjectCreator.Create(guid, objectTypeID, folderID, userId, groupIds);
		}

        public ScalarResult SetPublishSettings(Guid objectGuid, Guid accessPointGuid, DateTime? startDate, DateTime? endDate)
        {
            var userGuid   = Request.User.Guid;
            var groupGuids = Request.Groups.Select(item => item.Guid);

            if (McmRepository.AccessPointGet(accessPointGuid, userGuid, groupGuids, (uint) AccessPointPermission.Write).FirstOrDefault() == null)
                throw new InsufficientPermissionsException( "User does not have permission to set publish settings for object in accessPoint" );

            var result = McmRepository.AccessPointPublishSettingsSet(accessPointGuid, objectGuid, startDate, endDate);

            if (result == 1)
            {
                var obj = McmRepository.ObjectGet(objectGuid, true, true, true, true, true);

                ViewManager.Index(obj);
            }

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

            if(result == 1) 
                ViewManager.Delete(string.Format("Id:{0}", guid));

            return new ScalarResult((int)result);
        }

        public IPagedResult<IResult> Get(IEnumerable<Guid> objectGuids, Guid? accessPointGuid, bool includeAccessPoints = false, bool includeMetadata = false, bool includeFiles = false, bool includeObjectRelations = false, bool includeFolders = false, uint pageSize = 10, uint pageIndex = 0)
        {
            var query = new SolrQuery{Query = "*:*"};

            query.PageIndex = pageIndex;
            query.PageSize  = pageSize;

            if (objectGuids.Any()) query.Query = string.Format("Id:{0}", string.Join(" ", objectGuids));

            return ViewManager.GetObjects( query,
                                           accessPointGuid,
                                           GetFoldersWithAccess(),
                                           includeAccessPoints,
                                           includeMetadata,
                                           includeFiles,
                                           includeObjectRelations,
                                           includeFolders);
        }
    }
}
