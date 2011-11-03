using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Geckon.MCM.Core.Exception;
using Geckon.MCM.Data.Linq;
using Geckon.Portal.Core.Exception;
using Geckon.Portal.Core.Standard.Extension;
using Geckon.Portal.Core.Standard.Module;
using Geckon.Portal.Data;
using Object = Geckon.MCM.Data.Linq.Object;
using Geckon.Portal.Core.Standard;
using Geckon.Index;

namespace Geckon.MCM.Module.Standard
{
    public class MCMModule : AModule
    {
        #region Properties

        private String ConnectionString { get; set; }
        public Data.Linq.MCMDataContext DefaultMCMDataContext { get { return new MCMDataContext(ConnectionString); } }

        #endregion
        #region Construction

        public override void Init( XElement config )
        {
            ConnectionString = config.Attribute( "ConnectionString" ).Value;
        }

        #endregion
        #region Business Logic

        #region ObjectType

        [Datatype("ObjectType","Create")]
        public ObjectType ObjectType_Create( CallContext callContext, string value  )
        {
            using( MCMDataContext db = DefaultMCMDataContext )
            {
                int result = db.ObjectType_Insert( value, callContext.User.SystemPermission ); 

                if( result == -100 )
                    throw new Portal.Core.Exception.InsufficientPermissionsExcention( "User does not have permission to create an Object Type" );

                return db.ObjectType_Get( result, null ).First();
            }
        }

        [Datatype("ObjectType", "Get")]
        public IEnumerable<ObjectType> ObjectType_Get( CallContext callContext )
        {
            using( MCMDataContext db = DefaultMCMDataContext )
            {
                IEnumerable<ObjectType> results = db.ObjectType_Get( null, null );

                return results.ToList();
            }
        }

        [Datatype("ObjectType","Update")]
        public ScalarResult ObjectType_Update(  CallContext callContext, int id, string newName )
        {
            using( MCMDataContext db = DefaultMCMDataContext )
            {
                int result = db.ObjectType_Update(id, newName, callContext.User.SystemPermission);

                if( result == -100 )
                    throw new Portal.Core.Exception.InsufficientPermissionsExcention( "User does not have permission to update an Object Type" );

                return new ScalarResult( result );
            }
        }

        [Datatype("ObjectType","Delete")]
        public ScalarResult ObjectType_Delete( CallContext callContext, int id )
        {
            using( MCMDataContext db = DefaultMCMDataContext )
            {
                int result = db.ObjectType_Delete( id, null, callContext.User.SystemPermission );

                if( result == -100 )
                    throw new Portal.Core.Exception.InsufficientPermissionsExcention( "User does not have permission to delete an Object Type" );

                return new ScalarResult( result );
            }
        }

        #endregion
        #region Language

        [Datatype("Language","Get")]
        public IEnumerable<Language> Language_Get( CallContext callContext, string name, string languageCode )
        {
            using( MCMDataContext db = DefaultMCMDataContext )
            {
                return db.Language_Get( name, languageCode ).ToList();
            }
        }

        [Datatype("Language", "Create")]
        public Language Language_Create( CallContext callContext, string name, string languageCode )
        {
            using( MCMDataContext db = DefaultMCMDataContext )
            {
                int result = db.Language_Create( name, languageCode, callContext.User.SystemPermission );

                if( result == -100 )
                    throw new Portal.Core.Exception.InsufficientPermissionsExcention( "User does not have permission to delete an Object Type" );

                return db.Language_Get( name, languageCode ).First();
            }
        }

        [Datatype("Language", "Update")]
        public ScalarResult Language_Update( CallContext callContext, string languageCode, string newName )
        {
            using( MCMDataContext db = DefaultMCMDataContext )
            {
                int result = db.Language_Update( newName, languageCode, callContext.User.SystemPermission );

                if( result == -100 )
                    throw new Portal.Core.Exception.InsufficientPermissionsExcention( "User does not have permission to delete an Object Type" );

                return new ScalarResult( result );
            }
        }

        [Datatype("Language", "Delete")]
        public ScalarResult Language_Delete( CallContext callContext, string languageCode )
        {
            using( MCMDataContext db = DefaultMCMDataContext )
            {
                int result = db.Language_Delete( languageCode, callContext.User.SystemPermission );

                if( result == -100 )
                    throw new Portal.Core.Exception.InsufficientPermissionsExcention( "User does not have permission to delete an Object Type" );

                return new ScalarResult( result );
            }
        }

