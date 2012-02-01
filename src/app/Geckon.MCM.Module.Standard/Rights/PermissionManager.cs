using System;
using System.Collections.Generic;
using System.Linq;

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

		public IEnumerable<Folder> GetTopFolders( Guid userGuid, IEnumerable<int> directFolderIDs )
		{
			IList<Folder> folders = new List<Folder>();

		    foreach( int directFolderID in directFolderIDs )
		    {
		        Folder folder = FolderIndex[ directFolderID ];

				if( folder.ParentFolder != null && folders.Contains( folder.ParentFolder ) )
					folders.Remove( folder );

		    	if( IsTopFolder( folder.ParentFolder, userGuid, (int) FolderPermissions.Read ) )
					folders.Add( folder );
		    }

			return folders;
		}

    	private bool IsTopFolder( Folder folder, Guid userGuid, int permissions )
    	{
    		if( folder.ParentFolder == null )
				return true;

			if( folder.DoesUserHavePersmission( userGuid, permissions ) )
				return false;
			
			return IsTopFolder( folder.ParentFolder, userGuid, permissions );
    	}

    	//0-[X]-0-X
		// \0-0
		//   \0-0-0
		//     \[X]-0

		//Iterate through all X's (direct permissions)
		//Navigate to through parents, until Parent == null (Save original ID)
		//Navigate to through parents, until Parent.Permission == READ (Remove original ID)

        #endregion
    }
}
