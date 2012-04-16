using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Xml.Linq;
using CHAOS.MCM.Data.EF;
using CHAOS.Portal.Data.DTO;
using Geckon;
using Geckon.MCM.Core.Exception;
using Geckon.MCM.Module.Standard.Rights;
using Geckon.Portal.Core.Exception;
using Geckon.Portal.Core.Standard.Extension;
using Geckon.Portal.Core.Standard.Module;
using Geckon.Portal.Data;
using Geckon.Portal.Core.Standard;
using Geckon.Index;
using Geckon.Portal.Data.Result;
using Geckon.Portal.Data.Result.Standard;
using Folder = Geckon.MCM.Module.Standard.Rights.Folder;
using Object = CHAOS.MCM.Data.DTO.Object;
using Geckon.Serialization;
using CHAOS.MCM.Data.DTO;

namespace CHAOS.MCM.Module.Standard
{
    public class MCMModule : AModule
    {
        #region Properties

        private String ConnectionString { get; set; }
		private Timer Timer { get; set; }
		private PermissionManager PermissionManager { get; set; }

		public MCMEntities DefaultMCMEntities { get { return new MCMEntities(ConnectionString); } }

        #endregion
        #region Construction

        public override void Init( XElement config )
        {
            ConnectionString  = config.Attribute( "ConnectionString" ).Value;
			PermissionManager = new PermissionManager();
			Timer = new Timer( SynchronizeFolders, null, 1000, 1000 );
			SynchronizeFolders( null );
        }

    	#endregion
        #region Business Logic

		private object _SyncLock = new object();

		private void SynchronizeFolders( object state )
    	{
    		using( MCMEntities db = DefaultMCMEntities )
    		{
				PermissionManager pm = new PermissionManager();

				foreach( Data.EF.Folder folder in db.Folder )
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

				lock (_SyncLock)
				{
					PermissionManager = pm;
				}
			}
    	}

		#region ObjectType

		//[Datatype("ObjectType","Create")]
		//public ObjectType ObjectType_Create( CallContext callContext, string value  )
		//{
		//    using( MCMEntities db = DefaultMCMEntities )
		//    {
		//        int result = db.ObjectType_Insert( value, callContext.User.SystemPermissions ); 

		//        if( result == -100 )
		//            throw new Portal.Core.Exception.InsufficientPermissionsException( "User does not have permission to create an Object Type" );

		//        return db.ObjectType_Get( result, null ).First();
		//    }
		//}

		[Datatype("ObjectType", "Get")]
		public IEnumerable<Data.DTO.ObjectType> ObjectType_Get(CallContext callContext)
		{
			using( MCMEntities db = DefaultMCMEntities )
			{
				return db.ObjectType_Get( null, null ).ToDTO().ToList();
			}
		}

		//[Datatype("ObjectType","Update")]
		//public ScalarResult ObjectType_Update(  CallContext callContext, int id, string newName )
		//{
		//    using( MCMEntities db = DefaultMCMEntities )
		//    {
		//        int result = db.ObjectType_Update(id, newName, callContext.User.SystemPermission);

		//        if( result == -100 )
		//            throw new Portal.Core.Exception.InsufficientPermissionsException( "User does not have permission to update an Object Type" );

		//        return new ScalarResult( result );
		//    }
		//}

		//[Datatype("ObjectType","Delete")]
		//public ScalarResult ObjectType_Delete( CallContext callContext, int id )
		//{
		//    using( MCMEntities db = DefaultMCMEntities )
		//    {
		//        int result = db.ObjectType_Delete( id, null, callContext.User.SystemPermission );

		//        if( result == -100 )
		//            throw new Portal.Core.Exception.InsufficientPermissionsException( "User does not have permission to delete an Object Type" );

		//        return new ScalarResult( result );
		//    }
		//}

		#endregion
		#region Language

		[Datatype("Language", "Get")]
		public IEnumerable<Data.DTO.Language> Language_Get(CallContext callContext, string name, string languageCode)
		{
			using( MCMEntities db = DefaultMCMEntities )
			{
				return db.Language_Get(name, languageCode).ToDTO().ToList();
			}
		}

		//[Datatype("Language", "Create")]
		//public Language Language_Create( CallContext callContext, string name, string languageCode )
		//{
		//    using( MCMEntities db = DefaultMCMEntities )
		//    {
		//        int result = db.Language_Create( name, languageCode, callContext.User.SystemPermission );

