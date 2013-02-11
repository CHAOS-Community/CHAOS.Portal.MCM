namespace Chaos.Mcm.Data.Dto
{
    using System;
    using System.Collections.Generic;

    using CHAOS.Serialization;

    using Chaos.Mcm.Data.Dto.Standard;
    using Chaos.Portal.Data.Dto.Standard;
    
    public class Object : Result
    {
        #region Properties

        [Serialize]
        public Guid Guid { get; set; }

        [Serialize]
        public uint ObjectTypeID { get; set; }

        [Serialize]
        public DateTime DateCreated { get; set; }

        [Serialize]
        public IList<Metadata> Metadatas { get; set; }

        [Serialize]
        public IList<ObjectRelationInfo> ObjectRelationInfos { get; set; }

        [Serialize]
        public IList<FileInfo> Files { get; set; }

        [Serialize]
        public IList<AccessPoint_Object_Join> AccessPoints { get; set; }

        [Serialize]
        public IList<ObjectFolder> ObjectFolders { get; set; }

        #endregion
        #region Initialization

        public Object()
        {
            Metadatas = new List<Metadata>();
        }

        #endregion
    }
}