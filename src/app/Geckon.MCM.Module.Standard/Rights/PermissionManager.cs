using System;
using System.Collections.Generic;
using System.Linq;

namespace Geckon.MCM.Module.Standard.Rights
{
    public class PermissionManager
    {
        #region Properties

        private IDictionary< int, Folder >         FolderIndex { get; set; }
		private IDictionary< Guid, IList<Folder> > UserFolderIndex { get; set; }
		private IDictionary< Guid, IList<Folder> > GroupFolderIndex { get; set; }
		private IList< Folder >                    Folders     { get; set; }

        #endregion
        #region Constructors

        public PermissionManager( )
        {
            FolderIndex      = new Dictionary< int, Folder >();
            Folders          = new List<Folder>();
			UserFolderIndex  = new Dictionary<Guid, IList<Folder>>();
			GroupFolderIndex = new Dictionary<Guid, IList<Folder>>();
        }

        #endregion
        #region Business Logic

		public void AddFolder( Folder folder )
		{
			AddFolder( null, folder );
		}

		public void AddFolder( int? parentFolderID, Folder folder )
		{
			if( parentFolderID.HasValue )
			{
				Folder parent = GetFolder( parentFolderID.Value );
				
				parent.Add( folder );
				folder.ParentFolder = parent;
			}
			else
				Folders.Add( folder );

		    FolderIndex.Add( folder.ID, folder );

			foreach( Folder subFolder in folder.GetSubFolders( true ) )
			{
				FolderIndex.Add( subFolder.ID, subFolder );
			}
		}

		public void AddUser( int folderID, Guid userGUID, FolderPermissions permissions )
		{
			if( !UserFolderIndex.ContainsKey( userGUID ) )
				UserFolderIndex.Add( userGUID, new List<Folder>() );

			UserFolderIndex[ userGUID ].Add( FolderIndex[ folderID ] );
			FolderIndex[ folderID ].AddUser( userGUID, permissions );
		}

		public void AddGroup( int folderID, Guid groupGUID, FolderPermissions permissions )
		{
			if( !GroupFolderIndex.ContainsKey( groupGUID ) )
				GroupFolderIndex.Add( groupGUID, new List<Folder>() );

			GroupFolderIndex[ groupGUID ].Add( FolderIndex[ folderID ] );

			FolderIndex[ folderID ].AddGroup( groupGUID, permissions );
		}

        public int Count( )
        {
            return FolderIndex.Count;
        }

        public IEnumerable<Folder> GetFolders()
        {
            return Folders;
        }

		public IEnumerable<Folder> GetFolders( Guid userGuid, IEnumerable<Guid> groupGuids )
		{
			List<Folder> directFolders = UserFolderIndex[ userGuid ].ToList();

			foreach( Guid groupGuid in groupGuids )
			{
				directFolders.AddRange( GroupFolderIndex[ groupGuid ] );
			}

			return GetTopFolders( userGuid, groupGuids, directFolders );
		}

		public IEnumerable<Folder> GetFolders( Guid userGuid, IEnumerable<Guid> groupGuids, int parentFolderID )
		{
			List<Folder> directFolders = new List<Folder>();

			if( UserFolderIndex.ContainsKey( userGuid ) )
				directFolders = UserFolderIndex[ userGuid ].ToList();

			foreach( Guid groupGuid in groupGuids )
			{
				if( GroupFolderIndex.ContainsKey( groupGuid ) )
					directFolders.AddRange( GroupFolderIndex[ groupGuid ] );
			}

			if( !FolderIndex.ContainsKey( parentFolderID ) )
				yield break;

			foreach( Folder subFolder in FolderIndex[ parentFolderID ].GetSubFolders( false ) )
			{
				if( subFolder.DoesUserOrGroupHavePersmission( userGuid, groupGuids, FolderPermissions.Read, true ) )
					yield return subFolder;
			}
		}

        public Folder GetFolder( int id )
        {
            return FolderIndex[ id ];
        }

		public IEnumerable<Folder> GetTopFolders( Guid userGuid, IEnumerable<Guid> groupGuids, IEnumerable<Folder> directFolderIDs )
		{
			IList<Folder> folders = new List<Folder>();

			foreach( Folder directFolder in directFolderIDs )
			{
				Folder folder = FolderIndex[ directFolder.ID ];

				if( folder.ParentFolder != null && folders.Contains( folder.ParentFolder ) )
					folders.Remove( folder );

				if( IsTopFolder( folder.ParentFolder, userGuid, groupGuids, FolderPermissions.Read ) )
					folders.Add( folder );
			}

			return folders;
		}

    	private bool IsTopFolder( Folder folder, Guid userGuid, IEnumerable<Guid> groupGuids, FolderPermissions permissions )
    	{
			if( folder == null )
				return true;

			if( folder.DoesUserOrGroupHavePersmission( userGuid, groupGuids, permissions, false ) )
				return false;

			// If it's the top folder has been reached then there is nothing 
			if( folder.ParentFolder == null )
				return true;
			
			return IsTopFolder( folder.ParentFolder, userGuid, groupGuids, permissions );
    	}

		public void Clear()
    	{
    		FolderIndex.Clear();
			Folders.Clear();
    	}

        #endregion
	}
}
