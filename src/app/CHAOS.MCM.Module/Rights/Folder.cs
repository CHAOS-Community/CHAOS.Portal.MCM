using System;
using System.Collections.Generic;
using System.Linq;
using CHAOS.MCM.Data.DTO;

namespace CHAOS.MCM.Module.Rights
{
    public class Folder
    {
        #region Properties

		public Folder         ParentFolder { get; set; }
		public uint            ID { get; set; }
        private IList<Folder> SubFolders { get; set; }

		private IDictionary<Guid, FolderPermissions> GroupPermissions { get; set; }
		private IDictionary<Guid, FolderPermissions> UserPermissions { get; set; }


        #endregion
        #region Construction

        public Folder( uint id, Folder parentFolder ) : this( id )
        {
            ParentFolder = parentFolder;
        }

        public Folder( uint id )
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

        //public IEnumerable<PermissionDetails> GetUsersPermissionDetails(bool recursive)
        //{
        //    var permissions = UserPermissions.Select(item => new PermissionDetails((uint) item.Value)).ToList();

        //    return recursive ? GetUsersPermissionDetails(permissions) : permissions;
        //}

        //protected IEnumerable<PermissionDetails> GetUsersPermissionDetails(IList<PermissionDetails> permissions)
        //{
        //    if( ParentFolder == null )
        //        return permissions;

        //    return ParentFolder.GetUsersPermissionDetails(permissions);
        //}

        public FolderPermissions GetUserFolderPermission( Guid userGUID )
        {
            return GetUserParentFolderPermission( userGUID, this );
        }

        public static FolderPermissions GetUserParentFolderPermission( Guid userGUID, Folder folder )
        {
            var currentPermission = FolderPermissions.None;

            if( folder.UserPermissions.ContainsKey( userGUID ) )
                currentPermission = folder.UserPermissions[ userGUID ];

            if( folder.ParentFolder == null )
                return currentPermission;

            return currentPermission | GetUserParentFolderPermission( userGUID, folder.ParentFolder );
        }

        public FolderPermissions GetGroupFolderPermission( IEnumerable<Guid> groupGUIDs )
        {
            return groupGUIDs.Aggregate( FolderPermissions.None, (current, groupGUID) => current | GetGroupParentFolderPermission( groupGUID, this ) );
        }

        public static FolderPermissions GetGroupParentFolderPermission( Guid groupGUID, Folder folder )
        {
            var currentPermission = FolderPermissions.None;

            if( folder.GroupPermissions.ContainsKey( groupGUID ) )
                currentPermission = folder.GroupPermissions[ groupGUID ];

            if( folder.ParentFolder == null )
                return currentPermission;

            return currentPermission | GetGroupParentFolderPermission( groupGUID, folder.ParentFolder );
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

            List<Folder> tmpFolders = new List<Folder>();

            foreach( Folder subFolder in SubFolders )
            {
            	tmpFolders.Add( subFolder );
            	tmpFolders.AddRange( subFolder.GetSubFolders( true ) );
            }

        	return tmpFolders;
        }

        #endregion
	}
}
