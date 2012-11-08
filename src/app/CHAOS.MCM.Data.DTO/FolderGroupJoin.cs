using System;

namespace CHAOS.MCM.Data.DTO
{
    public class FolderGroupJoin
    {
        #region Properties

        public uint FolderID { get; set; }
        public Guid GroupGuid { get; set; }
        public uint Permission { get; set; }
        public DateTime DateCreated { get; set; }

        #endregion

    }
}