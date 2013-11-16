using System;
using System.Collections.Generic;
using System.Linq;
using Chaos.Mcm.Data;
using Chaos.Mcm.Permission;
using Chaos.Portal.Core;
using Chaos.Portal.Core.Extension;

namespace Chaos.Mcm.Extension.v6
{
    public abstract class AMcmExtension : AExtension
    {
        #region Properties

        private static string ConnectionString { get; set; }

        protected static IPermissionManager PermissionManager { get; set; }

        protected IMcmRepository McmRepository { get; set; }

        #endregion
        #region Construction

        protected AMcmExtension(IPortalApplication portalApplication, IMcmRepository mcmRepository, IPermissionManager permissionManager)
            : base(portalApplication)
        {
            McmRepository     = mcmRepository;
            PermissionManager = permissionManager;
        }

        #endregion
        #region Business Logic

//        protected void PutObjectInIndex( IIndex index, IEnumerable<Data.Dto.Standard.Object> newObject )
//        {
//            foreach( var o in newObject )
//            {
//                foreach (var ancestorFolder in o.Folders.Where(item => item.ObjectFolderTypeID == 1).SelectMany(folder => PermissionManager.GetFolders(folder.FolderID).GetAncestorFolders()))
//                {
//                    o.FolderTree.Add(ancestorFolder.ID);
//                }
//
//                if (o.ObjectRealtions.Any())
//                    o.RelatedObjects = McmRepository.GetObject(o.Guid, null).ToList();
//            }
//
//            index.Create( newObject.Select(item => item as Data.Dto.Standard.Object), false );
//        }

//        protected void RemoveObjectFromIndex( IIndex index, Data.Dto.Standard.Object delObject )
//        {
//            index.Remove( delObject, false );
//        }

        public bool HasPermissionToObject(Guid objectGuid, FolderPermission permissions)
	    {
            var userGuid   = Request.User.Guid;
            var groupGuids = Request.Groups.Select(item => item.Guid);
       
            return PermissionManager.HasPermissionToObject(objectGuid, userGuid, groupGuids, permissions);
	    }

        #endregion

        public IEnumerable<IFolder> GetFoldersWithAccess()
        {
            if(Request.IsAnonymousUser) return new List<IFolder>();

            var userGuid   = Request.User.Guid;
            var groupGuids = Request.Groups.Select(group => @group.Guid).ToList();
            
            return PermissionManager.GetFolders(FolderPermission.Read, userGuid, groupGuids).ToList();
        }
    }
}
