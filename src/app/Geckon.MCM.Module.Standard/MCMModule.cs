using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Geckon.MCM.Data.Linq;
using Geckon.Portal.Core;
using Geckon.Portal.Core.Standard.Extension;
using Geckon.Portal.Core.Standard.Module;
using Geckon.Portal.Data.Dto;

namespace Geckon.MCM.Module.Standard
{
    public class MCMModule : AModule
    {
        #region Properties

        private String ConnectionString { get; set; }
        public Data.Linq.MCMDataContext DefaultMCMDataContext { get { return new Data.Linq.MCMDataContext(ConnectionString); } }

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
        public ScalarResult ObjectType_Update( CallContext callContext, int id, string newName )
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
        public IEnumerable<Language> Language_Get( CallContext callContext, int? id, string name, string languageCode, string countryName )
        {
            using( MCMDataContext db = DefaultMCMDataContext )
            {
                return db.Language_Get( id, name, languageCode, countryName ).ToList();
            }
        }

        [Datatype("Language", "Create")]
        public Language Language_Create( CallContext callContext, string name, string languageCode, string countryName )
        {
            using( MCMDataContext db = DefaultMCMDataContext )
            {
                int result = db.Language_Create( name, languageCode, countryName, callContext.User.SystemPermission );

                if( result == -100 )
                    throw new Portal.Core.Exception.InsufficientPermissionsExcention( "User does not have permission to delete an Object Type" );

                return db.Language_Get( result, null, null, null ).First();
            }
        }

        [Datatype("Language", "Update")]
        public ScalarResult Language_Update(CallContext callContext, int? id, string name, string languageCode, string countryName)
        {
            using( MCMDataContext db = DefaultMCMDataContext )
            {
                int result = db.Language_Update( id, name, languageCode, countryName, callContext.User.SystemPermission );

                if( result == -100 )
                    throw new Portal.Core.Exception.InsufficientPermissionsExcention( "User does not have permission to delete an Object Type" );

                return new ScalarResult( result );
            }
        }

        [Datatype("Language", "Delete")]
        public ScalarResult Language_Delete(CallContext callContext, int? id )
        {
            using( MCMDataContext db = DefaultMCMDataContext )
            {
                int result = db.Language_Delete( id, callContext.User.SystemPermission );

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
        public ScalarResult ObjectRelationType_Delete(CallContext callContext, int? id)
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



        #endregion
        #region Format

        //[Datatype("Format","Get")]
        //public IEnumerable<Format> Format_Get( CallContext callContext,  )

        #endregion

        #endregion
    }
}
