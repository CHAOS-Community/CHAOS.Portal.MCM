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
    using Amazon.S3;
    using Amazon.S3.Model;

    using CHAOS.Portal.Exception;

    [Module("MCM")]
    public class AMCMModule : AModule
    {
        #region Properties

        private static string ConnectionString { get; set; }
        private static string S3Url { get; set; }
        
protected static string AccessKey { get; set; }
        protected static string SecretKey { get; set; }

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
            try
            {
                var root = XDocument.Parse(configuration).Root;

                ConnectionString  = root.Attribute("ConnectionString").Value;
                S3Url             = root.Attribute("S3URL").Value;
                AccessKey         = root.Attribute("AccessKey").Value;
                SecretKey         = root.Attribute("SecretKey").Value;
            }
            catch(NullReferenceException e)
            {
                throw new ModuleConfigurationMissingException("No Module configuration found for MCM", e);
            }

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

        protected void PutObjectInIndex( IIndex index, IEnumerable<Object> newObject )
        {
            foreach( var o in newObject )
            {
                foreach (var ancestorFolder in o.Folders.Where(item => item.ObjectFolderTypeID == 1).SelectMany(folder => PermissionManager.GetFolders(folder.FolderID).GetAncestorFolders()))
                {
                    o.FolderTree.Add(ancestorFolder.ID);
                }

                if (o.ObjectRealtions.Any())
                    o.RelatedObjects = McmRepository.GetObject(o.GUID.ToGuid(), null).ToList();
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

        protected void RemoveFiles(Object delObject)
        {
            foreach (var fileInfo in delObject.Files.Where(item => item.Token == "S3"))
            {
                RemoveFile(fileInfo);
            }
        }
        // todo: how to talk to the underlying infrastructure should be abstracted out of the modules.
        protected void RemoveFile(Data.Dto.Standard.FileInfo file)
        {
            using (var client = new AmazonS3Client(AccessKey, SecretKey))
            {
                //bucketname={BASE_PATH};key={FOLDER_PATH}{FILENAME}   
                var args       = file.URL.Split(';');
                var bucketname = args[0].Substring(11);
                var key        = args[1].Substring(4);

                var request = new DeleteObjectRequest().WithBucketName(bucketname).WithKey(key);

                using(var response = client.DeleteObject(request))
                {
                    if (!response.IsDeleteMarker) throw new ModuleConfigurationMissingException(string.Format("File {0} couldn't be deleted", file.ID));
                }
            }
        }

        #endregion
    }
}
