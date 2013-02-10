namespace Chaos.Mcm.Data.Dto
{
    using System;
    using System.Collections.Generic;

    using CHAOS.Serialization;

    using Chaos.Mcm.Data.Dto.Standard;
    using Chaos.Portal.Data.Dto.Standard;
    
    public class NewObject : Result
    {
        #region Properties

        [Serialize]
        public Guid Guid { get; set; }

        public IList<NewMetadata> Metadatas { get; set; }

        public IList<FileInfo> Files { get; set; }

        [Serialize]
        public uint ObjectTypeID { get; set; }

        [Serialize]
        public DateTime DateCreated { get; set; }

        public IList<ObjectRelationInfo> ObjectRelationInfos { get; set; }

        public IList<ObjectFolder> ObjectFolders { get; set; }

        public IList<AccessPoint_Object_Join> AccessPoints { get; set; }

        #endregion
        #region Initialization

        public NewObject()
        {
            Metadatas = new List<NewMetadata>();
        }

        #endregion
    }
}