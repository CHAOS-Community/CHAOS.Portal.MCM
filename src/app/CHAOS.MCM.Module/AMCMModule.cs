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

        private string            ConnectionString { get; set; }
		private Timer             Timer { get; set; }
		protected PermissionManager PermissionManager { get; set; }

		public MCMEntities DefaultMCMEntities { get { return new MCMEntities(ConnectionString); } }

        #endregion
        #region Construction

        public override void Initialize( string configuration )
        {
            ConnectionString  = XDocument.Parse(configuration).Root.Attribute( "ConnectionString" ).Value;
			PermissionManager = new PermissionManager();
			Timer = new Timer( SynchronizeFolders, null, 0, 5000 );
			SynchronizeFolders( null );
        }

    	#endregion
        #region Business Logic

        private object _syncLock = new object();

		private void SynchronizeFolders( object state )
    	{
    		using( MCMEntities db = DefaultMCMEntities )
    		{
				var pm = new PermissionManager();

				foreach( var folder in db.Folder )
				{
					pm.AddFolder( (uint?) folder.ParentID, new Folder( (uint) folder.ID ) );
				}

				foreach( var folderUserJoin in db.Folder_User_Join )
				{
					pm.AddUser( (uint) folderUserJoin.FolderID, folderUserJoin.UserGUID, (FolderPermissions) folderUserJoin.Permission );
				}

				foreach( var folderGroupJoin in db.Folder_Group_Join )
				{
					pm.AddGroup( (uint) folderGroupJoin.FolderID, folderGroupJoin.GroupGUID, (FolderPermissions) folderGroupJoin.Permission );
				}

				lock( _syncLock )
				{
					PermissionManager = pm;
				}
			}
    	}

        protected void PutObjectInIndex( IIndex index, IEnumerable<Data.DTO.Object> newObject )
        {
            foreach( var o in newObject )
			{
				o.FolderTree = PermissionManager.GetParentFolders( o.Folders.Where( item => item.ObjectFolderTypeID == 1 ).Select( item => item.FolderID ) ).ToList();
			}

            index.Set( newObject );
        }

        #endregion
    }
}
