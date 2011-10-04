using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Geckon.Serialization;
using Geckon.Portal.Data.Result.Standard;

namespace Geckon.MCM.Data.Linq
{
    public partial class MCMDataContext
    {
        #region Folder

        public IEnumerable<FolderInfo> Folder_Get( List<Guid> groupGUIDs, Guid userGUID, int? id, int? folderTypeID, int? parentID )
        {
            DataTable groupGUIDsTable = new DataTable();
            groupGUIDsTable.Columns.Add( "GUID", typeof( Guid ) );

            foreach( Guid guid in groupGUIDs )
                groupGUIDsTable.Rows.Add( guid );

            using( SqlConnection conn = new SqlConnection( Connection.ConnectionString ) )
            {
                SqlCommand cmd = new SqlCommand( "FolderInfo_Get", conn );
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter p = cmd.Parameters.AddWithValue( "@GroupGUIDs", groupGUIDsTable );
                p.SqlDbType = SqlDbType.Structured;
                p.TypeName  = "GUIDList";

                p = cmd.Parameters.AddWithValue( "@UserGUID", userGUID );
                p.SqlDbType = SqlDbType.UniqueIdentifier;

                p = cmd.Parameters.AddWithValue( "@FolderID", id );
                p.SqlDbType = SqlDbType.Int;

                p = cmd.Parameters.AddWithValue( "@FolderTypeID", folderTypeID );
                p.SqlDbType = SqlDbType.Int;

                p = cmd.Parameters.AddWithValue( "@ParentID", parentID );
                p.SqlDbType = SqlDbType.Int;

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                return Translate<FolderInfo>(reader).ToList();
            }
        }

        public int Folder_Delete( List<Guid> groupGUIDs, Guid userGUID, int id )
        {
            DataTable groupGUIDsTable = new DataTable();
            groupGUIDsTable.Columns.Add( "GUID", typeof( Guid ) );

            foreach( Guid guid in groupGUIDs )
                groupGUIDsTable.Rows.Add( guid );

            using( SqlConnection conn = new SqlConnection( Connection.ConnectionString ) )
            {
                SqlCommand cmd = new SqlCommand( "Folder_Delete", conn );
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter p = cmd.Parameters.AddWithValue( "@GroupGUIDs", groupGUIDsTable );
                p.SqlDbType = SqlDbType.Structured;
                p.TypeName  = "GUIDList";

                p = cmd.Parameters.AddWithValue( "@UserGUID", userGUID );
                p.SqlDbType = SqlDbType.UniqueIdentifier;

                p = cmd.Parameters.AddWithValue( "@ID", id );
                p.SqlDbType = SqlDbType.Int;
                
                SqlParameter rv = cmd.Parameters.Add( new SqlParameter("@ReturnValue",SqlDbType.Int) );
                rv.Direction = ParameterDirection.ReturnValue; 

                conn.Open();

                cmd.ExecuteNonQuery();

                return (int) rv.Value;
            }
        }

        public int Folder_Update( List<Guid> groupGUIDs, Guid userGUID, int id, string newTitle, int? newParentID, int? newFolderTypeID )
        {
            DataTable groupGUIDsTable = new DataTable();
            groupGUIDsTable.Columns.Add( "GUID", typeof( Guid ) );

            foreach( Guid guid in groupGUIDs )
                groupGUIDsTable.Rows.Add( guid );

            using( SqlConnection conn = new SqlConnection( Connection.ConnectionString ) )
            {
                SqlCommand cmd = new SqlCommand( "Folder_Update", conn );
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter p = cmd.Parameters.AddWithValue( "@GroupGUIDs", groupGUIDsTable );
                p.SqlDbType = SqlDbType.Structured;
                p.TypeName  = "GUIDList";

                p = cmd.Parameters.AddWithValue( "@UserGUID", userGUID );
                p.SqlDbType = SqlDbType.UniqueIdentifier;

                p = cmd.Parameters.AddWithValue( "@ID", id );
                p.SqlDbType = SqlDbType.Int;

                p = cmd.Parameters.AddWithValue( "@NewTitle", newTitle );
                p.SqlDbType = SqlDbType.VarChar;
                p.Size      = 255;

                p = cmd.Parameters.AddWithValue( "@NewParentID", newParentID );
                p.SqlDbType = SqlDbType.Int;

                p = cmd.Parameters.AddWithValue( "@NewFolderTypeID", newFolderTypeID );
                p.SqlDbType = SqlDbType.Int;
                
                SqlParameter rv = cmd.Parameters.Add( new SqlParameter("@ReturnValue",SqlDbType.Int) );
                rv.Direction = ParameterDirection.ReturnValue; 

                conn.Open();

                cmd.ExecuteNonQuery();

                return (int) rv.Value;
            }
        }

