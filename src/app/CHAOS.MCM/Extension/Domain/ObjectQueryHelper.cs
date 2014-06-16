namespace Chaos.Mcm.Extension.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Permission;
    using Portal.Core;
    using Portal.Core.Data.Model;
    using Portal.Core.Exceptions;
    using Portal.Core.Indexing;
    using View;

    public class ObjectQueryHelper
    {
        public IPortalApplication PortalApplication { get; set; }

        public ObjectQueryHelper(IPortalApplication portalApplication)
        {
            PortalApplication = portalApplication;
        }

        public IPagedResult<IResult> GetObjects(IQuery query, Guid? accessPointGuid, IEnumerable<IFolder> folderFilter,
                                                bool includeAccessPoints = false, bool includeMetadata = false,
                                                bool includeFiles = false, bool includeObjectRelations = false,
                                                bool includeFolders = false)
        {
            if (accessPointGuid == null)
            {
                var list = folderFilter as List<IFolder> ?? folderFilter.ToList();

                if (!list.Any()) throw new InsufficientPermissionsException("User does not have access to any folders");

                var folderQuery = String.Format("FolderAncestors:{0}", String.Join(" ", list.Select(item => item.ID)));

                // todo user Filter instead of query for permission
                query.Query = String.Format("({0})AND({1})", query.Query, folderQuery);
            }
            else
                query.Query = String.Format("({0})AND({1}_PubStart:[* TO NOW] AND {1}_PubEnd:[NOW TO *])", query.Query, accessPointGuid); // todo user Filter instead of query for permission

            PortalApplication.Log.Debug(query.ToString());

            // todo remove metadata schemas the user doesnt have permission to read
            var page = PortalApplication.ViewManager.GetView("Object").Query(query);

            FilterIncludes(page, includeAccessPoints, includeMetadata, includeFiles, includeObjectRelations, includeFolders);

            return page;
        }

        private static void FilterIncludes(IPagedResult<IResult> page, bool includeAccessPoints, bool includeMetadata, bool includeFiles, bool includeObjectRelations, bool includeFolders)
        {
            foreach (ObjectViewData obj in page.Results)
            {
                if (!includeMetadata) obj.Object.Metadatas = null;
                if (!includeAccessPoints) obj.Object.AccessPoints = null;
                if (!includeFiles) obj.Object.Files = null;
                if (!includeObjectRelations) obj.Object.ObjectRelationInfos = null;
                if (!includeFolders) obj.Object.ObjectFolders = null;
            }
        }
    }
}