		//        if( result == -100 )
		//            throw new Portal.Core.Exception.InsufficientPermissionsException( "User does not have permission to delete an Object Type" );

		//        return db.Language_Get( name, languageCode ).First();
		//    }
		//}

		//[Datatype("Language", "Update")]
		//public ScalarResult Language_Update( CallContext callContext, string languageCode, string newName )
		//{
		//    using( MCMEntities db = DefaultMCMEntities )
		//    {
		//        int result = db.Language_Update( newName, languageCode, callContext.User.SystemPermission );

		//        if( result == -100 )
		//            throw new Portal.Core.Exception.InsufficientPermissionsException( "User does not have permission to delete an Object Type" );

		//        return new ScalarResult( result );
		//    }
		//}

		//[Datatype("Language", "Delete")]
		//public ScalarResult Language_Delete( CallContext callContext, string languageCode )
		//{
		//    using( MCMEntities db = DefaultMCMEntities )
		//    {
		//        int result = db.Language_Delete( languageCode, callContext.User.SystemPermission );

		//        if( result == -100 )
		//            throw new Portal.Core.Exception.InsufficientPermissionsException( "User does not have permission to delete an Object Type" );

		//        return new ScalarResult( result );
		//    }
		//}

		#endregion
		#region ObjectRelationType

		[Datatype("ObjectRelationType", "Get")]
		public IEnumerable<Data.DTO.ObjectRelationType> ObjectRelationType_Get( CallContext callContext, int? id, string value )
		{
			using( MCMEntities db = DefaultMCMEntities )
			{
				return db.ObjectRelationType_Get( id, value ).ToDTO().ToList();
			}
		}

		//[Datatype("ObjectRelationType", "Create")]
		//public ObjectRelationType ObjectRelationType_Create(CallContext callContext, string value)
		//{
		//    using( MCMEntities db = DefaultMCMEntities )
		//    {
		//        int result = db.ObjectRelationType_Create(value, callContext.User.SystemPermission);

		//        if( result == -100 )
		//            throw new Portal.Core.Exception.InsufficientPermissionsException( "User does not have permission to delete an Object Type" );

		//        return db.ObjectRelationType_Get(result, null).First();
		//    }
		//}

		//[Datatype("ObjectRelationType", "Update")]
		//public ScalarResult ObjectRelationType_Update(CallContext callContext, int? id, string value)
		//{
		//    using( MCMEntities db = DefaultMCMEntities )
		//    {
		//        int result = db.ObjectRelationType_Update(id, value, callContext.User.SystemPermission);

		//        if( result == -100 )
		//            throw new Portal.Core.Exception.InsufficientPermissionsException( "User does not have permission to delete an Object Type" );

		//        return new ScalarResult( result );
		//    }
		//}

		//[Datatype("ObjectRelationType", "Delete")]
		//public ScalarResult ObjectRelationType_Delete( CallContext callContext, int? id )
		//{
		//    using( MCMEntities db = DefaultMCMEntities )
		//    {
		//        int result = db.ObjectRelationType_Delete(id, callContext.User.SystemPermission);

		//        if( result == -100 )
		//            throw new Portal.Core.Exception.InsufficientPermissionsException( "User does not have permission to delete an Object Type" );

		//        return new ScalarResult( result );
		//    }
		//}

		#endregion
		#region FolderType

		[Datatype("FolderType", "Get")]
		public IEnumerable<Data.DTO.FolderType> FolderType_Get( CallContext callContext, int? id, string name )
		{
			using( MCMEntities db = DefaultMCMEntities )
			{
				return db.FolderType_Get(id, name).ToDTO().ToList();
			}
		}

		//[Datatype("FolderType", "Create")]
		//public FolderType FolderType_Create(CallContext callContext, string name)
		//{
		//    using (MCMEntities db = DefaultMCMEntities)
		//    {
		//        int result = db.FolderType_Create(name, callContext.User.SystemPermission);

		//        if (result == -100)
		//            throw new Portal.Core.Exception.InsufficientPermissionsException("User does not have permission to delete an Object Type");

		//        return db.FolderType_Get(result, null).First();
		//    }
		//}