        public int Folder_Create(List<Guid> groupGUIDs, Guid userGUID, Guid? subscriptionGUID, int? subscriptionPermission, string title, int? parentID, int folderTypeID )
        {
            DataTable groupGUIDsTable = new DataTable();
            groupGUIDsTable.Columns.Add( "GUID", typeof( Guid ) );

            foreach( Guid guid in groupGUIDs )
                groupGUIDsTable.Rows.Add( guid );

            using( SqlConnection conn = new SqlConnection( Connection.ConnectionString ) )
            {
                SqlCommand cmd = new SqlCommand( "Folder_Create", conn );
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter p = cmd.Parameters.AddWithValue( "@GroupGUIDs", groupGUIDsTable );
                p.SqlDbType = SqlDbType.Structured;
                p.TypeName  = "GUIDList";

                p = cmd.Parameters.AddWithValue( "@UserGUID", userGUID );
                p.SqlDbType = SqlDbType.UniqueIdentifier;

                p = cmd.Parameters.AddWithValue( "@SubscriptionGUID", subscriptionGUID );
                p.SqlDbType = SqlDbType.UniqueIdentifier;

                p = cmd.Parameters.AddWithValue( "@Title", title );
                p.SqlDbType = SqlDbType.VarChar;
                p.Size      = 255;

                p = cmd.Parameters.AddWithValue( "@ParentID", parentID );
                p.SqlDbType = SqlDbType.Int;

                p = cmd.Parameters.AddWithValue( "@FolderTypeID", folderTypeID );
                p.SqlDbType = SqlDbType.Int;
                
                p = cmd.Parameters.AddWithValue( "@SubscriptionPermission", subscriptionPermission );
                p.SqlDbType = SqlDbType.Int;

                SqlParameter rv = cmd.Parameters.Add( new SqlParameter("@ReturnValue",SqlDbType.Int) );
                rv.Direction = ParameterDirection.ReturnValue; 

                conn.Open();

                cmd.ExecuteNonQuery();

                return (int) rv.Value;
            }
        }

        #endregion        
        #region Object

        public IEnumerable<Object> Object_Get( List<Guid> groupGUIDs, Guid userGUID, List<Guid> GUIDs, int? objectID, int? folderID )
        {
            DataTable groupGUIDsTable = ConvertToDataTable( groupGUIDs );
            DataTable GUIDsTable      = ConvertToDataTable( GUIDs );

            using( SqlConnection conn = new SqlConnection( Connection.ConnectionString ) )
            {
                SqlCommand cmd = new SqlCommand( "Object_Get", conn );
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter p = cmd.Parameters.AddWithValue( "@GroupGUIDs", groupGUIDsTable );
                p.SqlDbType = SqlDbType.Structured;
                p.TypeName  = "GUIDList";

                p = cmd.Parameters.AddWithValue( "@UserGUID", userGUID );
                p.SqlDbType = SqlDbType.UniqueIdentifier;

                p = cmd.Parameters.AddWithValue( "@GUIDs", GUIDsTable );
                p.SqlDbType = SqlDbType.Structured;
                p.TypeName  = "GUIDList";

                p = cmd.Parameters.AddWithValue( "@ObjectID", objectID );
                p.SqlDbType = SqlDbType.Int;

                p = cmd.Parameters.AddWithValue( "@FolderID", folderID );
                p.SqlDbType = SqlDbType.Int;

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                
                return Translate<Object>( reader ).ToList();
            }
        }

