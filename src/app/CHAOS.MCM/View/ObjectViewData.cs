﻿using System;
using System.Collections.Generic;
using System.Globalization;
using CHAOS.Serialization;
using Chaos.Mcm.Data.Dto;
using Chaos.Mcm.Permission;
using Chaos.Portal.Core.Data.Model;
using Chaos.Portal.Core.Indexing.View;
using Object = Chaos.Mcm.Data.Dto.Object;

namespace Chaos.Mcm.View
{
    using Newtonsoft.Json;

    public class ObjectViewData : AResult, IViewData
    {
        private IPermissionManager _permissionManager;

        #region Initialization

        public ObjectViewData(Object obj)
        {
            Object = obj;
        }

        public ObjectViewData(Object obj, IPermissionManager permissionManager)
        {
            Object            = obj;
            PermissionManager = permissionManager;
        }

        public ObjectViewData()
        {
            Object = new Object();
        }

        #endregion
        #region Properties

        public Object Object { get; set; }

        [JsonIgnore]
        protected IPermissionManager PermissionManager
        {
            get
            {
                if (_permissionManager == null)
                    throw new NullReferenceException("PermissionManager is not initialized in ObjectViewData");

                return _permissionManager;
            }
            set { _permissionManager = value; }
        }

        public KeyValuePair<string, string> UniqueIdentifier { get { return new KeyValuePair<string, string>("Id", Object.Guid.ToString()); } }

        [JsonIgnore]
        [Serialize("Id")]
        public string Id
        {
            get
            {
                return Object.Guid.ToString();
            }
            set
            {
                Object.Guid = Guid.Parse(value);
            }
        }

        [JsonIgnore]
        [Serialize]
        public uint ObjectTypeId
        {
            get
            {
                return Object.ObjectTypeID;
            }
            set
            {
                Object.ObjectTypeID = value;
            }
        }

        [JsonIgnore]
        [Serialize]
        public DateTime DateCreated
        {
            get
            {
                return Object.DateCreated;
            }
            set
            {
                Object.DateCreated = value;
            }
        }

        [JsonIgnore]
        [Serialize]
        public IList<Metadata> Metadatas
        {
            get { return Object.Metadatas; }
        }

        [JsonIgnore]
        [Serialize]
        public IList<ObjectFolder> ObjectFolders { get { return Object.ObjectFolders; } }

        [JsonIgnore]
        [Serialize]
        public IList<ObjectRelationInfo> ObjectRelationInfos { get { return Object.ObjectRelationInfos; } }

        [JsonIgnore]
        [Serialize]
        public IList<FileInfo> Files { get { return Object.Files; } }

        [JsonIgnore]
        [Serialize]
        public IList<ObjectAccessPoint> AccessPoints { get { return Object.AccessPoints; } }

        #endregion
        #region Business Logic

        public virtual IEnumerable<KeyValuePair<string, string>> GetIndexableFields()
        {
            yield return UniqueIdentifier;

            yield return new KeyValuePair<string, string>("ObjectTypeId", ObjectTypeId.ToString(CultureInfo.InvariantCulture));

            if (AccessPoints != null)
                foreach (var ap in AccessPoints)
                {
                    var start = ap.StartDate.HasValue ? ap.StartDate.Value : DateTime.MaxValue;
                    var end = ap.EndDate.HasValue ? ap.EndDate.Value : DateTime.MaxValue;

                    yield return new KeyValuePair<string, string>(string.Format("{0}_PubStart", ap.AccessPointGuid), start.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'"));
                    yield return new KeyValuePair<string, string>(string.Format("{0}_PubEnd", ap.AccessPointGuid), end.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'"));
                }

             if (ObjectFolders != null)
                foreach (var objectFolder in ObjectFolders)
                {
                    yield return new KeyValuePair<string, string>("FolderId", objectFolder.ID.ToString(CultureInfo.InvariantCulture));

                    foreach (var folder in PermissionManager.GetFolders(objectFolder.ID).GetAncestorFolders())
                    {
                        yield return new KeyValuePair<string, string>("FolderAncestors", folder.ID.ToString(CultureInfo.InvariantCulture));
                    }
                }

            yield return new KeyValuePair<string, string>("Files.Count", Files.Count.ToString(CultureInfo.InvariantCulture));
            yield return new KeyValuePair<string, string>("ObjectRelations.Count", ObjectRelationInfos.Count.ToString(CultureInfo.InvariantCulture));
            yield return new KeyValuePair<string, string>("DateCreated", DateCreated.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'"));
        }
        
        #endregion

        #region Implementation of IResult
        
        #endregion
    }
}