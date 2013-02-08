namespace Chaos.Mcm.Data.Dto
{
    using System;
    using System.Collections.Generic;

    using Chaos.Mcm.Data.Dto.Standard;

    public class NewObject
    {
        #region Properties

        public Guid Guid { get; set; }

        public IList<NewMetadata> Metadatas { get; set; }

        public IList<FileInfo> Files { get; set; }
        
        public uint ObjectTypeID { get; set; }

        public DateTime DateCreated { get; set; }

        public IList<ObjectRelationInfo> ObjectRelationInfos { get; set; }

        public IList<ObjectFolder> ObjectFolders { get; set; }

        #endregion
        #region Initialization

        public NewObject()
        {
            Metadatas = new List<NewMetadata>();
        }

        #endregion
    }
}