        public int Object_Create( List<Guid> groupGUIDs, Guid userGUID, Guid? guid, int objectTypeID, int folderID )
        {
            DataTable groupGUIDsTable = ConvertToDataTable( groupGUIDs );

            using( SqlConnection conn = new SqlConnection( Connection.ConnectionString ) )
            {
                SqlCommand cmd = new SqlCommand( "Object_Create", conn );
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter p = cmd.Parameters.AddWithValue( "@GroupGUIDs", groupGUIDsTable );
                p.SqlDbType = SqlDbType.Structured;
                p.TypeName  = "GUIDList";

                p = cmd.Parameters.AddWithValue( "@UserGUID", userGUID );
                p.SqlDbType = SqlDbType.UniqueIdentifier;

                p = cmd.Parameters.AddWithValue( "@GUID", guid );
                p.SqlDbType = SqlDbType.UniqueIdentifier;

                p = cmd.Parameters.AddWithValue( "@ObjectTypeID", objectTypeID );
                p.SqlDbType = SqlDbType.Int;

                p = cmd.Parameters.AddWithValue( "@FolderID", folderID );
                p.SqlDbType = SqlDbType.Int;

                conn.Open();
                
                SqlParameter rv = cmd.Parameters.Add(new SqlParameter("@ReturnValue", SqlDbType.Int));
                rv.Direction = ParameterDirection.ReturnValue; 
                
                cmd.ExecuteNonQuery();

                return (int) rv.Value;
            }
        }

        public int Object_Delete( List<Guid> groupGUIDs, Guid userGUID, Guid? guid, int folderID )
        {
            DataTable groupGUIDsTable = ConvertToDataTable( groupGUIDs );

            using( SqlConnection conn = new SqlConnection( Connection.ConnectionString ) )
            {
                SqlCommand cmd = new SqlCommand( "Object_Delete", conn );
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter p = cmd.Parameters.AddWithValue( "@GroupGUIDs", groupGUIDsTable );
                p.SqlDbType = SqlDbType.Structured;
                p.TypeName  = "GUIDList";

                p = cmd.Parameters.AddWithValue( "@UserGUID", userGUID );
                p.SqlDbType = SqlDbType.UniqueIdentifier;

                p = cmd.Parameters.AddWithValue( "@GUID", guid );
                p.SqlDbType = SqlDbType.UniqueIdentifier;

                p = cmd.Parameters.AddWithValue( "@FolderID", folderID );
                p.SqlDbType = SqlDbType.Int;

                conn.Open();
                
                SqlParameter rv = cmd.Parameters.Add(new SqlParameter("@ReturnValue", SqlDbType.Int));
                rv.Direction = ParameterDirection.ReturnValue; 
                
                cmd.ExecuteNonQuery();

                return (int) rv.Value;
            }
        }

        private DataTable ConvertToDataTable( List<Guid> guids )
        {
            DataTable groupGUIDsTable = new DataTable();
            groupGUIDsTable.Columns.Add( "GUID", typeof( Guid ) );

            if( guids == null )
                return groupGUIDsTable;

            foreach( Guid guid in guids )
                groupGUIDsTable.Rows.Add( guid );

            return groupGUIDsTable;
        }

        #endregion
        #region Metadata

        public int Metadata_Set( List<Guid> groupGUIDs, Guid userGUID, Guid objectGUID, Guid metadataSchemaGUID, int languageID, string metadataXML, bool lockMetadata )
        {
            DataTable groupGUIDsTable = ConvertToDataTable(groupGUIDs);

            using (SqlConnection conn = new SqlConnection(Connection.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Metadata_Set", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter p = cmd.Parameters.AddWithValue("@GroupGUIDs", groupGUIDsTable);
                p.SqlDbType = SqlDbType.Structured;
                p.TypeName = "GUIDList";

                p = cmd.Parameters.AddWithValue("@UserGUID", userGUID);
                p.SqlDbType = SqlDbType.UniqueIdentifier;

                p = cmd.Parameters.AddWithValue("@ObjectGUID", objectGUID);
                p.SqlDbType = SqlDbType.UniqueIdentifier;

                p = cmd.Parameters.AddWithValue("@MetadataSchemaGUID", metadataSchemaGUID);
                p.SqlDbType = SqlDbType.UniqueIdentifier;

                p = cmd.Parameters.AddWithValue("@LanguageID", languageID);
                p.SqlDbType = SqlDbType.Int;

                p = cmd.Parameters.AddWithValue("@MetadataXML", metadataXML);
                p.SqlDbType = SqlDbType.Xml;

                p = cmd.Parameters.AddWithValue("@Lock", lockMetadata);
                p.SqlDbType = SqlDbType.Bit;

                conn.Open();

                SqlParameter rv = cmd.Parameters.Add(new SqlParameter("@ReturnValue", SqlDbType.Int));
                rv.Direction = ParameterDirection.ReturnValue;

                cmd.ExecuteNonQuery();

                return (int)rv.Value;
            }
        }

        #endregion
    }       

