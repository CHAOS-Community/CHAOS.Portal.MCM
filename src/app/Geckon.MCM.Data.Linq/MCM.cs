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
}