		//[Datatype("FolderType", "Update")]
		//public ScalarResult FolderType_Update(CallContext callContext, int? id, string name)
		//{
		//    using (MCMEntities db = DefaultMCMEntities)
		//    {
		//        int result = db.FolderType_Update(id, name, callContext.User.SystemPermission);

		//        if (result == -100)
		//            throw new Portal.Core.Exception.InsufficientPermissionsException("User does not have permission to delete an Object Type");

		//        return new ScalarResult(result);
		//    }
		//}

		//[Datatype("FolderType", "Delete")]
		//public ScalarResult FolderType_Delete(CallContext callContext, int? id)
		//{
		//    using (MCMEntities db = DefaultMCMEntities)
		//    {
		//        int result = db.FolderType_Delete(id, callContext.User.SystemPermission);

		//        if (result == -100)
		//            throw new Portal.Core.Exception.InsufficientPermissionsException("User does not have permission to delete an Object Type");

		//        return new ScalarResult(result);
		//    }
		//}

		#endregion
		#region FormatType

		[Datatype("FormatType", "Get")]
		public IEnumerable<Data.DTO.FormatType> FormatType_Get( CallContext callContext, int? id, string name )
		{
			using( MCMEntities db = DefaultMCMEntities )
			{
				return db.FormatType_Get( id, name ).ToDTO().ToList();
			}
		}

		//[Datatype("FormatType", "Create")]
		//public FormatType FormatType_Create(CallContext callContext, string name)
		//{
		//    using( MCMEntities db = DefaultMCMEntities )
		//    {
		//        int result = db.FormatType_Create(name, callContext.User.SystemPermission);

		//        if( result == -100 )
		//            throw new Portal.Core.Exception.InsufficientPermissionsException("User does not have permission to delete an Object Type");

		//        return db.FormatType_Get(result, null).First();
		//    }
		//}

		//[Datatype("FormatType", "Update")]
		//public ScalarResult FormatType_Update(CallContext callContext, int? id, string name)
		//{
		//    using( MCMEntities db = DefaultMCMEntities )
		//    {
		//        int result = db.FormatType_Update( id, name, callContext.User.SystemPermission );

		//        if( result == -100 )
		//            throw new Portal.Core.Exception.InsufficientPermissionsException("User does not have permission to delete an Object Type");

		//        return new ScalarResult(result);
		//    }
		//}

		//[Datatype("FormatType", "Delete")]
		//public ScalarResult FormatType_Delete(CallContext callContext, int? id)
		//{
		//    using( MCMEntities db = DefaultMCMEntities )
		//    {
		//        int result = db.FormatType_Delete(id, callContext.User.SystemPermission);

		//        if( result == -100 )
		//            throw new Portal.Core.Exception.InsufficientPermissionsException("User does not have permission to delete an Object Type");

		//        return new ScalarResult(result);
		//    }
		//}

		#endregion
		//#region FormatCategory

		////[Datatype("FormatCategory","Get")]

		//#endregion
		//#region Format

		////[Datatype("Format","Get")]
		////public IEnumerable<Format> Format_Get( CallContext callContext,  )

		//#endregion
		#region Folder

		[Datatype("Folder", "Get")]
		public IEnumerable<Data.DTO.FolderInfo> Folder_Get(CallContext callContext, uint? id, uint? folderTypeID, uint? parentID)
		{
			IList<long> folderIDs = new List<long>();

			if( !parentID.HasValue && !id.HasValue )
				folderIDs = PermissionManager.GetFolders( callContext.User.GUID.ToGuid(), callContext.Groups.Select( group => group.GUID.ToGuid() ).ToList() ).Select( folder => (long) folder.ID ).ToList();

			if( parentID.HasValue )
				folderIDs = PermissionManager.GetFolders( callContext.User.GUID.ToGuid(), callContext.Groups.Select( group => group.GUID.ToGuid() ).ToList(), parentID.Value).Select(folder => (long) folder.ID).ToList();

			if (id.HasValue)
			{
				Folder folder = PermissionManager.GetFolder( id.Value );

				if( folder.DoesUserOrGroupHavePersmission( callContext.User.GUID.ToGuid(), callContext.Groups.Select( group => group.GUID.ToGuid() ).ToList(), FolderPermissions.Read, true ) )
					folderIDs.Add( folder.ID );
			}

			using( MCMEntities db = DefaultMCMEntities )
			{
				return db.FolderInfo.Where( fi => folderIDs.Contains<long>( fi.ID ) ).ToDTO().ToList();
			}
		}

