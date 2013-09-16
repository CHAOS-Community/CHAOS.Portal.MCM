namespace Chaos.Mcm.View
{
    using System.Collections.Generic;
    using System.Linq;
    using Chaos.Mcm.Permission;
    using Chaos.Portal.Core.Data.Model;
    using Chaos.Portal.Core.Indexing.View;

    using IFolder = Chaos.Mcm.Permission.IFolder;
    using Object = Chaos.Mcm.Data.Dto.Object;

    public class ObjectView : AView
    {
        #region Initialization

        public ObjectView(IPermissionManager permissionManager) : base("Object")
        {
            PermissionManager = permissionManager;
        }

        #endregion

        #region Properties

        public IPermissionManager PermissionManager { get; set; }

        #endregion

        #region Overrides of AView

        public override IList<IViewData> Index(object objectsToIndex)
        {
            var obj = objectsToIndex as Object;

            if (obj == null) return new List<IViewData>();

            return new[] { new ObjectViewData(obj, PermissionManager) };
        }

        public override IPagedResult<IResult> Query(Portal.Core.Indexing.IQuery query)
        {
            var result = Core.Query(query);

            var foundCount = result.QueryResult.FoundCount;
            var startIndex = result.QueryResult.StartIndex;
            var keys       = result.QueryResult.Results.Select(item => CreateKey(item.Id));
            var results    = Cache.Get<ObjectViewData>(keys);

            return new PagedResult<IResult>(foundCount, startIndex, results);
        }

        #endregion
    }
}