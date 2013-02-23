namespace Chaos.Mcm
{
    using System.Xml.Linq;

    using Chaos.Mcm.Data;
    using Chaos.Mcm.Extension;
    using Chaos.Mcm.Permission;
    using Chaos.Mcm.Permission.InMemory;
    using Chaos.Mcm.Permission.Specification;
    using Chaos.Portal;
    using Chaos.Portal.Extension;
    using Chaos.Portal.Module;

    using Folder = Chaos.Mcm.Extension.Folder;

    public class McmModule : IModule
    {
        #region Field

        private const string CONFIGURATION_NAME = "MCM";

        #endregion

        #region Properties

        public IExtension[]       Extensions { get; private set; }
        public IMcmRepository     McmRepository { get; private set; }
        public IPermissionManager PermissionManager { get; private set; }

        #endregion
        #region Implementation of IModule

        public void Load(IPortalApplication portalApplication)
        {
            var configuration = portalApplication.PortalRepository.ModuleGet(CONFIGURATION_NAME);
            var connectionString = XDocument.Parse(configuration.Configuration).Root.Attribute("ConnectionString").Value;
            
            McmRepository     = new McmRepository().WithConfiguration(connectionString);
            PermissionManager = new InMemoryPermissionManager().WithSynchronization(new PermissionRepository(McmRepository), new IntervalSpecification(10000));

            Extensions = new IExtension[10];
            Extensions[0] = new Destination(portalApplication, McmRepository, PermissionManager);
            Extensions[1] = new File(portalApplication, McmRepository, PermissionManager);
            Extensions[2] = new Folder(portalApplication, McmRepository, PermissionManager);
            Extensions[3] = new Format(portalApplication, McmRepository, PermissionManager);
            Extensions[4] = new Link(portalApplication, McmRepository, PermissionManager);
            Extensions[5] = new Metadata(portalApplication, McmRepository, PermissionManager);
            Extensions[6] = new MetadataSchema(portalApplication, McmRepository, PermissionManager);
            Extensions[7] = new Object(portalApplication, McmRepository, PermissionManager);
            Extensions[8] = new ObjectRelation(portalApplication, McmRepository, PermissionManager);
            Extensions[9] = new ObjectType(portalApplication, McmRepository, PermissionManager);

            portalApplication.AddExtension("Destination", Extensions[0]);
            portalApplication.AddExtension("File", Extensions[1]);
            portalApplication.AddExtension("Folder", Extensions[2]);
            portalApplication.AddExtension("Format", Extensions[3]);
            portalApplication.AddExtension("Link", Extensions[4]);
            portalApplication.AddExtension("Metadata", Extensions[5]);
            portalApplication.AddExtension("MetadataSchema", Extensions[6]);
            portalApplication.AddExtension("Object", Extensions[7]);
            portalApplication.AddExtension("ObjectRelation", Extensions[8]);
            portalApplication.AddExtension("ObjectType", Extensions[9]);
        }

        #endregion
    }
}
