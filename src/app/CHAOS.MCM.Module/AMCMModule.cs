using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Xml.Linq;
using CHAOS.Extensions;
using CHAOS.Index;
using CHAOS.MCM.Data.DTO;
using CHAOS.MCM.Data.EF;
using CHAOS.MCM.Permission;
using CHAOS.MCM.Permission.InMemory;
using CHAOS.MCM.Permission.Specification;
using CHAOS.Portal.Core;
using CHAOS.Portal.Core.Module;
using Folder = CHAOS.MCM.Module.Rights.Folder;

namespace CHAOS.MCM.Module
{
    [Module("MCM")]
    public class AMCMModule : AModule
    {
        #region Properties

        private static string ConnectionString { get; set; }
        private static Thread SynchronizationThread { get; set; }

		public static IPermissionManager PermissionManager { get; set; }

        public MCMEntities DefaultMCMEntities { get { return new MCMEntities(ConnectionString); } }

        #endregion
        #region Construction

        public override void Initialize( string configuration )
        {
            // TODO: Removed default Permission Manager from Module logic (IoC)
            Initialize(configuration, new InMemoryPermissionManager().WithSynchronization(new PermissionRepository(), new IntervalSpecification(10000)) );
        }

        public void Initialize(string configuration, IPermissionManager permissionManager)
        {
            ConnectionString  = XDocument.Parse(configuration).Root.Attribute("ConnectionString").Value;
            PermissionManager = permissionManager;
        }

    	#endregion
        #region Business Logic

        protected void PutObjectInIndex( IIndex index, IEnumerable<Data.DTO.Object> newObject )
        {
            foreach( var o in newObject )
			{
				o.FolderTree = PermissionManager.GetParentFolders( o.Folders.Where( item => item.ObjectFolderTypeID == 1 ).Select( item => item.FolderID ) ).ToList();
			}

            index.Set( newObject, false );
        }

        protected void RemoveObjectFromIndex( IIndex index, Data.DTO.Object delObject )
        {
            index.Remove( delObject, false );
        }

		public bool HasPermissionToObject( ICallContext callContext, UUID objectGUID, FolderPermissions permissions )
	    {
		    using( var db = DefaultMCMEntities )
		    {
				var folderIDs  = db.Folder_Get( null, objectGUID.ToByteArray() ).Select( item => (uint) item.ID );
				var userGUID   = callContext.User.GUID.ToGuid();
				var groupGUIDs = callContext.Groups.Select( item => item.GUID.ToGuid() );

			    return PermissionManager.DoesUserOrGroupHavePersmissionToFolders( folderIDs, userGUID, groupGUIDs, permissions );
		    }

	    }

        #endregion
    }
}
