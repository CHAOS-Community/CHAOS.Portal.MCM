using System;
using CHAOS.Portal.DTO.Standard;
using CHAOS.Serialization;

namespace CHAOS.MCM.Data.DTO
{
    public class Link : Result
    {
        #region Properties

        [Serialize]
        public uint FolderID { get; set; }

        [Serialize]
        public string ObjectGUID { get; set; }

        [Serialize]
        public uint ObjectFolderTypeID { get; set; }

        [Serialize]
        public DateTime DateCreated { get; set; }

        #endregion
        #region Contructors

        public Link()
        {
            
        }

        public Link( uint folderID, Guid objectGUID, uint objectFolderTypeID, DateTime dateCreated )
        {
            FolderID           = folderID;
            ObjectGUID         = new UUID( objectGUID.ToByteArray() ).ToString();
            ObjectFolderTypeID = objectFolderTypeID;
            DateCreated        = dateCreated;
        }

        #endregion
    }
}
