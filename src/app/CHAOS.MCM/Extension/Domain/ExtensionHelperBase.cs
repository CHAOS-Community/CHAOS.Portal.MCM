namespace Chaos.Mcm.Extension.Domain
{
    using Data;
    using Permission;
    using Portal.Core.Indexing.View;

    public abstract class ExtensionHelperBase
    {
        protected IMcmRepository McmRepository { get; set; }
        protected IPermissionManager PermissionManager { get; set; }
        protected IViewManager ViewManager { get; set; }

        protected ExtensionHelperBase(IMcmRepository mcmRepository, IPermissionManager permissionManager, IViewManager viewManager)
        {
            McmRepository = mcmRepository;
            PermissionManager = permissionManager;
            ViewManager = viewManager;
        }
    }
}