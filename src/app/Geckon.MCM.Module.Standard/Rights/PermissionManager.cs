using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Geckon.MCM.Module.Standard.Rights
{
    public class PermissionManager
    {
        #region Properties

        private IDictionary< int, Folder > FolderIndex { get; set; }
		private IList< Folder >            Folders     { get; set; }

        #endregion
        #region Constructors

        public PermissionManager( )
        {
            FolderIndex = new Dictionary< int, Folder >();
            Folders     = new List<Folder>();
        }

        #endregion
        #region Business Logic

        public void Add( Folder folder )
        {
            Add( folder.ID, folder );
        }

        public void Add( int folderID, Folder folder)
        {
            Folders.Add( folder );
            FolderIndex.Add( folder.ID, folder );

            foreach( Folder subFolder in folder.GetSubFolders( true ) )
            {
                FolderIndex.Add( subFolder.ID, subFolder );
            }
        }

        public int Count( )
        {
            return FolderIndex.Count;
        }

        public IEnumerable<Folder> GetFolders()
        {
            return Folders;
        }

        public Folder GetFolder( int id )
        {
            return FolderIndex[ id ];
        }

		public IEnumerable<Folder> GetTopFolders( Guid userGuid, IEnumerable<Guid> groupGuids, IEnumerable<int> directFolderIDs )
		{
			IList<Folder> folders = new List<Folder>();

			foreach( int directFolderID in directFolderIDs )
			{
				Folder folder = FolderIndex[ directFolderID ];

				if( folder.ParentFolder != null && folders.Contains( folder.ParentFolder ) )
					folders.Remove( folder );

				if( IsTopFolder( folder.ParentFolder, userGuid, groupGuids, FolderPermissions.Read ) )
					folders.Add( folder );
			}

			return folders;
		}

    	private bool IsTopFolder( Folder folder, Guid userGuid, IEnumerable<Guid> groupGuids, FolderPermissions permissions )
    	{
    		if( folder.ParentFolder == null )
				return true;

			if( folder.DoesUserOrGroupHavePersmission( userGuid, groupGuids, permissions, false ) )
				return false;
			
			return IsTopFolder( folder.ParentFolder, userGuid, groupGuids, permissions );
    	}

        #endregion
    }
}
