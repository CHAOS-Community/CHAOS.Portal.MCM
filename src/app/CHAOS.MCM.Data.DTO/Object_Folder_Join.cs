﻿using System;
using Geckon;
using Geckon.Portal.Data.Result.Standard;
using Geckon.Serialization;

namespace CHAOS.MCM.Data.DTO
{
    public class Object_Folder_Join : Result
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

        public Object_Folder_Join()
        {
            
        }

        public Object_Folder_Join( uint folderID, Guid objectGUID, uint objectFolderTypeID, DateTime dateCreated )
        {
            FolderID           = folderID;
            ObjectGUID         = new UUID( objectGUID.ToByteArray() ).ToString();
            ObjectFolderTypeID = objectFolderTypeID;
            DateCreated        = dateCreated;
        }

        #endregion
    }
}
