using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using CHAOS.Index.Solr;
using CHAOS.MCM.Core.Exception;
using CHAOS.MCM.Data.EF;
using CHAOS.Portal.Core.Module;
using CHAOS.Portal.DTO.Standard;
using CHAOS.Portal.Exception;
using CHAOS.Portal.Core;
using FolderPermission = CHAOS.MCM.Permission.FolderPermission;
using FormatType = CHAOS.MCM.Data.Dto.Standard.FormatType;
using Language = CHAOS.MCM.Data.Dto.Standard.Language;
using ObjectRelationType = CHAOS.MCM.Data.Dto.Standard.ObjectRelationType;

namespace CHAOS.MCM.Module
{
    [Module("MCM")]
    public class MCMModule : AMCMModule
    {
        #region Business Logic

		#region Language

		[Datatype("Language", "Get")]
		public IEnumerable<Language> Language_Get( ICallContext callContext, string name, string languageCode )
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
		public IEnumerable<ObjectRelationType> ObjectRelationType_Get( ICallContext callContext, int? id, string value )
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
        public IEnumerable<Data.Dto.Standard.FolderType> FolderType_Get(ICallContext callContext, int? id, string name)
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
		public IEnumerable<FormatType> FormatType_Get( ICallContext callContext, int? id, string name )
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

		#region Object

        //[Datatype("Object", "Get")]
        //public IPagedResult<IResult> Get( ICallContext callContext, IQuery query, bool? includeMetadata, bool? includeFiles, bool? includeObjectRelations, bool? includeAccessPoints )
        //{
        //    using( var db = DefaultMCMEntities )
        //    {
        //        IEnumerable<UUID> resultPage = null;

        //        if( query != null )
        //        {
        //            //TODO: Implement Folder Permissions Enum Flags (GET OBJECT FLAG)

        //            var folders = PermissionManager.GetFolders( callContext.User.GUID.ToGuid(), callContext.Groups.Select( group => group.GUID.ToGuid() ) ).ToList();

        //            //TODO: Refactor building of queries
        //            var sb = new System.Text.StringBuilder(query.Query);
        //            sb.Append(" AND (");

        //            for (int i = 0; i < folders.Count(); i++)
        //            {
        //                sb.Append(string.Format("FolderTree:{0}", folders[i].ID));

        //                if (i + 1 < folders.Count())
        //                    sb.Append(" OR ");
        //            }

        //            sb.Append(")");

        //            query.Query = sb.ToString();

        //            var indexResult = callContext.IndexManager.GetIndex<MCMModule>().Get(query);

        //            resultPage = indexResult.Results.Select(result => ((UUIDResult)result).Guid);

        //            // if solr doesnt return anything there is no need to continue, so just return an empty list
        //            if( !resultPage.Any() )
        //                return new PagedResult<IResult>(0, 0, new List<Data.DTO.Object>());
					
        //            return new PagedResult<IResult>(indexResult.FoundCount, query.PageIndex, db.Get(resultPage, includeMetadata ?? false, includeFiles ?? false, includeObjectRelations ?? false, false, includeAccessPoints ?? false ).ToDTO().ToList());
        //        }
        //    }

        //    throw new NotImplementedException("No implmentation for Object Get without solr parameters");
        //}

        //[Datatype("Object","Create")]
        //public Object Create( ICallContext callContext, UUID GUID, uint objectTypeID, uint folderID )
        //{
        //    using( var db = DefaultMCMEntities )
        //    {
        //        if( !PermissionManager.GetFolder( folderID ).DoesUserOrGroupHavePersmission( callContext.User.GUID.ToGuid(), callContext.Groups.Select( item => item.GUID.ToGuid() ), FolderPermissions.CreateUpdateObjects ) )
        //            throw new InsufficientPermissionsException( "User does not have permissions to create object" );

        //        var guid = GUID ?? new UUID();

        //        int result = db.Create( guid.ToByteArray(), (int) objectTypeID, (int) folderID ).First().Value;

        //        if( result == -200 )
        //            throw new UnhandledException("Unhandled exception, Create was rolled back");

        //        var newObject = db.Get( guid, true, true, true, true, true ).ToDTO().ToList();

        //        if( newObject == null )
        //            throw new UnhandledException("Error retrieving object from DB");

        //        PutObjectInIndex( callContext.IndexManager.GetIndex<MCMModule>(), newObject );

        //        return newObject.First();
        //    }
        //}

		//[Datatype("Object", "Delete")]
		//public ScalarResult Object_Delete( CallContext callContext, Guid GUID, int folderID )
		//{
		//    using( MCMEntities db = DefaultMCMEntities )
		//    {
		//        int result = db.Object_Delete( callContext.Groups.Select( group => group.GUID ).ToList(), callContext.User.GUID, GUID, folderID );

		//        if( result == -100 )
		//            throw new InsufficientPermissionsException( "User does not have permissions to delete object" );

		//        PutObjectInIndex( callContext.IndexManager.GetIndex<MCMModule>(), db.Get( new []{ GUID }, true, false, true, true ) );

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

		//        PutObjectInIndex( callContext.IndexManager.GetIndex<MCMModule>(), db.Get( new []{ GUID }, true, false, true, true ) );

		//        return new ScalarResult( result );
		//    }
		//}

		#endregion
		#region Metadata

