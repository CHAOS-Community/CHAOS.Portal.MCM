namespace Chaos.Mcm.Data.Dto
{
    using System;
    using System.Collections.Generic;

    using CHAOS.Serialization;

    using Portal.Core.Data.Model;
    
    public class Object : AResult
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
        public IList<ObjectRelationInfo> ObjectRealtionInfos { get; set; }

        [Serialize]
        public IList<FileInfo> Files { get; set; }

        [Serialize]
        public IList<ObjectAccessPoint> AccessPoints { get; set; }

        [Serialize]
        public IList<ObjectFolder> ObjectFolders { get; set; }

        #endregion
        #region Initialization

        public Object()
        {
            Metadatas           = new List<Metadata>();
            ObjectRealtionInfos = new List<ObjectRelationInfo>();
            Files               = new List<FileInfo>();
            AccessPoints        = new List<ObjectAccessPoint>();
            ObjectFolders       = new List<ObjectFolder>();
        }

        #endregion
    }
}