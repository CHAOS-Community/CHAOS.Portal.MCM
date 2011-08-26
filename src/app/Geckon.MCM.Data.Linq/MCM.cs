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
        #region Folder_Get

        public IList<Folder> Folder_Get(IList<Guid> groupGUIDs, Guid userGUID )
        {
            DataTable groupGUIDsTable = new DataTable();
            groupGUIDsTable.Columns.Add( "GUID", typeof( Guid ) );

            foreach( Guid guid in groupGUIDs )
                groupGUIDsTable.Rows.Add( guid );

            using( SqlConnection conn = new SqlConnection( Connection.ConnectionString ) )
            {
                SqlCommand cmd = new SqlCommand( "Folder_Get", conn );
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter p = cmd.Parameters.AddWithValue( "@GroupGUIDs", groupGUIDsTable );
                p.SqlDbType = SqlDbType.Structured;
                p.TypeName  = "GUIDList";

                p = cmd.Parameters.AddWithValue( "@UserGUID", userGUID );
                p.SqlDbType = SqlDbType.UniqueIdentifier;

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                return Translate<Folder>(reader).ToList();
            }
        }


        #endregion

    }

    [Document("Geckon.MCM.Data.Linq.DTO.Folder")]
    public partial class Folder : XmlSerialize
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
