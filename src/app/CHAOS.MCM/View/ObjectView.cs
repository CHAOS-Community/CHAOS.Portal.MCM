namespace Chaos.Mcm.View
{
    using System.Collections.Generic;
    using Permission;
    using Portal.Core.Data.Model;
    using Portal.Core.Indexing.View;
    using Object = Data.Dto.Object;

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
            return Query<ObjectViewData>(query);
        }

        #endregion
    }
}