		//[Datatype("Folder","Delete")]
		//public ScalarResult Folder_Delete( CallContext callContext, int id )
		//{
		//    using( MCMEntities db = DefaultMCMEntities )
		//    {
		//        int result = db.Folder_Delete( callContext.Groups.Select(group => group.GUID).ToList(), callContext.User.GUID, id );

		//        if( result == -50 )
		//            throw new FolderNotEmptyException( "You cannot delete non empty folder" );
 
		//        if( result == -100 )
		//            throw new Portal.Core.Exception.InsufficientPermissionsException( "User does not have permission to delete the folder" );

		//        return new ScalarResult( result );
		//    }
		//}

		//[Datatype("Folder","Update")]
		//public ScalarResult Folder_Update( CallContext callContext, int id, string newTitle, int? newFolderTypeID )
		//{
		//    using( MCMEntities db = DefaultMCMEntities )
		//    {
		//        int result = db.Folder_Update( callContext.Groups.Select(group => group.GUID).ToList(), callContext.User.GUID, id, newTitle, null, newFolderTypeID );

		//        if( result == -10 )
		//            throw new InvalidProtocolException( "The parameters to update cant all be null" );

		//        if( result == -100 )
		//            throw new InsufficientPermissionsException( "User does not have permission to update the folder" );

		//        return new ScalarResult( result );
		//    }
		//}

		//[Datatype("Folder", "Create")]
		//public FolderInfo Folder_Create( CallContext callContext, string subscriptionGUID, string title, int? parentID, int folderTypeID )
		//{
		//    using( MCMEntities db = DefaultMCMEntities )
		//    {
		//        Guid? subGUID       = null;
		//        int?  subPermission = 0;

		//        if( !string.IsNullOrEmpty( subscriptionGUID ) )
		//        {
		//            SubscriptionInfo subscription = callContext.Subscriptions.FirstOrDefault( sub => sub.GUID.CompareTo( Guid.Parse( subscriptionGUID ) ) == 0 );

		//            subGUID       = subscription.GUID;
		//            subPermission = subscription.Permission;
		//        }

		//        int result = db.Folder_Create( callContext.Groups.Select( group => group.GUID ).ToList(), 
		//                                       callContext.User.GUID,
		//                                       subGUID,
		//                                       subPermission,
		//                                       title, 
		//                                       parentID, 
		//                                       folderTypeID);

		//        if( result == -100 )
		//            throw new Portal.Core.Exception.InsufficientPermissionsException( "User does not have permission to Create the folder" );

		//        return db.Folder_Get( callContext.Groups.Select(group => group.GUID).ToList(),
		//                              callContext.User.GUID, 
		//                              result, 
		//                              null, 
		//                              null ).First();

		//    //    FolderInfo folder = Folder_Get( callContext, result, null, null ).First();

		//     //   return folder;
		//    }
		//}

		#endregion
		#region Object

		[Datatype("Object", "Get")]
		public IPagedResult<IResult> Object_Get(CallContext callContext, IQuery query, bool? includeMetadata, bool? includeFiles, bool? includeObjectRelations)
		{
			using( MCMEntities db = DefaultMCMEntities )
			{
				IEnumerable<UUID> resultPage = null;

				if( query != null )
				{
					//TODO: Implement Folder Permissions Enum Flags (GET OBJECT FLAG)

                    IList<Folder> folders = PermissionManager.GetFolders( callContext.User.GUID.ToGuid(), callContext.Groups.Select( group => group.GUID.ToGuid() ) ).ToList();

					//TODO: Refactor building of queries
					System.Text.StringBuilder sb = new System.Text.StringBuilder(query.Query);
					sb.Append(" AND (");

					for (int i = 0; i < folders.Count(); i++)
					{
						sb.Append(string.Format("FolderTree:{0}", folders[i].ID));

						if (i + 1 < folders.Count())
							sb.Append(" OR ");
					}

					sb.Append(")");

					query.Query = sb.ToString();

					IPagedResult<IIndexResult> indexResult = callContext.IndexManager.GetIndex<MCMModule>().Get(query);

					resultPage = indexResult.Results.Select(result => ((UUIDResult)result).Guid);

					// if solr doesnt return anything there is no need to continue, so just return an empty list
					if (resultPage.Count() == 0)
						return new Geckon.Index.Standard.PagedResult<IResult>(0, 0, new List<Data.DTO.Object>());
					
					return new Geckon.Index.Standard.PagedResult<IResult>(indexResult.FoundCount, query.PageIndex, db.Object_Get(resultPage, includeMetadata ?? false, includeFiles ?? false, includeObjectRelations ?? false).ToDTO().ToList());
				}
			}

			throw new NotImplementedException("No implmentation for Object Get without solr parameters");
		}

