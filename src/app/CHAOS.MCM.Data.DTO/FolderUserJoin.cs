using System;

namespace CHAOS.MCM.Data.DTO
{
    public class FolderUserJoin
    {
        #region Properties

        public uint FolderID { get; set; }
        public Guid UserGuid { get; set; }
        public uint Permission { get; set; }
        public DateTime DateCreated { get; set; }

        #endregion

    }
}
