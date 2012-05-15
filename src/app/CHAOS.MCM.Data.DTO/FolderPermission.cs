using CHAOS.Portal.DTO.Standard;
using CHAOS.Serialization;

namespace CHAOS.MCM.Data.DTO
{
    public class FolderPermission : Result
    {
        [Serialize]
        public uint AccumulatedPermission { get; set; }
    }
}