        #endregion
        #region ObjectRelationType

        [Datatype("ObjectRelationType", "Get")]
        public IEnumerable<ObjectRelationType> ObjectRelationType_Get(CallContext callContext, int? id, string value)
        {
            using( MCMDataContext db = DefaultMCMDataContext )
            {
                return db.ObjectRelationType_Get(id, value).ToList();
            }
        }

        [Datatype("ObjectRelationType", "Create")]
        public ObjectRelationType ObjectRelationType_Create(CallContext callContext, string value)
        {
            using( MCMDataContext db = DefaultMCMDataContext )
            {
                int result = db.ObjectRelationType_Create(value, callContext.User.SystemPermission);

                if( result == -100 )
                    throw new Portal.Core.Exception.InsufficientPermissionsExcention( "User does not have permission to delete an Object Type" );

                return db.ObjectRelationType_Get(result, null).First();
            }
        }

        [Datatype("ObjectRelationType", "Update")]
        public ScalarResult ObjectRelationType_Update(CallContext callContext, int? id, string value)
        {
            using( MCMDataContext db = DefaultMCMDataContext )
            {
                int result = db.ObjectRelationType_Update(id, value, callContext.User.SystemPermission);

                if( result == -100 )
                    throw new Portal.Core.Exception.InsufficientPermissionsExcention( "User does not have permission to delete an Object Type" );

                return new ScalarResult( result );
            }
        }

        [Datatype("ObjectRelationType", "Delete")]
        public ScalarResult ObjectRelationType_Delete( CallContext callContext, int? id )
        {
            using( MCMDataContext db = DefaultMCMDataContext )
            {
                int result = db.ObjectRelationType_Delete(id, callContext.User.SystemPermission);

                if( result == -100 )
                    throw new Portal.Core.Exception.InsufficientPermissionsExcention( "User does not have permission to delete an Object Type" );

                return new ScalarResult( result );
            }
        }

        #endregion
        #region FolderType

        [Datatype("FolderType", "Get")]
        public IEnumerable<FolderType> FolderType_Get(CallContext callContext, int? id, string name)
        {
            using (MCMDataContext db = DefaultMCMDataContext)
            {
                return db.FolderType_Get(id, name).ToList();
            }
        }

        [Datatype("FolderType", "Create")]
        public FolderType FolderType_Create(CallContext callContext, string name)
        {
            using (MCMDataContext db = DefaultMCMDataContext)
            {
                int result = db.FolderType_Create(name, callContext.User.SystemPermission);

                if (result == -100)
                    throw new Portal.Core.Exception.InsufficientPermissionsExcention("User does not have permission to delete an Object Type");

                return db.FolderType_Get(result, null).First();
            }
        }

        [Datatype("FolderType", "Update")]
        public ScalarResult FolderType_Update(CallContext callContext, int? id, string name)
        {
            using (MCMDataContext db = DefaultMCMDataContext)
            {
                int result = db.FolderType_Update(id, name, callContext.User.SystemPermission);

                if (result == -100)
                    throw new Portal.Core.Exception.InsufficientPermissionsExcention("User does not have permission to delete an Object Type");

                return new ScalarResult(result);
            }
        }

        [Datatype("FolderType", "Delete")]
        public ScalarResult FolderType_Delete(CallContext callContext, int? id)
        {
            using (MCMDataContext db = DefaultMCMDataContext)
            {
                int result = db.FolderType_Delete(id, callContext.User.SystemPermission);

                if (result == -100)
                    throw new Portal.Core.Exception.InsufficientPermissionsExcention("User does not have permission to delete an Object Type");

                return new ScalarResult(result);
            }
        }

        #endregion
        #region FormatType

        [Datatype("FormatType", "Get")]
        public IEnumerable<FormatType> FormatType_Get(CallContext callContext, int? id, string name)
        {
            using( MCMDataContext db = DefaultMCMDataContext )
            {
                return db.FormatType_Get(id, name).ToList();
            }
        }

        [Datatype("FormatType", "Create")]
        public FormatType FormatType_Create(CallContext callContext, string name)
        {
            using( MCMDataContext db = DefaultMCMDataContext )
            {
                int result = db.FormatType_Create(name, callContext.User.SystemPermission);

                if( result == -100 )
                    throw new Portal.Core.Exception.InsufficientPermissionsExcention("User does not have permission to delete an Object Type");

                return db.FormatType_Get(result, null).First();
            }
        }