		[Datatype("Metadata","Set")]
		public ScalarResult Metadata_Set( ICallContext callContext, UUID objectGUID, UUID metadataSchemaGUID, string languageCode, uint? revisionID, string metadataXML )
		{
		    // TODO: replace with proper XML validation, Quick ugly fix, to make sure it's valid XML
            XDocument.Parse( metadataXML );

		    using( var db = DefaultMCMEntities )
		    {
				if( !HasPermissionToObject( callContext, objectGUID, FolderPermission.CreateUpdateObjects ) )
					throw new InsufficientPermissionsException( "User does not have permissions to set metadata on this object" );

		        var result = db.Metadata_Set( new UUID().ToByteArray(), objectGUID.ToByteArray(), metadataSchemaGUID.ToByteArray(), languageCode, (int?) revisionID, metadataXML, callContext.User.GUID.ToByteArray() ).FirstOrDefault();

                if (!result.HasValue)
                    throw new UnhandledException("Metadata set failed on the database and was rolled back");

                if( result.Value == -300 )
                    throw new InvalidRevisionException( "RevisionID is too old, set metadata with the latest revisionID." );

                if( result.Value == -350 )
                    throw new InvalidRevisionException( "RevisionID can only be null if there is no metadata already on the object" );

                if( result.Value == -200 )
                    throw new UnhandledException( "Metadata Set was rolledback due to an unhandled exception" );

                var objects = db.Object_Get( objectGUID, true, false, false, true, true ).ToDTO().ToList();

		        PutObjectInIndex( callContext.IndexManager.GetIndex<MCMModule>(), objects );

		        return new ScalarResult( result.Value );
		    }
		}

	    #endregion
		#region Test

		[Datatype("Test","ReIndex")]
		public ScalarResult Test_ReIndex( ICallContext callContext, uint? folderID, bool? clearIndex )
		{
            var index = (Solr)callContext.IndexManager.GetIndex<MCMModule>();

            if (clearIndex.HasValue && clearIndex.Value)
                index.RemoveAll(false);

            const uint pageSize = 1000;

            for (uint i = 0; ; i++)
            {
                // using ensure the Database Context is disposed once in a while, to avoid OOM exceptions
                using (var db = DefaultMCMEntities)
                {
                    var objects = db.Object_Get(folderID, true, false, true, true, true, i, pageSize).ToDTO().ToList();

                    PutObjectInIndex(index, objects);

                    if (objects.Count() != pageSize)
                        break;
                }
            }

            return new ScalarResult(1);
		}

		#endregion
		#region ObjectRelation

		[Datatype("ObjectRelation", "Create")]
		public ScalarResult ObjectRelation_Create( ICallContext callContext, UUID object1GUID, UUID object2GUID, uint objectRelationTypeID, int? sequence )
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
        public ScalarResult ObjectRelation_Delete( ICallContext callContext, UUID object1GUID, UUID object2GUID, uint objectRelationTypeID )
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
        #region Link

        [Datatype("Link", "Create")]
        public ScalarResult Link_Create( ICallContext callContext, UUID objectGUID, uint folderID)
        {
            using( MCMEntities db = DefaultMCMEntities )
            {
                if( !HasPermissionToObject( callContext, objectGUID, FolderPermission.CreateLink ) )
                    throw new InsufficientPermissionsException("User can only create links");
                
                // TODO: Manage magical number better (ObjectFolderTypeID:2 is link by default)
                var result = db.Object_Folder_Join_Create( objectGUID.ToByteArray(), (int) folderID, 2 ).FirstOrDefault();

                if(!result.HasValue)
                    throw new UnhandledException("Link create failed on the database and was rolled back");

                if( result.Value == -100 )
                    throw new InsufficientPermissionsException( "User can only create links" );

                PutObjectInIndex( callContext.IndexManager.GetIndex<MCMModule>(), db.Object_Get( objectGUID , true, true, true, true, true ).ToDTO().ToList() );

                return new ScalarResult( result.Value );
            }
        }

        [Datatype("Link", "Update")]
        public ScalarResult Link_Update( ICallContext callContext, UUID objectGUID, uint folderID, uint newFolderID )
        {
            using( MCMEntities db = DefaultMCMEntities )
            {
                if( !HasPermissionToObject( callContext, objectGUID, FolderPermission.CreateLink ) )
                    throw new InsufficientPermissionsException("User does not have permission to update link");

                int result = db.Object_Folder_Join_Update( objectGUID.ToByteArray(), (int) folderID, (int) newFolderID ).First().Value;

                PutObjectInIndex( callContext.IndexManager.GetIndex<MCMModule>(), db.Object_Get( objectGUID , true, true, true, true, true ).ToDTO().ToList() );

                return new ScalarResult( result );
            }
        }

        [Datatype("Link", "Delete")]
        public ScalarResult Link_Delete( ICallContext callContext, UUID objectGUID, uint folderID )
        {
            using( MCMEntities db = DefaultMCMEntities )
            {
                if( !HasPermissionToObject( callContext, objectGUID, FolderPermission.CreateLink ) )
                    throw new InsufficientPermissionsException("User does not have permission to delete link");

                var result = db.Object_Folder_Join_Delete( objectGUID.ToByteArray(), (int) folderID ).FirstOrDefault();

                if(!result.HasValue)
                    throw new UnhandledException("Link delete failed on the database and was rolled back");

                PutObjectInIndex( callContext.IndexManager.GetIndex<MCMModule>(), db.Object_Get( objectGUID , true, true, true, true, true ).ToDTO().ToList() );

                return new ScalarResult( result.Value );
            }
        }

        #endregion
        #region Destination

        [Datatype("Destination", "Get")]
        public IEnumerable<Data.Dto.Standard.DestinationInfo> Destination_Get(ICallContext callContext, uint destinationID)
        {
            using( MCMEntities db = DefaultMCMEntities )
            {
                return db.DestinationInfo_Get( (int?) destinationID ).ToDTO().ToList();
            }
        }

        #endregion

		#endregion
    }

}
