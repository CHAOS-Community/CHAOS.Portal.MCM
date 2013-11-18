using Chaos.Portal.Core;
using Chaos.Portal.Core.Exceptions;
using Chaos.Portal.Core.Extension;
using Chaos.Portal.Core.Indexing.Solr;
using Chaos.Portal.Core.Indexing.View;

namespace Chaos.Mcm
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Xml.Linq;

    using CHAOS.Net;
    using CHAOS.Serialization.Standard;
    using Configuration;
    using Data;
    using Extension.Domain;
    using Extension.v5.Download;
    using Permission;
    using Permission.InMemory;
    using Permission.Specification;
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

        private McmModuleConfiguration McmModuleConfiguration { get; set; }

        #endregion
        #region Implementation of IModule

        public virtual void Load(IPortalApplication portalApplication)
        {
            PortalApplication = portalApplication;
            
            var configuration = XDocument.Parse(PortalApplication.PortalRepository.ModuleGet(ConfigurationName).Configuration);
            McmModuleConfiguration = SerializerFactory.Get<XDocument>().Deserialize<McmModuleConfiguration>(configuration);

            McmRepository = new McmRepository().WithConfiguration(McmModuleConfiguration.ConnectionString);
            PermissionManager = new InMemoryPermissionManager().WithSynchronization(McmRepository, new IntervalSpecification(10000));

            var objectView = CreateObjectView();
            objectView.WithPortalApplication(PortalApplication);
            objectView.WithCache(PortalApplication.Cache);
            objectView.WithIndex(new SolrCore(new HttpConnection(ConfigurationManager.AppSettings["SOLR_URL"]), McmModuleConfiguration.ObjectCoreName));

            ObjectExtensions.ObjectViewName = objectView.Name;

            portalApplication.ViewManager.AddView(objectView);
        }

        protected virtual IView CreateObjectView()
        {
            return new ObjectView(PermissionManager);
        }

        public virtual IEnumerable<string> GetExtensionNames(Protocol version)
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
            
            if(version == Protocol.V5)
                yield return "Download";

        }

        public IExtension GetExtension<TExtension>(Protocol version) where TExtension : IExtension
        {
            return GetExtension(version, typeof(TExtension).Name);
        }

        public virtual IExtension GetExtension(Protocol version, string name)
        {
            if (PortalApplication == null) throw new ConfigurationErrorsException("Load not call on module");

            if (version == Protocol.V5)
            {
                switch (name)
                {
                    case "Destination": 
                        return new Extension.v6.Destination(PortalApplication, McmRepository, PermissionManager);
                    case "File":
                        return new Extension.v6.File(PortalApplication, McmRepository, PermissionManager);
                    case "Folder":
                        return new Extension.v6.Folder(PortalApplication, McmRepository, PermissionManager);
                    case "Format":
                        return new Extension.v6.Format(PortalApplication, McmRepository, PermissionManager);
                    case "Link":
                        return new Extension.v6.Link(PortalApplication, McmRepository, PermissionManager);
                    case "Metadata":
                        return new Extension.v6.Metadata(PortalApplication, McmRepository, PermissionManager);
                    case "MetadataSchema":
                        return new Extension.v6.MetadataSchema(PortalApplication, McmRepository, PermissionManager);
                    case "Object": 
                        return new Extension.v5.Object(PortalApplication, McmRepository, PermissionManager);
                    case "ObjectRelation":
                        return new Extension.v6.ObjectRelation(PortalApplication, McmRepository, PermissionManager);
                    case "ObjectType":
                        return new Extension.v6.ObjectType(PortalApplication, McmRepository, PermissionManager);
                    case "Mcm": 
                        return new Extension.v6.Mcm(PortalApplication, McmRepository, PermissionManager);
					case "Download":
                        return new Download(PortalApplication, McmRepository, McmModuleConfiguration);
                    case "UserProfile":
                        return new Extension.v6.UserProfile(PortalApplication, McmRepository, PermissionManager);
					case "UserManagement":
						return new Extension.v6.UserManagement(PortalApplication, McmRepository, PermissionManager, McmModuleConfiguration.UserManagement);
                    default:
                        throw new ExtensionMissingException(string.Format("No extension by the name {0}, found on the Portal Module", name));
                }
            }

            if (version == Protocol.V6)
            {
                switch (name)
                {
                    case "Destination":
                        return new Extension.v6.Destination(PortalApplication, McmRepository, PermissionManager);
                    case "File":
                        return new Extension.v6.File(PortalApplication, McmRepository, PermissionManager);
                    case "Folder":
                        return new Extension.v6.Folder(PortalApplication, McmRepository, PermissionManager);
                    case "Format":
                        return new Extension.v6.Format(PortalApplication, McmRepository, PermissionManager);
                    case "Link":
                        return new Extension.v6.Link(PortalApplication, McmRepository, PermissionManager);
                    case "Metadata":
                        return new Extension.v6.Metadata(PortalApplication, McmRepository, PermissionManager);
                    case "MetadataSchema":
                        return new Extension.v6.MetadataSchema(PortalApplication, McmRepository, PermissionManager);
                    case "Object":
                        return new Extension.v6.Object(PortalApplication, McmRepository, PermissionManager);
                    case "ObjectRelation":
                        return new Extension.v6.ObjectRelation(PortalApplication, McmRepository, PermissionManager);
                    case "ObjectType":
                        return new Extension.v6.ObjectType(PortalApplication, McmRepository, PermissionManager);
                    case "Mcm": 
                        return new Extension.v6.Mcm(PortalApplication, McmRepository, PermissionManager);
					case "UserProfile":
                        return new Extension.v6.UserProfile(PortalApplication, McmRepository, PermissionManager);
					case "UserManagement":
						return new Extension.v6.UserManagement(PortalApplication, McmRepository, PermissionManager, McmModuleConfiguration.UserManagement);
                    default:
                        throw new ExtensionMissingException(string.Format("No extension by the name {0}, found on the Portal Module", name));
                }
            }

            throw new ProtocolVersionException();
        }

        #endregion
    }
}
