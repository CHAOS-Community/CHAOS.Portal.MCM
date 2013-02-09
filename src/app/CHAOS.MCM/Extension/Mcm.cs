﻿using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using CHAOS;
using CHAOS.Index.Solr;
using Chaos.Mcm.Data.EF;
using Chaos.Mcm.Permission;
using Chaos.Mcm.Exception;
using Chaos.Portal;
using Chaos.Portal.Data.Dto.Standard;
using Chaos.Portal.Exceptions;
using DestinationInfo = Chaos.Mcm.Data.Dto.Standard.DestinationInfo;
using FolderType = Chaos.Mcm.Data.Dto.Standard.FolderType;
using FormatType = Chaos.Mcm.Data.Dto.Standard.FormatType;
using Language = Chaos.Mcm.Data.Dto.Standard.Language;
using ObjectRelationType = Chaos.Mcm.Data.Dto.Standard.ObjectRelationType;

namespace Chaos.Mcm.Extension
{
    public class Mcm : AMcmExtension
    {
        #region Business Logic
		
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
		#region Test

//		public ScalarResult Test_ReIndex( ICallContext callContext, uint? folderID, bool? clearIndex )
//		{
//            var index = (Solr)callContext.IndexManager.GetIndex<Mcm>();
//
//            if (clearIndex.HasValue && clearIndex.Value)
//                index.RemoveAll(false);
//
//            const uint pageSize = 1000;
//
//            for (uint i = 0; ; i++)
//            {
//                // using ensure the Database Context is disposed once in a while, to avoid OOM exceptions
//                using (var db = DefaultMCMEntities)
//                {
//                    var objects = db.Object_Get(folderID, true, false, true, true, true, i, pageSize).ToDto().ToList();
//
//                    PutObjectInIndex(index, objects);
//
//                    if (objects.Count() != pageSize)
//                        break;
//                }
//            }
//
//            return new ScalarResult(1);
//		}

		#endregion
        #region Link

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

//                PutObjectInIndex( callContext.IndexManager.GetIndex<Mcm>(), db.Object_Get( objectGUID , true, true, true, true, true ).ToDto().ToList() );

                return new ScalarResult( result.Value );
            }
        }

        public ScalarResult Link_Update( ICallContext callContext, UUID objectGUID, uint folderID, uint newFolderID )
        {
            using( MCMEntities db = DefaultMCMEntities )
            {
                if( !HasPermissionToObject( callContext, objectGUID, FolderPermission.CreateLink ) )
                    throw new InsufficientPermissionsException("User does not have permission to update link");

                int result = db.Object_Folder_Join_Update( objectGUID.ToByteArray(), (int) folderID, (int) newFolderID ).First().Value;

//                PutObjectInIndex( callContext.IndexManager.GetIndex<Mcm>(), db.Object_Get( objectGUID , true, true, true, true, true ).ToDto().ToList() );

                return new ScalarResult( result );
            }
        }

        public ScalarResult Link_Delete( ICallContext callContext, UUID objectGUID, uint folderID )
        {
            using( MCMEntities db = DefaultMCMEntities )
            {
                if( !HasPermissionToObject( callContext, objectGUID, FolderPermission.CreateLink ) )
                    throw new InsufficientPermissionsException("User does not have permission to delete link");

                var result = db.Object_Folder_Join_Delete( objectGUID.ToByteArray(), (int) folderID ).FirstOrDefault();

                if(!result.HasValue)
                    throw new UnhandledException("Link delete failed on the database and was rolled back");

//                PutObjectInIndex( callContext.IndexManager.GetIndex<Mcm>(), db.Object_Get( objectGUID , true, true, true, true, true ).ToDto().ToList() );

                return new ScalarResult( result.Value );
            }
        }

        #endregion
        #region Destination

        public IEnumerable<DestinationInfo> Destination_Get(ICallContext callContext, uint destinationID)
        {
            using( MCMEntities db = DefaultMCMEntities )
            {
                return db.DestinationInfo_Get( (int?) destinationID ).ToDto().ToList();
            }
        }

        #endregion

		#endregion
    }

}