		[Datatype("Object","Create")]
		public Object Object_Create( CallContext callContext, UUID GUID, uint objectTypeID, uint folderID )
		{
		    using( MCMEntities db = DefaultMCMEntities )
		    {
				if( !PermissionManager.GetFolder( folderID ).DoesUserOrGroupHavePersmission( callContext.User.GUID.ToGuid(), callContext.Groups.Select( item => item.GUID.ToGuid() ), FolderPermissions.CreateUpdateObjects ) )
					throw new InsufficientPermissionsException( "User does not have permissions to create object" );

				UUID guid = GUID ?? new UUID();

		        int result = db.Object_Create( guid.ToByteArray(), (int) objectTypeID, (int) folderID ).First().Value;

				if( result == -200 )
					throw new UnhandledException("Unhandled exception, Object_Create was rolled back");

		        IEnumerable<Object> newObject = db.Object_Get( new[]{guid}, false, false, false ).ToDTO();

		        PutObjectInIndex( callContext.IndexManager.GetIndex<MCMModule>(), newObject );

		        return newObject.First();
		    }
		}

		//[Datatype("Object", "Delete")]
		//public ScalarResult Object_Delete( CallContext callContext, Guid GUID, int folderID )
		//{
		//    using( MCMEntities db = DefaultMCMEntities )
		//    {
		//        int result = db.Object_Delete( callContext.Groups.Select( group => group.GUID ).ToList(), callContext.User.GUID, GUID, folderID );

		//        if( result == -100 )
		//            throw new InsufficientPermissionsException( "User does not have permissions to delete object" );

		//        PutObjectInIndex( callContext.IndexManager.GetIndex<MCMModule>(), db.Object_Get( new []{ GUID }, true, false, true, true ) );

		//        return new ScalarResult( result );
		//    }
		//}

		//[Datatype("Object", "PutInFolder")]
		//public ScalarResult Object_PutInFolder(CallContext callContext, Guid GUID, int folderID, int objectFolderTypeID)
		//{
		//    using( MCMEntities db = DefaultMCMEntities )
		//    {
		//        int result = db.Object_PutInFolder( callContext.Groups.Select( group => group.GUID ).ToList(), callContext.User.GUID, GUID, folderID, objectFolderTypeID );

		//        if( result == -100 )
		//            throw new InsufficientPermissionsException( "User does not have permissions to put object into folder" );

		//        PutObjectInIndex( callContext.IndexManager.GetIndex<MCMModule>(), db.Object_Get( new []{ GUID }, true, false, true, true ) );

		//        return new ScalarResult( result );
		//    }
		//}

		#endregion
		#region Metadata

		[Datatype("Metadata","Set")]
		public ScalarResult Metadata_Set( CallContext callContext, UUID objectGUID, UUID metadataSchemaGUID, string languageCode, uint? revisionID, string metadataXML )
		{
		    using( MCMEntities db = DefaultMCMEntities )
		    {
		    	bool doesUserHavePermission = false;

				// TODO: Refactor Permission check
				foreach( Data.EF.Folder folder in db.Folder_Get(null, objectGUID.ToByteArray()) )
				{
					if( PermissionManager.GetFolder( (uint) folder.ID ).DoesUserOrGroupHavePersmission( callContext.User.GUID.ToGuid(), callContext.Groups.Select( item => item.GUID.ToGuid() ), FolderPermissions.CreateUpdateObjects ) )
					{
						doesUserHavePermission = true;
						break;
					}
				}

				if( !doesUserHavePermission )
					throw new InsufficientPermissionsException( "User does not have permissions to set metadata on this object" );

		        int result = db.Metadata_Set( new UUID().ToByteArray(), objectGUID.ToByteArray(), metadataSchemaGUID.ToByteArray(), languageCode, (int?) revisionID, metadataXML, callContext.User.GUID.ToByteArray() ).First().Value;
                
                if( result == -300 )
                    throw new InvalidRevisionException( "RevisionID is too old, set metadata with the latest revisionID." );

                if( result == -350 )
                    throw new InvalidRevisionException( "RevisionID can only be null if there is no metadata already on the object" );

                if( result == -200 )
                    throw new UnhandledException( "Metadata Set was rolledback due to an unhandled exception" );

		        PutObjectInIndex( callContext.IndexManager.GetIndex<MCMModule>(), db.Object_Get( new []{ objectGUID }, true, true, true ).ToDTO() );

		        return new ScalarResult( result );
		    }
		}

