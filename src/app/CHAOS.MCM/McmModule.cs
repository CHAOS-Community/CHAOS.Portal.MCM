namespace Chaos.Mcm
{
    using Configuration;
    using Data;
    using Extension.v5.Download;
    using Permission;
    using Permission.InMemory;
    using Permission.Specification;
    using Portal.Core;
    using Portal.Core.Exceptions;
    using View;

    public class McmModule : IMcmModule
    {
        #region Field

        private const string ConfigurationName = "MCM";

        #endregion
        #region Initialization

        public McmModule()
        {
            
        }

        public McmModule(IMcmRepository mcmRepository, IPermissionManager permissionManager)
        {
            McmRepository     = mcmRepository;
            PermissionManager = permissionManager;
        }

        #endregion
        #region Properties

        public IMcmRepository     McmRepository { get; private set; }
        public IPermissionManager PermissionManager { get; private set; }
        public IPortalApplication PortalApplication { get; private set; }

        public McmModuleConfiguration Configuration { get; set; }

        #endregion
        #region Implementation of IModule

        public virtual void Load(IPortalApplication portalApplication)
        {
            PortalApplication = portalApplication;

            Configuration = PortalApplication.GetSettings<McmModuleConfiguration>(ConfigurationName);

            McmRepository = new McmRepository().WithConfiguration(Configuration.ConnectionString);
            PermissionManager = new InMemoryPermissionManager().WithSynchronization(McmRepository, new IntervalSpecification(10000));

            try
            {
                if (!string.IsNullOrEmpty(Configuration.ObjectCoreName))
                {
                    var objectView = new ObjectView(PermissionManager);

                    portalApplication.AddView(objectView, Configuration.ObjectCoreName);

                    PortalApplication.MapRoute("/v5/Object", () => new Extension.v5.Object(PortalApplication, McmRepository, PermissionManager));
                    PortalApplication.MapRoute("/v6/Object", () => new Extension.v6.Object(PortalApplication, McmRepository, PermissionManager));
                }

                PortalApplication.MapRoute("/v5/Destination", () => new Extension.v6.Destination(PortalApplication, McmRepository, PermissionManager));
                PortalApplication.MapRoute("/v5/File", () => new Extension.v6.File(PortalApplication, McmRepository, PermissionManager));
                PortalApplication.MapRoute("/v5/Folder", () => new Extension.v6.Folder(PortalApplication, McmRepository, PermissionManager));
                PortalApplication.MapRoute("/v5/Format", () => new Extension.v6.Format(PortalApplication, McmRepository, PermissionManager));
                PortalApplication.MapRoute("/v5/Link", () => new Extension.v6.Link(PortalApplication, McmRepository, PermissionManager));
                PortalApplication.MapRoute("/v5/Metadata", () => new Extension.v6.Metadata(PortalApplication, McmRepository, PermissionManager));
                PortalApplication.MapRoute("/v5/MetadataSchema", () => new Extension.v6.MetadataSchema(PortalApplication, McmRepository, PermissionManager));
                PortalApplication.MapRoute("/v5/ObjectRelation", () => new Extension.v6.ObjectRelation(PortalApplication, McmRepository, PermissionManager));
                PortalApplication.MapRoute("/v5/ObjectType", () => new Extension.v6.ObjectType(PortalApplication, McmRepository, PermissionManager));
                PortalApplication.MapRoute("/v5/Mcm", () => new Extension.v6.Mcm(PortalApplication, McmRepository, PermissionManager));
                PortalApplication.MapRoute("/v5/Download", () => new Download(PortalApplication, McmRepository, Configuration));
                PortalApplication.MapRoute("/v5/UserProfile", () => new Extension.v6.UserProfile(PortalApplication, McmRepository, PermissionManager));
                PortalApplication.MapRoute("/v5/UserManagement", () => new Extension.v6.UserManagement(PortalApplication, McmRepository, PermissionManager, Configuration.UserManagement));

                PortalApplication.MapRoute("/v6/Destination", () => new Extension.v6.Destination(PortalApplication, McmRepository, PermissionManager));
                PortalApplication.MapRoute("/v6/File", () => new Extension.v6.File(PortalApplication, McmRepository, PermissionManager));
                PortalApplication.MapRoute("/v6/Folder", () => new Extension.v6.Folder(PortalApplication, McmRepository, PermissionManager));
                PortalApplication.MapRoute("/v6/Format", () => new Extension.v6.Format(PortalApplication, McmRepository, PermissionManager));
                PortalApplication.MapRoute("/v6/Link", () => new Extension.v6.Link(PortalApplication, McmRepository, PermissionManager));
                PortalApplication.MapRoute("/v6/Metadata", () => new Extension.v6.Metadata(PortalApplication, McmRepository, PermissionManager));
                PortalApplication.MapRoute("/v6/MetadataSchema", () => new Extension.v6.MetadataSchema(PortalApplication, McmRepository, PermissionManager));
                PortalApplication.MapRoute("/v6/ObjectRelation", () => new Extension.v6.ObjectRelation(PortalApplication, McmRepository, PermissionManager));
                PortalApplication.MapRoute("/v6/ObjectType", () => new Extension.v6.ObjectType(PortalApplication, McmRepository, PermissionManager));
                PortalApplication.MapRoute("/v6/Mcm", () => new Extension.v6.Mcm(PortalApplication, McmRepository, PermissionManager));
                PortalApplication.MapRoute("/v6/UserProfile", () => new Extension.v6.UserProfile(PortalApplication, McmRepository, PermissionManager));
                PortalApplication.MapRoute("/v6/UserManagement", () => new Extension.v6.UserManagement(PortalApplication, McmRepository, PermissionManager, Configuration.UserManagement));
            }
            catch (System.Exception)
            {
                // Another module already added these
            }
        }

        #endregion
    }
}