        [Datatype("FormatType", "Update")]
        public ScalarResult FormatType_Update(CallContext callContext, int? id, string name)
        {
            using( MCMDataContext db = DefaultMCMDataContext )
            {
                int result = db.FormatType_Update( id, name, callContext.User.SystemPermission );

                if( result == -100 )
                    throw new Portal.Core.Exception.InsufficientPermissionsExcention("User does not have permission to delete an Object Type");

                return new ScalarResult(result);
            }
        }

        [Datatype("FormatType", "Delete")]
        public ScalarResult FormatType_Delete(CallContext callContext, int? id)
        {
            using( MCMDataContext db = DefaultMCMDataContext )
            {
                int result = db.FormatType_Delete(id, callContext.User.SystemPermission);

                if( result == -100 )
                    throw new Portal.Core.Exception.InsufficientPermissionsExcention("User does not have permission to delete an Object Type");

                return new ScalarResult(result);
            }
        }

        #endregion
        #region FormatCategory

        //[Datatype("FormatCategory","Get")]

        #endregion
        #region Format

        //[Datatype("Format","Get")]
        //public IEnumerable<Format> Format_Get( CallContext callContext,  )

        #endregion
        #region Folder

        [Datatype("Folder","Get")]
        public IEnumerable<FolderInfo> Folder_Get( CallContext callContext, int? id, int? folderTypeID, int? parentID )
        {
            using( MCMDataContext db = DefaultMCMDataContext )
            {
                return db.Folder_Get( callContext.Groups.Select( group => group.GUID ).ToList(), callContext.User.GUID, id, folderTypeID, parentID );
            }
        }

        [Datatype("Folder","Delete")]
        public ScalarResult Folder_Delete( CallContext callContext, int id )
        {
            using( MCMDataContext db = DefaultMCMDataContext )
            {
                int result = db.Folder_Delete( callContext.Groups.Select(group => group.GUID).ToList(), callContext.User.GUID, id );

                if( result == -50 )
                    throw new FolderNotEmptyException( "You cannot delete non empty folder" );
 
                if( result == -100 )
                    throw new Portal.Core.Exception.InsufficientPermissionsExcention( "User does not have permission to delete the folder" );

                return new ScalarResult( result );
            }
        }

        [Datatype("Folder","Update")]
        public ScalarResult Folder_Update( CallContext callContext, int id, string newTitle, int? newParentID, int? newFolderTypeID )
        {
            using( MCMDataContext db = DefaultMCMDataContext )
            {
                int result = db.Folder_Update( callContext.Groups.Select(group => group.GUID).ToList(), callContext.User.GUID, id, newTitle, newParentID, newFolderTypeID );

                if( result == -10 )
                    throw new Portal.Core.Exception.InvalidProtocolException( "The parameters to update cant all be null" );

                if( result == -100 )
                    throw new Portal.Core.Exception.InsufficientPermissionsExcention( "User does not have permission to update the folder" );

                return new ScalarResult( result );
            }
        }

        [Datatype("Folder", "Create")]
        public FolderInfo Folder_Create( CallContext callContext, string subscriptionGUID, string title, int? parentID, int folderTypeID )
        {
            using( MCMDataContext db = DefaultMCMDataContext )
            {
                Guid? subGUID       = null;
                int?  subPermission = 0;

                if( !string.IsNullOrEmpty( subscriptionGUID ) )
                {
                    SubscriptionInfo subscription = callContext.Subscriptions.FirstOrDefault( sub => sub.GUID.CompareTo( Guid.Parse( subscriptionGUID ) ) == 0 );

                    subGUID       = subscription.GUID;
                    subPermission = subscription.Permission;
                }

                int result = db.Folder_Create( callContext.Groups.Select( group => group.GUID ).ToList(), 
                                               callContext.User.GUID,
                                               subGUID,
                                               subPermission,
                                               title, 
                                               parentID, 
                                               folderTypeID);

                if( result == -100 )
                    throw new Portal.Core.Exception.InsufficientPermissionsExcention( "User does not have permission to Create the folder" );

                FolderInfo folder = Folder_Get( callContext, result, null, null ).First();

                return folder;
            }
        }