		//[Datatype("Metadata", "Get")]
		//public IEnumerable<Metadata> Metadata_Get( CallContext callContext, string objectGUID, string metadataSchemaGUID, string languageCode )
		//{
		//    using( MCMEntities db = DefaultMCMEntities )
		//    {
		//        return db.Metadata_Get( Guid.Parse( objectGUID ), metadataSchemaGUID == null ? (Guid?) null : Guid.Parse( metadataSchemaGUID ), languageCode ).ToList();
		//    }
		//}

		#endregion
		#region MetadataSchema

		[Datatype("MetadataSchema", "Get")]
		public IEnumerable<Data.DTO.MetadataSchema> MetadataSchema_Get(CallContext callContext, UUID metadataSchemaGUID )
		{
			using( MCMEntities db = DefaultMCMEntities )
			{
				return db.MetadataSchema_Get( metadataSchemaGUID == null ? null : metadataSchemaGUID.ToByteArray() ).ToDTO().ToList();
			}
		}

		#endregion
		#region Test

		[Datatype("Test","ReIndex")]
		public ScalarResult Test_ReIndex( CallContext callContext, uint? folderID, bool? clearIndex )
		{
            Geckon.Index.Solr.Solr<UUIDResult> index = ( Geckon.Index.Solr.Solr<UUIDResult> )callContext.IndexManager.GetIndex<MCMModule>();

            if( clearIndex.HasValue && clearIndex.Value )
                index.RemoveAll(false);

		    uint pageSize = 1000;

		    for( uint i = 0;; i++ )
		    {
		        // using ensure the Database Context is disposed once in a while, to avoid OOM exceptions
		        using( MCMEntities db = DefaultMCMEntities )
		        {
		            var objects = db.Object_Get( folderID, true, false, false, true, i, pageSize ).ToDTO().ToList();
					
					foreach( Object o in objects )
					{
						o.FolderTree = PermissionManager.GetParentFolders( o.Folders.Select( item => item.FolderID ) ).ToList();
					}

					index.Set( objects.Select( obj => (IIndexable) obj ), true );

		            if( objects.Count() != pageSize )
		                break;
		        }
		    }

		    return new ScalarResult(1);
		}

		#endregion
		#region ObjectRelation

		[Datatype("ObjectRelation", "Create")]
		public ScalarResult ObjectRelation_Create( CallContext callContext, UUID object1GUID, UUID object2GUID, uint objectRelationTypeID, int? sequence )
		{
		    using( MCMEntities db = DefaultMCMEntities )
		    {
		        int? result = db.ObjectRelation_Create( object1GUID.ToByteArray(),
		                                                object2GUID.ToByteArray(),
		                                                (int?) objectRelationTypeID,
		                                                sequence ).First();
                if( !result.HasValue )
                    throw new UnhandledException( "No result was returned from the database when calling ObjectRelation_Create" );

		        if( result == -100 )
		            throw new InsufficientPermissionsException( "The user do not have permission to create object relations" );

		        if( result == -200 )
		            throw new ObjectRelationAlreadyExistException( "The object relation already exists" );

		        return new ScalarResult( result.Value );
		    }
		}

        [Datatype("ObjectRelation", "Delete")]
        public ScalarResult ObjectRelation_Delete( CallContext callContext, UUID object1GUID, UUID object2GUID, uint objectRelationTypeID )
        {
            using( MCMEntities db = DefaultMCMEntities )
            {
                int? result = db.ObjectRelation_Delete( object1GUID.ToByteArray(),
                                                        object2GUID.ToByteArray(),
                                                        (int) objectRelationTypeID ).First();

                if( !result.HasValue )
                    throw new UnhandledException( "ObjectRelation Delete failed on the database" );

                if( result == -100 )
                    throw new InsufficientPermissionsException( "The user do not have permission to delete object relations" );

                return new ScalarResult( result.Value );
            }
        }

