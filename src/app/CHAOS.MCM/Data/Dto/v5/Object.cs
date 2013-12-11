﻿namespace Chaos.Mcm.Data.Dto.v5
{
    using System;
    using System.Collections.Generic;
    using CHAOS;
    using CHAOS.Serialization;
    using Portal.Core.Data.Model;

    public class Object : IResult
    {
        [Serialize("GUID")]
        public UUID Guid { get; set; }

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
        public IList<ObjectAccessPoint> AccessPoints { get; set; }

        [Serialize]
        public IList<ObjectFolder> ObjectFolders { get; set; }

        public string Fullname { get; private set; }
        
        public static Object Create(Dto.Object obj)
        {
            return new Object();
        }

        public Object()
        {
            Fullname = "CHAOS.MCM.Data.Dto.Standard.Object";
            Metadatas = new List<Metadata>();
            ObjectRelationInfos = new List<ObjectRelationInfo>();
            Files = new List<FileInfo>();
            AccessPoints = new List<ObjectAccessPoint>();
            ObjectFolders = new List<ObjectFolder>();
        }
    }
}
