namespace Chaos.Mcm
{
    using Chaos.Mcm.Extension;

    using Chaos.Portal;
    using Chaos.Portal.Extension;
    using Chaos.Portal.Module;

    public class McmModule : IModule
    {
        #region Field

        private const string CONFIGURATION_NAME = "MCM";

        #endregion
        #region Properties

        public IExtension[] Extensions { get; private set; }

        #endregion
        #region Implementation of IModule

        public void Load(IPortalApplication portalApplication)
        {
            var configuration = portalApplication.PortalRepository.ModuleGet(CONFIGURATION_NAME);

            Extensions = new IExtension[10];
            Extensions[0] = new Destination();
            Extensions[1] = new File();
            Extensions[2] = new Folder();
            Extensions[3] = new Format();
            Extensions[4] = new Link();
            Extensions[5] = new Metadata();
            Extensions[6] = new MetadataSchema();
            Extensions[7] = new Object();
            Extensions[8] = new ObjectRelation();
            Extensions[9] = new ObjectType();

            foreach (var extension in Extensions)
            {
                extension.WithPortalApplication(portalApplication);
                extension.WithConfiguration(configuration.Configuration);
            }

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