    public partial class FormatType : Result
    {
        #region Properties

        [Serialize("ID")]
        public int pID
        {
            get { return ID; }
            set { ID = value; }
        }

        [Serialize("Value")]
        public string pValue
        {
            get { return Value; }
            set { Value = value; }
        }

        #endregion
    }

    public partial class FolderType : Result
    {
        #region Properties

        [Serialize("ID")]
        public int pID
        {
            get { return ID; }
            set { ID = value; }
        }

        [Serialize("Name")]
        public string pName
        {
            get { return Name; }
            set { Name = value; }
        }

        #endregion
    }

    public partial class ObjectRelationType : Result
    {
        #region Properties

        [Serialize("ID")]
        public int pID
        {
            get { return ID; }
            set { ID = value; }
        }

        [Serialize("Value")]
        public string pValue
        {
            get { return Value; }
            set { Value = value; }
        }

        #endregion
    }

    public partial class FolderInfo : Result
    {
        #region Properties

        [Serialize("ID")]
        public int pID
        {
            get { return ID; }
            set { ID = value; }
        }

        [Serialize("ParentID")]
        public int? pParentID
        {
            get { return ParentID; }
            set { ParentID = value; }
        }

        [Serialize("FolderTypeID")]
        public int pFolderTypeID
        {
            get { return FolderTypeID; }
            set { FolderTypeID = value; }
        }

        [Serialize("SubscriptionGUID")]
        public Guid pSubscriptionGUID
        {
            get { return SubscriptionGUID; }
            set { SubscriptionGUID = value; }
        }

        [Serialize("Title")]
        public string pTitle
        {
            get { return Title; }
            set { Title = value; }
        }

        [Serialize("DateCreated")]
        public DateTime pDateCreated
        {
            get { return DateCreated; }
            set { DateCreated = value; }
        }

        [Serialize("NumberOfSubFolders")]
        public int? pNumberOfSubFolders
        {
            get { return NumberOfSubFolders; }
            set { NumberOfSubFolders = value; }
        }

        [Serialize("NumberOfObjects")]
        public int? pNumberOfObjects
        {
            get { return NumberOfObjects; }
            set { NumberOfObjects = value; }
        }

        #endregion
    }

    public partial class ObjectType : Result
    {
        #region Properties

        [Serialize("ID")]
        public int pID
        {
            get { return ID; }
        }

        [Serialize("Value")]
        public string pValue
        {
            get { return Value; }
        }

        #endregion
    }

    public partial class Language : Result
    {
        #region Properties

        [Serialize("ID")]
        public int pID
        {
            get { return ID; }
        }

        [Serialize("Name")]
        public string pName
        {
            get { return Name; }
        }

        [Serialize("LanguageCode")]
        public string pLanguageCode
        {
            get { return LanguageCode; }
        }

        [Serialize("CountryName")]
        public string pCountryName
        {
            get { return CountryName; }
        }

        #endregion
    }

    public partial class Object : Result
    {
        #region Properties

        [Serialize("GUID")]
        public Guid pGUID
        {
            get { return GUID; }
            set { GUID = value; }
        }

        [Serialize("ObjectTypeID")]
        public int pObjectTypeID
        {
            get { return ObjectTypeID; }
            set { ObjectTypeID = value; }
        }

        [Serialize("DateCreated")]
        public DateTime pDateCreated
        {
            get { return DateCreated; }
            set { DateCreated = value; }
        }

        /// <summary>
        /// This property is used to Serialize Metadata relations
        /// </summary>
        [Serialize("Metadatas")]
        public IList<Metadata> pMetadata{ get; set; }

        /// <summary>
        /// This property is used to Serialize File relations
        /// </summary>
        [Serialize("Files")]
        public IList<File> pFiles { get; set; }

        #endregion
        #region Construction

        public void Include( bool includeMetadata, bool includeFiles )
        {
            if( includeMetadata )  
                pMetadata = Metadatas.ToList();

            if( includeFiles )
                pFiles    = Files.ToList();
        }

        #endregion
    }

    public partial class Metadata : Result
    {
        #region Properties



        #endregion
    }
}
