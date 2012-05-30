using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Xml.Linq;
using CHAOS.Index;
using CHAOS.MCM.Data.EF;
using CHAOS.MCM.Module.Rights;
using CHAOS.Portal.Core.Module;
using Folder = CHAOS.MCM.Module.Rights.Folder;

namespace CHAOS.MCM.Module
{
    [Module("MCM")]
    public class AMCMModule : AModule
    {
        #region Properties

        private string        ConnectionString { get; set; }
        private static Thread SynchronizationThread { get; set; }
        
        protected static PermissionManager PermissionManager { get; set; }

        public MCMEntities DefaultMCMEntities { get { return new MCMEntities(ConnectionString); } }

        #endregion
        #region Construction

        public override void Initialize( string configuration )
        {
            ConnectionString  = XDocument.Parse(configuration).Root.Attribute( "ConnectionString" ).Value;
            PermissionManager = SynchronizeFoldersOnce();
            SynchronizationThread = new Thread( SynchronizeFolders );
            SynchronizationThread.Start();
        }

    	#endregion
        #region Business Logic

		protected void SynchronizeFolders( )
    	{
            while( true )
            {
                PermissionManager = SynchronizeFoldersOnce();

                Thread.Sleep(5 * 1000);
            }
    	}

        private PermissionManager SynchronizeFoldersOnce()
        {
            using( var db = DefaultMCMEntities )
            {
                var pm = new PermissionManager();

                foreach( var folder in db.Folder )
                {
                    pm.AddFolder((uint?) folder.ParentID, new Folder((uint) folder.ID));
                }

                foreach (var folderUserJoin in db.Folder_User_Join)
                {
                    pm.AddUser((uint) folderUserJoin.FolderID, folderUserJoin.UserGUID,
                               (FolderPermissions) folderUserJoin.Permission);
                }

                foreach (var folderGroupJoin in db.Folder_Group_Join)
                {
                    pm.AddGroup((uint) folderGroupJoin.FolderID, folderGroupJoin.GroupGUID,
                                (FolderPermissions) folderGroupJoin.Permission);
                }

                 return pm;
            }
        }

        protected void PutObjectInIndex( IIndex index, IEnumerable<Data.DTO.Object> newObject )
        {
            foreach( var o in newObject )
			{
				o.FolderTree = PermissionManager.GetParentFolders( o.Folders.Where( item => item.ObjectFolderTypeID == 1 ).Select( item => item.FolderID ) ).ToList();
			}

            index.Set( newObject, false );
        }

        #endregion
    }
}