        #endregion
        #region Object

        [Datatype("Object","Get")]
        public IEnumerable<Object> Object_Get( CallContext callContext, IQuery query, bool includeMetadata, bool includeFiles, int? objectTypeID, int? folderID, int pageIndex, int pageSize )
        {
            IEnumerable<Guid> resultPage = null;

            // TODO: Replace parameters with query
            if( query != null )
            {
                resultPage = callContext.IndexManager.GetIndex<MCMModule>().Get( query ).Results.Select( result => ( (GuidResult) result ).Guid );

                // if solr doesnt return anything there is no need to continue, so just return an empty list
                if( resultPage.Count() == 0 )
                    return new List<Object>();
            }

            using( MCMDataContext db = DefaultMCMDataContext )
            {
                return db.Object_Get( callContext.Groups.Select( group => group.GUID ).ToList(), callContext.User.GUID, resultPage, includeMetadata, includeFiles, null, objectTypeID, folderID, pageIndex, pageSize );
            }
        }

        [Datatype("Object","Create")]
        public Object Object_Create( CallContext callContext, string guid, int objectTypeID, int folderID )
        {
            using( MCMDataContext db = DefaultMCMDataContext )
            {
                int objectID = db.Object_Create( callContext.Groups.Select( group => group.GUID ).ToList(), callContext.User.GUID, Guid.Parse( guid ), objectTypeID, folderID );

                return db.Object_Get( callContext.Groups.Select( group => group.GUID ).ToList(), callContext.User.GUID, null, false, false, objectID, null, null, 0, 1 ).First();
            }
        }

        [Datatype("Object", "Delete")]
        public ScalarResult Object_Delete( CallContext callContext, string guid, int folderID )
        {
            using( MCMDataContext db = DefaultMCMDataContext )
            {
                int result = db.Object_Delete( callContext.Groups.Select( group => group.GUID ).ToList(), callContext.User.GUID, Guid.Parse( guid ), folderID );

                if( result == -100 )
                    throw new InsufficientPermissionsExcention( "User does not have permissions to delete object" );

                return new ScalarResult( result );
            }
        }

        #endregion
        #region Metadata

        [Datatype("Metadata","Set")]
        public ScalarResult Metadata_Set( CallContext callContext, string objectGUID, string metadataSchemaGUID, string languageCode, string metadataXML )
        {
            using( MCMDataContext db = DefaultMCMDataContext )
            {
                int result = db.Metadata_Set( callContext.Groups.Select( group => group.GUID ).ToList(), callContext.User.GUID, Guid.Parse( objectGUID ), Guid.Parse( metadataSchemaGUID ), languageCode, metadataXML, false );
                
                callContext.IndexManager.GetIndex<MCMModule>().Set( db.Object_Get( callContext.Groups.Select( group => group.GUID ).ToList(), callContext.User.GUID, new []{ Guid.Parse( objectGUID ) }, true, false, null, null, null, 0, 1 ).First() );

                return new ScalarResult( result );
            }
        }

        [Datatype("Metadata", "Get")]
        public IEnumerable<Metadata> Metadata_Get( CallContext callContext, string objectGUID, string metadataSchemaGUID, string languageCode )
        {
            using( MCMDataContext db = DefaultMCMDataContext )
            {
                return db.Metadata_Get( Guid.Parse( objectGUID ), metadataSchemaGUID == null ? (Guid?) null : Guid.Parse( metadataSchemaGUID ), languageCode ).ToList();
            }
        }

        #endregion
        #region Test

        [Datatype("Test","ReIndex")]
        public ScalarResult Test_ReIndex( CallContext callContext, int folderID )
        {
            Geckon.Index.Solr.Solr<GuidResult> index = ( Geckon.Index.Solr.Solr<GuidResult> )callContext.IndexManager.GetIndex<MCMModule>();

            index.RemoveAll(false); 
            //index.Set( Object_Get( callContext, new Geckon.Index.Solr.SolrQuery("*:*",null), true, false, null, null, 0, int.MaxValue ).Select( obj => (IIndexable) obj ) );

            foreach( Object obj in Object_Get( callContext, null, true, false, null, folderID, 0, int.MaxValue ) )
            {
                index.Set( obj, false );
            }

            index.Commit();

            return new ScalarResult(1);
        }

        #endregion
        #endregion
    }
}
