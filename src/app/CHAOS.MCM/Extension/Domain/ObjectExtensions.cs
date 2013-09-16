namespace Chaos.Mcm.Extension.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Chaos.Mcm.Permission;
    using Chaos.Portal.Core.Data.Model;
    using Chaos.Portal.Core.Exceptions;
    using Chaos.Portal.Core.Indexing;
    using Chaos.Portal.Core.Indexing.View;

    public static class ObjectExtensions
    {
        public static IPagedResult<IResult> GetObjects(this IViewManager manager, IQuery query, Guid? accessPointGuid, IEnumerable<IFolder> folders , bool includeAccessPoints = false, bool includeMetadata = false, bool includeFiles = false, bool includeObjectRelations = false, bool includeFolders = false)
        {
            if (accessPointGuid == null)
            {
                var list = folders as List<IFolder> ?? folders.ToList();

                if (!list.Any()) throw new InsufficientPermissionsException("User does not have access to any folders");
                
                var folderQuery = string.Format("FolderAncestors:{0}", string.Join(" ", list.Select(item => item.ID)));

                query.Query = string.Format("({0})AND({1})", query.Query, folderQuery);
            }
            else
                query.Query = string.Format("({0})AND({1}_PubStart:[* TO NOW] AND {1}_PubEnd:[NOW TO *])", query.Query, accessPointGuid);

            // todo remove metadata schemas the user doesnt have permission to read
            return manager.GetView("Object").Query(query);
        } 
    }
}