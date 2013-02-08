using System;
using CHAOS;
using CHAOS.Serialization;
using Chaos.Portal.Data.Dto.Standard;

namespace Chaos.Mcm.Data.Dto.Standard
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
