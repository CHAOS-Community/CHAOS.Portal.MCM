using System;

namespace CHAOS.MCM.Data.Dto
{
    public interface IFolderGroupJoin
    {
        uint FolderID { get; set; }
        Guid GroupGuid { get; set; }
        uint Permission { get; set; }
        DateTime DateCreated { get; set; }
    }
}