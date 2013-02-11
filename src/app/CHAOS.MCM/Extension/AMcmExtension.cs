namespace Chaos.Mcm.Extension
{
    using System;
    using System.Linq;
    using System.Xml.Linq;

    using Chaos.Mcm.Data;
    using Chaos.Mcm.Binding;
    using Chaos.Mcm.Permission;
    using Chaos.Portal;
    using Chaos.Portal.Extension;

    [PortalExtension( configurationName: "MCM" )]
    public abstract class AMcmExtension : AExtension
    {
        #region Properties

        private static string ConnectionString { get; set; }

        protected static IPermissionManager PermissionManager { get; set; }

        protected IMcmRepository McmRepository { get; set; }

        #endregion
        #region Construction

        public override IExtension WithConfiguration( string configuration )
        {
            var mcmDefaultRepository = new McmRepository();
//            var permissionManager    = PermissionManager ?? new InMemoryPermissionManager().WithSynchronization(new PermissionRepository(mcmDefaultRepository), new IntervalSpecification(10000));

            return WithConfiguration(configuration, null, mcmDefaultRepository);
        }

        public IExtension WithConfiguration( string configuration, IPermissionManager permissionManager, IMcmRepository mcmRepository )
        {
            var connectionString = XDocument.Parse( configuration ).Root.Attribute( "ConnectionString" ).Value;

            // Provides backwards compaitibility with configurations from versino 5 and earlier.
            ConnectionString = connectionString.Replace( "metadata=res://*/MCM.csdl|res://*/MCM.ssdl|res://*/MCM.msl;", 
                                                         "metadata=res://*/Data.EF.MCM.csdl|res://*/Data.EF.MCM.ssdl|res://*/Data.EF.MCM.msl;" );

            return WithConfiguration(permissionManager, mcmRepository);
        }

        public IExtension WithConfiguration( IPermissionManager permissionManager, IMcmRepository mcmRepository )
        {
            PermissionManager = permissionManager;
            McmRepository     = mcmRepository.WithConfiguration(ConnectionString);

            return this;
        }

        public new IExtension WithPortalApplication(IPortalApplication portalApplication)
        {
            portalApplication.Bindings.Add(typeof(Data.Dto.Metadata), new MetadataBinding());

            return base.WithPortalApplication(portalApplication);
        }

    	#endregion
        #region Business Logic

//        protected void PutObjectInIndex( IIndex index, IEnumerable<Data.Dto.Standard.Object> newObject )
//        {
//            foreach( var o in newObject )
//            {
//                foreach (var ancestorFolder in o.Folders.Where(item => item.ObjectFolderTypeID == 1).SelectMany(folder => PermissionManager.GetFolders(folder.FolderID).GetAncestorFolders()))
//                {
//                    o.FolderTree.Add(ancestorFolder.ID);
//                }
//
//                if (o.ObjectRealtions.Any())
//                    o.RelatedObjects = McmRepository.GetObject(o.Guid, null).ToList();
//            }
//
//            index.Set( newObject.Select(item => item as Data.Dto.Standard.Object), false );
//        }

//        protected void RemoveObjectFromIndex( IIndex index, Data.Dto.Standard.Object delObject )
//        {
//            index.Remove( delObject, false );
//        }

        public bool HasPermissionToObject(ICallContext callContext, Guid objectGuid, FolderPermission permissions)
	    {
            //review: look into using the folder returned from the database directly for the permission check
            var folders    = McmRepository.FolderGet(null, null, objectGuid).Select(item => PermissionManager.GetFolders(item.ID));
            var userGuid   = callContext.User.Guid;
			var groupGuids = callContext.Groups.Select( item => item.Guid );
                   
            return PermissionManager.DoesUserOrGroupHavePermissionToFolders(userGuid, groupGuids, permissions, folders);
	    }

        #endregion
    }
}
