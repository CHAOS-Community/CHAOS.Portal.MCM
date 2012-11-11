using System;

namespace CHAOS.MCM.Data.Dto
{
    public interface IFolderUserJoin
    {
        uint FolderID { get; set; }
        Guid UserGuid { get; set; }
        uint Permission { get; set; }
        DateTime DateCreated { get; set; }
    }
}