using System;
using System.Collections.Generic;
using CHAOS.MCM.Data.Dto.Standard;

namespace CHAOS.MCM.Data.Dto
{
    public interface IObject
    {
        UUID GUID { get; set; }
        uint ObjectTypeID { get; set; }
        DateTime DateCreated { get; set; }
        IEnumerable<Metadata> Metadatas { get; set; }
        IEnumerable<FileInfo> Files { get; set; }
        List<Object_Object_Join> ObjectRealtions { get; set; }
        IList<Link> Folders { get; set; }
        IList<uint> FolderTree { get; set; }
        List<IObject> RelatedObjects { get; set; }
        IList<AccessPoint_Object_Join> AccessPoints { get; set; }

        KeyValuePair<string, string> UniqueIdentifier { get; }
    }
}