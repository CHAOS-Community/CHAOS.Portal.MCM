namespace Chaos.Mcm.Extension.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Permission;
    using Portal.Core.Data.Model;
    using Portal.Core.Exceptions;
    using Portal.Core.Indexing;
    using Portal.Core.Indexing.View;

    public static class ObjectExtensions
    {
        // TODO need to redesign, need to find a better way for this class to get access to the current configuration
        private static string _ObjectViewName = DefaultObjectViewName();

        public static string ObjectViewName
        {
            get
            {
                return _ObjectViewName;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = DefaultObjectViewName();

                _ObjectViewName = value;
            }
        }

        private static string DefaultObjectViewName()
        {
            return "Object";
        }

        public static IPagedResult<IResult> GetObjects(this IViewManager manager, IQuery query, Guid? accessPointGuid, IEnumerable<IFolder> folders , bool includeAccessPoints = false, bool includeMetadata = false, bool includeFiles = false, bool includeObjectRelations = false, bool includeFolders = false)
        {
            if (accessPointGuid == null)
            {
                var list = folders as List<IFolder> ?? folders.ToList();

                if (!list.Any()) throw new InsufficientPermissionsException("User does not have access to any folders");

                var folderQuery = string.Format("FolderAncestors:{0}", string.Join(" ", list.Select(item => item.ID)));

                // todo user Filter instead of query for permission
                query.Query = string.Format("({0})AND({1})", query.Query, folderQuery);
            }
            else
                query.Query = string.Format("({0})AND({1}_PubStart:[* TO NOW] AND {1}_PubEnd:[NOW TO *])", query.Query, accessPointGuid); // todo user Filter instead of query for permission

            // todo remove metadata schemas the user doesnt have permission to read
            return manager.GetView(ObjectViewName).Query(query);
        } 
    }
}