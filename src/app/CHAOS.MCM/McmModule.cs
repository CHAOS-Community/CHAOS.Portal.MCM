namespace Chaos.Mcm
{
    using System;
    using System.Linq;
    using System.Xml.Linq;

    using CHAOS.Serialization.Standard;
    using Configuration;
    using Data;
    using Data.Configuration;
    using Extension.Domain;
    using Extension.v5.Download;
    using Permission;
    using Permission.InMemory;
    using Permission.Specification;
    using Portal.Core;
    using Portal.Core.Data.Model;
    using Portal.Core.Exceptions;
    using Portal.Core.Indexing.View;
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

            LoadModuleConfiguration();

            McmRepository = new McmRepository().WithConfiguration(Configuration.ConnectionString);
            PermissionManager = new InMemoryPermissionManager().WithSynchronization(McmRepository, new IntervalSpecification(10000));

            var objectView = CreateObjectView();
            ObjectExtensions.ObjectViewName = objectView.Name;

            portalApplication.AddView(objectView, Configuration.ObjectCoreName);
            
            PortalApplication.MapRoute("/v5/Destination", () => new Extension.v6.Destination(PortalApplication, McmRepository, PermissionManager));
            PortalApplication.MapRoute("/v5/File", () => new Extension.v6.File(PortalApplication, McmRepository, PermissionManager));
            PortalApplication.MapRoute("/v5/Folder", () => new Extension.v6.Folder(PortalApplication, McmRepository, PermissionManager));
            PortalApplication.MapRoute("/v5/Format", () => new Extension.v6.Format(PortalApplication, McmRepository, PermissionManager));
            PortalApplication.MapRoute("/v5/Link", () => new Extension.v6.Link(PortalApplication, McmRepository, PermissionManager));
            PortalApplication.MapRoute("/v5/Metadata", () => new Extension.v6.Metadata(PortalApplication, McmRepository, PermissionManager));
            PortalApplication.MapRoute("/v5/MetadataSchema", () => new Extension.v6.MetadataSchema(PortalApplication, McmRepository, PermissionManager));
            PortalApplication.MapRoute("/v5/Object", () => new Extension.v5.Object(PortalApplication, McmRepository, PermissionManager));
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
            PortalApplication.MapRoute("/v6/Object", () => new Extension.v6.Object(PortalApplication, McmRepository, PermissionManager));
            PortalApplication.MapRoute("/v6/ObjectRelation", () => new Extension.v6.ObjectRelation(PortalApplication, McmRepository, PermissionManager));
            PortalApplication.MapRoute("/v6/ObjectType", () => new Extension.v6.ObjectType(PortalApplication, McmRepository, PermissionManager));
            PortalApplication.MapRoute("/v6/Mcm", () => new Extension.v6.Mcm(PortalApplication, McmRepository, PermissionManager));
            PortalApplication.MapRoute("/v6/UserProfile", () => new Extension.v6.UserProfile(PortalApplication, McmRepository, PermissionManager));
            PortalApplication.MapRoute("/v6/UserManagement", () => new Extension.v6.UserManagement(PortalApplication, McmRepository, PermissionManager, Configuration.UserManagement));
        }

        private void LoadModuleConfiguration()
        {
            try
            {
                var module = PortalApplication.PortalRepository.Module.Get(ConfigurationName);
                var configuration = XDocument.Parse(module.Configuration);

                Configuration = SerializerFactory.Get<XDocument>().Deserialize<McmModuleConfiguration>(configuration);

                if (string.IsNullOrEmpty(Configuration.ConnectionString) || string.IsNullOrEmpty(Configuration.ObjectCoreName))
                    throw new ModuleConfigurationMissingException("MCM configuration is invalid.");
            }
            catch (ArgumentException e)
            {
                var dummyConfig = new McmModuleConfiguration
                    {
                        Aws = new AwsConfiguration
                            {
                                AccessKey = "",
                                SecretKey = ""
                            },
                        UserManagement = new UserManagementConfiguration
                            {
                                UserFolderTypeId = 0,
                                UserObjectTypeId = 0,
                                UsersFolderName = ""
                            },
                        ConnectionString = "",
                        ObjectCoreName = ""
                    };

                var moduleTemplate = new Module
                    {
                        Name = ConfigurationName,
                        Configuration = SerializerFactory.XMLSerializer.Serialize(dummyConfig).ToString()
                    };

                PortalApplication.PortalRepository.Module.Set(moduleTemplate);

                throw new ModuleConfigurationMissingException("MCM configuration was missing, a template was created in the database", e);
            }
        }

        protected virtual IView CreateObjectView()
        {
            return new ObjectView(PermissionManager);
        }

        #endregion
    }
}