		#endregion
		#region Files

		[Datatype("File", "Create")]
		public Data.DTO.File File_Create( CallContext callContext, UUID objectGUID, uint? parentFileID, uint formatID, uint destinationID, string filename, string originalFilename, string folderPath )
		{
            using( MCMEntities db = DefaultMCMEntities )
            {
                if( !PermissionManager.DoesUserOrGroupHavePersmissionToFolders( db.Folder_Get(null, objectGUID.ToByteArray()).Select( item => (uint) item.ID ), callContext.User.GUID.ToGuid(), callContext.Groups.Select( item => item.GUID.ToGuid() ), FolderPermissions.CreateUpdateObjects ) )
                    throw new InsufficientPermissionsException("User does not have permissions to create a file for this object");

		        int id = db.File_Create( objectGUID.ToByteArray(), (int?) parentFileID, (int) formatID, (int) destinationID, filename, originalFilename, folderPath ).First().Value;

		        return db.File_Get( id ).First().ToDTO();
            }
        }

        #endregion
        #region Link

        [Datatype("Link", "Create")]
        public ScalarResult Link_Create(CallContext callContext, UUID objectGUID, uint folderID)
        {
            using( MCMEntities db = DefaultMCMEntities )
            {
                if( !PermissionManager.DoesUserOrGroupHavePersmissionToFolders( db.Folder_Get( null, objectGUID.ToByteArray()).Select( item => (uint) item.ID ), callContext.User.GUID.ToGuid(), callContext.Groups.Select( item => item.GUID.ToGuid() ), FolderPermissions.CreateLink ) )
                    throw new InsufficientPermissionsException("User can only create links");
                
                // TODO: Manage magical number better (ObjectFolderTypeID:2 is link by default)
                int result = db.Object_Folder_Join_Create( objectGUID.ToByteArray(), (int) folderID, 2 ).First().Value;

                if( result == -100 )
                    throw new InsufficientPermissionsException( "User can only create links" );

                return new ScalarResult( result );
            }
        }

        [Datatype("Link", "Update")]
        public ScalarResult Link_Update( CallContext callContext, UUID objectGUID, uint folderID, uint newFolderID )
        {
            using( MCMEntities db = DefaultMCMEntities )
            {
                if( !PermissionManager.DoesUserOrGroupHavePersmissionToFolders( db.Folder_Get( null, objectGUID.ToByteArray()).Select( item => (uint) item.ID ), callContext.User.GUID.ToGuid(), callContext.Groups.Select( item => item.GUID.ToGuid() ), FolderPermissions.CreateLink ) )
                    throw new InsufficientPermissionsException("User does not have permission to update link");

                int result = db.Object_Folder_Join_Update( objectGUID.ToByteArray(), (int) folderID, (int) newFolderID ).First().Value;

                return new ScalarResult( result );
            }
        }

        [Datatype("Link", "Delete")]
        public ScalarResult Link_Delete( CallContext callContext, UUID objectGUID, uint folderID )
        {
            using( MCMEntities db = DefaultMCMEntities )
            {
                if( !PermissionManager.DoesUserOrGroupHavePersmissionToFolders( db.Folder_Get( null, objectGUID.ToByteArray()).Select( item => (uint) item.ID ), callContext.User.GUID.ToGuid(), callContext.Groups.Select( item => item.GUID.ToGuid() ), FolderPermissions.CreateLink ) )
                    throw new InsufficientPermissionsException("User does not have permission to delete link");

                int result = db.Object_Folder_Join_Delete( objectGUID.ToByteArray(), (int) folderID ).First().Value;

                return new ScalarResult( result );
            }
        }

        #endregion
        #region Destination

        [Datatype("Destination", "Get")]
        public IEnumerable<Data.DTO.DestinationInfo> Destination_Get(CallContext callContext, uint destinationID )
        {
            using( MCMEntities db = DefaultMCMEntities )
            {
                return db.DestinationInfo_Get( (int?) destinationID ).ToDTO().ToList();
            }
        }

        #endregion

        private void PutObjectInIndex( IIndex index, IEnumerable<Data.DTO.Object> newObject )
        {
            index.Set( newObject );
        }

		#endregion
    }

}
