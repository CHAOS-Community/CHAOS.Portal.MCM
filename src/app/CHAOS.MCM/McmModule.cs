using Chaos.Mcm.Extension.Domain;

namespace Chaos.Mcm
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Xml.Linq;

    using CHAOS.Net;

    using Chaos.Mcm.Data;
    using Chaos.Mcm.Extension;
    using Chaos.Mcm.Permission;
    using Chaos.Mcm.Permission.InMemory;
    using Chaos.Mcm.Permission.Specification;
    using Chaos.Mcm.View;
    using Chaos.Portal.Core;
    using Chaos.Portal.Core.Exceptions;
    using Chaos.Portal.Core.Extension;
    using Chaos.Portal.Core.Indexing.Solr;
    using Chaos.Portal.Core.Indexing.View;

    using Folder = Chaos.Mcm.Extension.Folder;

    public class McmModule : IMcmModule
    {
        #region Field

        private const string CONFIGURATION_NAME = "MCM";

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

        #endregion
        #region Implementation of IModule

        public virtual void Load(IPortalApplication portalApplication)
        {
            PortalApplication = portalApplication;

            var configuration    =  XDocument.Parse(PortalApplication.PortalRepository.ModuleGet(CONFIGURATION_NAME).Configuration);
	        var connectionString = GetConfigurationAttribute(configuration, "ConnectionString");
			var objectCoreName   = GetConfigurationAttribute(configuration, "ObjectCoreName");

            ObjectExtensions.ObjectViewName = objectCoreName;

            McmRepository     = new McmRepository().WithConfiguration(connectionString);
            PermissionManager = new InMemoryPermissionManager().WithSynchronization(new PermissionRepository(McmRepository), new IntervalSpecification(10000));

            var objectView = CreateObjectView();
            objectView.WithPortalApplication(PortalApplication);
            objectView.WithCache(PortalApplication.Cache);
            objectView.WithIndex(new SolrCore(new HttpConnection(ConfigurationManager.AppSettings["SOLR_URL"]), objectCoreName));

            portalApplication.ViewManager.AddView(objectView);
        }

	    private string GetConfigurationAttribute(XDocument configuration, string name)
	    {
		    var attribute = configuration.Root.Attribute(name);
		    
			if(attribute == null)
				throw new ConfigurationErrorsException(string.Format("Attribute {0} was not found in configuration", name));

		    return attribute.Value;
	    }

        protected virtual IView CreateObjectView()
        {
            return new ObjectView(PermissionManager);
        }

        public IEnumerable<string> GetExtensionNames(Protocol version)
        {
            yield return "Destination";
            yield return "File";
            yield return "Folder";
            yield return "Format";
            yield return "Link";
            yield return "Metadata";
            yield return "MetadataSchema";
            yield return "Object";
            yield return "ObjectRelation";
            yield return "ObjectType";
            yield return "Mcm";
			yield return "UserProfile";
            yield return "UserManagement";

        }

        public IExtension GetExtension<TExtension>(Protocol version) where TExtension : IExtension
        {
            return GetExtension(version, typeof(TExtension).Name);
        }

        public IExtension GetExtension(Protocol version, string name)
        {
            if (PortalApplication == null) throw new ConfigurationErrorsException("Load not call on module");

            if (version == Protocol.V5)
            {
                switch (name)
                {
                    case "Destination": 
                        return new Destination(PortalApplication, McmRepository, PermissionManager);
                    case "File": 
                        return new File(PortalApplication, McmRepository, PermissionManager);
                    case "Folder": 
                        return new Folder(PortalApplication, McmRepository, PermissionManager);
                    case "Format": 
                        return new Format(PortalApplication, McmRepository, PermissionManager);
                    case "Link": 
                        return new Link(PortalApplication, McmRepository, PermissionManager);
                    case "Metadata": 
                        return new Metadata(PortalApplication, McmRepository, PermissionManager);
                    case "MetadataSchema": 
                        return new MetadataSchema(PortalApplication, McmRepository, PermissionManager);
                    case "Object": 
                        return new Extension.v5.Object(PortalApplication, McmRepository, PermissionManager);
                    case "ObjectRelation": 
                        return new ObjectRelation(PortalApplication, McmRepository, PermissionManager);
                    case "ObjectType": 
                        return new ObjectType(PortalApplication, McmRepository, PermissionManager);
                    case "Mcm": 
                        return new Mcm(PortalApplication, McmRepository, PermissionManager);
					case "UserProfile":
						return new UserProfile(PortalApplication, McmRepository, PermissionManager);
					case "UserManagement":
						return new UserManagement(PortalApplication, McmRepository, PermissionManager).WithConfiguration("<UserManagementConfiguration UsersFolderName=\"Users\" UserFolderTypeId=\"1\" UserObjectTypeId=\"11\" />");
                    default:
                        throw new ExtensionMissingException(string.Format("No extension by the name {0}, found on the Portal Module", name));
                }
            }

            if (version == Protocol.V6)
            {
                switch (name)
                {
                    case "Destination": 
                        return new Destination(PortalApplication, McmRepository, PermissionManager);
                    case "File": 
                        return new File(PortalApplication, McmRepository, PermissionManager);
                    case "Folder": 
                        return new Folder(PortalApplication, McmRepository, PermissionManager);
                    case "Format": 
                        return new Format(PortalApplication, McmRepository, PermissionManager);
                    case "Link": 
                        return new Link(PortalApplication, McmRepository, PermissionManager);
                    case "Metadata": 
                        return new Metadata(PortalApplication, McmRepository, PermissionManager);
                    case "MetadataSchema": 
                        return new MetadataSchema(PortalApplication, McmRepository, PermissionManager);
                    case "Object": 
                        return new Object(PortalApplication, McmRepository, PermissionManager);
                    case "ObjectRelation": 
                        return new ObjectRelation(PortalApplication, McmRepository, PermissionManager);
                    case "ObjectType": 
                        return new ObjectType(PortalApplication, McmRepository, PermissionManager);
                    case "Mcm": 
                        return new Mcm(PortalApplication, McmRepository, PermissionManager);
					case "UserProfile":
						return new UserProfile(PortalApplication, McmRepository, PermissionManager);
					case "UserManagement":
						return new UserManagement(PortalApplication, McmRepository, PermissionManager).WithConfiguration("<UserManagementConfiguration UsersFolderName=\"Users\" UserFolderTypeId=\"1\" UserObjectTypeId=\"11\" />");
                    default:
                        throw new ExtensionMissingException(string.Format("No extension by the name {0}, found on the Portal Module", name));
                }
            }

            throw new ProtocolVersionException();
        }

        #endregion
    }
}
