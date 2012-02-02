using System;
using System.Collections.Generic;
using System.Linq;

namespace Geckon.MCM.Module.Standard.Rights
{
    public class Folder
    {
        #region Properties

        public  int           ID { get; set; }
		public Folder         ParentFolder { get; private set; }
        private IList<Folder> SubFolders { get; set; }

		private IDictionary<Guid, FolderPermissions> GroupPermissions { get; set; }
		private IDictionary<Guid, FolderPermissions> UserPermissions { get; set; }

        #endregion
        #region Construction

        public Folder( int id, Folder parentFolder ) : this( id )
        {
            ParentFolder = parentFolder;
        }

        public Folder( int id )
        {
            ID               = id;
            SubFolders       = new List<Folder>();
			GroupPermissions = new Dictionary<Guid, FolderPermissions>();
			UserPermissions  = new Dictionary<Guid, FolderPermissions>();
        }

        #endregion
        #region Business Logic

        public virtual void Add( IEnumerable<Folder> folders )
        {
            foreach( Folder folder in folders )
            {
                Add( folder );
            }
        }

        public virtual void Add( Folder folder )
        {
            folder.ParentFolder = this;

            SubFolders.Add( folder );
        }

		public virtual void AddGroup( Guid groupGUID, FolderPermissions permission )
        {
			GroupPermissions.Add( groupGUID, permission );
        }

		public virtual void AddUser( Guid userGUID, FolderPermissions permission )
        {
			UserPermissions.Add( userGUID, permission );
        }

		public bool DoesUserOrGroupHavePersmission( Guid userGUID, IEnumerable<Guid> groupGUIDs, FolderPermissions permission )
		{
		    return DoesUserOrGroupHavePersmission( userGUID, groupGUIDs, permission, true );
		}

		public bool DoesUserOrGroupHavePersmission( Guid userGUID, IEnumerable<Guid> groupGUIDs, FolderPermissions permission, bool recursive )
		{
			if( UserPermissions.ContainsKey( userGUID ) && ( UserPermissions[ userGUID ] & permission ) == permission )
		        return true;

			foreach( Guid groupGUID in groupGUIDs )
			{
				if( GroupPermissions.ContainsKey( groupGUID ) && ( GroupPermissions[ groupGUID ] & permission ) == permission )
					return true;
			}

			if( !recursive )
				return false;

			if( ParentFolder == null )
				return false;

			return ParentFolder.DoesUserOrGroupHavePersmission( userGUID, groupGUIDs, permission );
		}

        public int Count()
        {
            int count = 0;

            count = SubFolders.Count;

            foreach( Folder folder in SubFolders )
            {
                count += folder.Count();
            }

            return count;
        }

        public IEnumerable<Folder> GetSubFolders()
        {
            return GetSubFolders( false );
        }

        public IEnumerable<Folder> GetSubFolders( bool isRecursive )
        {
            if( !isRecursive )
                return SubFolders;

            IList<Folder> tmpFolders = new List<Folder>();

            foreach( Folder subFolder in SubFolders )
            {
                tmpFolders.Add( subFolder );

                foreach( Folder folder in subFolder.GetSubFolders( true ) )
                {
                    tmpFolders.Add( folder );
                }
            }

            return tmpFolders;
        }

        #endregion
    }
}
