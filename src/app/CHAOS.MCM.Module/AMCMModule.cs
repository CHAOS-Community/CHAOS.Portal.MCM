using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using CHAOS.Extensions;
using CHAOS.Index;
using CHAOS.MCM.Data.Dto;
using CHAOS.MCM.Data.EF;
using CHAOS.MCM.Permission;
using CHAOS.MCM.Permission.InMemory;
using CHAOS.MCM.Permission.Specification;
using CHAOS.Portal.Core;
using CHAOS.Portal.Core.Module;
using Chaos.Mcm.Data;
using FolderPermission = CHAOS.MCM.Permission.FolderPermission;
using Object = CHAOS.MCM.Data.Dto.Standard.Object;

namespace CHAOS.MCM.Module
{
    [Module("MCM")]
    public class AMCMModule : AModule
    {
        #region Properties

        private static string ConnectionString { get; set; }

		public static IPermissionManager PermissionManager { get; set; }

        public MCMEntities DefaultMCMEntities { get { return new MCMEntities(ConnectionString); } }
        public IMcmRepository McmRepository { get; set; }

        #endregion
        #region Construction

        public override void Initialize( string configuration )
        {
            var mcmDefaultRepository = new McmRepository();

            // TODO: Removed default Permission Manager from Module logic (DI)
            Initialize(configuration,
                       PermissionManager ?? new InMemoryPermissionManager().WithSynchronization(new PermissionRepository(mcmDefaultRepository), new IntervalSpecification(10000)),
                       mcmDefaultRepository);
        }

        public void Initialize(string configuration, IPermissionManager permissionManager, IMcmRepository mcmRepository)
        {
            ConnectionString  = XDocument.Parse(configuration).Root.Attribute("ConnectionString").Value;
            PermissionManager = permissionManager;
            McmRepository     = mcmRepository.WithConfiguration(ConnectionString);
        }

        public void Initialize(IPermissionManager permissionManager, IMcmRepository mcmRepository)
        {
            PermissionManager = permissionManager;
            McmRepository     = mcmRepository.WithConfiguration(ConnectionString);
        }

    	#endregion
        #region Business Logic

        protected void PutObjectInIndex( IIndex index, IEnumerable<IObject> newObject )
        {
            foreach( var o in newObject )
            {
                foreach (var ancestorFolder in o.Folders.Where(item => item.ObjectFolderTypeID == 1).SelectMany(folder => PermissionManager.GetFolders(folder.FolderID).GetAncestorFolders()))
                {
                    o.FolderTree.Add(ancestorFolder.ID);
                }
            }

            index.Set( newObject.Select(item => item as Object), false );
        }

        protected void RemoveObjectFromIndex( IIndex index, Object delObject )
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
