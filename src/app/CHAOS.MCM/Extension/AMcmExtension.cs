using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using CHAOS;
using CHAOS.Extensions;
using CHAOS.Index;
using Chaos.Mcm.Data;
using Chaos.Mcm.Data.EF;
using Chaos.Mcm.Permission;
using Chaos.Mcm.Permission.InMemory;
using Chaos.Mcm.Permission.Specification;
using Chaos.Portal;
using Chaos.Portal.Extension;

namespace Chaos.Mcm.Extension
{
    [PortalExtension( configurationName: "MCM" )]
    public abstract class AMcmExtension : AExtension
    {
        #region Properties

        private static string ConnectionString { get; set; }

		public static IPermissionManager PermissionManager { get; set; }

        public MCMEntities DefaultMCMEntities { get { return new MCMEntities(ConnectionString); } }
        public IMcmRepository McmRepository { get; set; }

        #endregion
        #region Construction

        public override IExtension WithConfiguration( string configuration )
        {
            var mcmDefaultRepository = new McmRepository();
            var permissionManager    = PermissionManager ?? new InMemoryPermissionManager().WithSynchronization(new PermissionRepository(mcmDefaultRepository), new IntervalSpecification(10000));

            return WithConfiguration(configuration, permissionManager, mcmDefaultRepository);
        }

        public IExtension WithConfiguration( string configuration, IPermissionManager permissionManager, IMcmRepository mcmRepository )
        {
            var connectionString = XDocument.Parse( configuration ).Root.Attribute( "ConnectionString" ).Value;

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

    	#endregion
        #region Business Logic

        protected void PutObjectInIndex( IIndex index, IEnumerable<Data.Dto.Standard.Object> newObject )
        {
            foreach( var o in newObject )
            {
                foreach (var ancestorFolder in o.Folders.Where(item => item.ObjectFolderTypeID == 1).SelectMany(folder => PermissionManager.GetFolders(folder.FolderID).GetAncestorFolders()))
                {
                    o.FolderTree.Add(ancestorFolder.ID);
                }

                if (o.ObjectRealtions.Any())
                    o.RelatedObjects = McmRepository.GetObject(o.GUID.ToGuid(), null).ToList();
            }

            index.Set( newObject.Select(item => item as Data.Dto.Standard.Object), false );
        }

        protected void RemoveObjectFromIndex( IIndex index, Data.Dto.Standard.Object delObject )
        {
            index.Remove( delObject, false );
        }

        public bool HasPermissionToObject(ICallContext callContext, UUID objectGUID, FolderPermission permissions)
	    {
		    using( var db = DefaultMCMEntities )
		    {
				var folders    = db.Folder_Get( null, objectGUID.ToByteArray() ).Select( item => PermissionManager.GetFolders((uint) item.ID) );
				var userGUID   = callContext.User.GUID.ToGuid();
				var groupGUIDs = callContext.Groups.Select( item => item.GUID.ToGuid() );
                
                return PermissionManager.DoesUserOrGroupHavePermissionToFolders(userGUID, groupGUIDs, permissions, folders);
		    }

	    }

        #endregion
    }
}
