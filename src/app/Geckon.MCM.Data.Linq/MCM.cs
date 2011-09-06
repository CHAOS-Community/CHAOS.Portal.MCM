using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Geckon.Serialization.Xml;

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

        #endregion
    }

    [Document("Geckon.MCM.Data.Linq.DTO.FormatType")]
    public partial class FormatType : XmlSerialize
    {
        #region Properties

        [Element("ID")]
        public int pID
        {
            get { return ID; }
            set { ID = value; }
        }

        [Element("Value")]
        public string pValue
        {
            get { return Value; }
            set { Value = value; }
        }

        #endregion
    }

    [Document("Geckon.MCM.Data.Linq.DTO.FolderType")]
    public partial class FolderType : XmlSerialize
    {
        #region Properties

        [Element("ID")]
        public int pID
        {
            get { return ID; }
            set { ID = value; }
        }

        [Element("Name")]
        public string pName
        {
            get { return Name; }
            set { Name = value; }
        }

        #endregion
    }

    [Document("Geckon.MCM.Data.Linq.DTO.ObjectRelationType")]
    public partial class ObjectRelationType : XmlSerialize
    {
        #region Properties

        [Element("ID")]
        public int pID
        {
            get { return ID; }
            set { ID = value; }
        }

        [Element("Value")]
        public string pValue
        {
            get { return Value; }
            set { Value = value; }
        }

        #endregion
    }

    [Document("Geckon.MCM.Data.Linq.DTO.FolderInfo")]
    public partial class FolderInfo : XmlSerialize
    {
        #region Properties

        [Element("ID")]
        public int pID
        {
            get { return ID; }
            set { ID = value; }
        }

        [Element("ParentID")]
        public int? pParentID
        {
            get { return ParentID; }
            set { ParentID = value; }
        }

        [Element("FolderTypeID")]
        public int pFolderTypeID
        {
            get { return FolderTypeID; }
            set { FolderTypeID = value; }
        }

        [Element("SubscriptionGUID")]
        public Guid pSubscriptionGUID
        {
            get { return SubscriptionGUID; }
            set { SubscriptionGUID = value; }
        }

        [Element("Title")]
        public string pTitle
        {
            get { return Title; }
            set { Title = value; }
        }

        [Element("DateCreated")]
        public DateTime pDateCreated
        {
            get { return DateCreated; }
            set { DateCreated = value; }
        }

        [Element("NumberOfSubFolders")]
        public int? pNumberOfSubFolders
        {
            get { return NumberOfSubFolders; }
            set { NumberOfSubFolders = value; }
        }

        #endregion
    }

    [Document("Geckon.MCM.Data.Linq.DTO.ObjectType")]
    public partial class ObjectType : XmlSerialize
    {
        #region Properties

        [Element("ID")]
        public int pID
        {
            get { return ID; }
        }

        [Element("Value")]
        public string pValue
        {
            get { return Value; }
        }

        #endregion
    }

    [Document("Geckon.MCM.Data.Linq.Language")]
    public partial class Language : XmlSerialize
    {
        #region Properties

        [Element("ID")]
        public int pID
        {
            get { return ID; }
        }

        [Element("Name")]
        public string pName
        {
            get { return Name; }
        }

        [Element("LanguageCode")]
        public string pLanguageCode
        {
            get { return LanguageCode; }
        }

        [Element("CountryName")]
        public string pCountryName
        {
            get { return CountryName; }
        }

        #endregion
    }
}
