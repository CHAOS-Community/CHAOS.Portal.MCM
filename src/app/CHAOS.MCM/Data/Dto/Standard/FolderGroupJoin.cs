using System;

namespace CHAOS.MCM.Data.Dto.Standard
{
    public class FolderGroupJoin : IFolderGroupJoin
    {
        #region Properties

        public uint FolderID { get; set; }
        public Guid GroupGuid { get; set; }
        public uint Permission { get; set; }
        public DateTime DateCreated { get; set; }

        #endregion
    }
}