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

		private IDictionary<int, IList<Guid>> GroupPermissions { get; set; }
		private IDictionary<int, IList<Guid>> UserPermissions { get; set; }

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
			GroupPermissions = new Dictionary<int, IList<Guid>>();
			UserPermissions  = new Dictionary<int, IList<Guid>>();
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

		public virtual void AddGroup( Guid groupGUID, int permission )
        {
            if( !GroupPermissions.ContainsKey( permission ) )
				GroupPermissions.Add(permission, new List<Guid>());

			GroupPermissions[ permission ].Add( groupGUID );
        }

		public virtual void AddUser( Guid userGUID, int permission )
        {
            if( !UserPermissions.ContainsKey( permission ) )
				UserPermissions.Add(permission, new List<Guid>());

			UserPermissions[ permission ].Add( userGUID );
        }

		public bool DoesUserOrGroupHavePersmission( Guid userGUID, IEnumerable<Guid> groupGUIDs, int permission )
		{
		    if( UserPermissions.ContainsKey( permission ) )
		        return UserPermissions[ permission ].Where( guid => guid.Equals( userGUID ) ).FirstOrDefault() != null;

			if( GroupPermissions.ContainsKey( permission ) )
				return GroupPermissions[ permission ].Where( guid => groupGUIDs.Contains( guid ) ).FirstOrDefault( ) != null;

			return false;
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
