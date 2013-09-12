namespace Chaos.Mcm.View
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using CHAOS.Serialization;

    using Chaos.Mcm.Data.Dto;
    using Chaos.Mcm.Permission;
    using Chaos.Portal.Core.Data.Model;
    using Chaos.Portal.Core.Indexing.View;

    using IFolder = Chaos.Mcm.Permission.IFolder;
    using Object = Chaos.Mcm.Data.Dto.Object;

    public class ObjectView : AView
    {
        #region Initialization

        public ObjectView(IPermissionManager permissionManager) : base("Object")
        {
            PermissionManager = permissionManager;
        }

        #endregion

        #region Properties

        public IPermissionManager PermissionManager { get; set; }

        #endregion

        #region Overrides of AView

        public override IList<IViewData> Index(object objectsToIndex)
        {
            var obj = objectsToIndex as Object;

            if (obj == null) return new List<IViewData>();

            return new[] { new ObjectViewData(obj, PermissionManager) };
        }

        public override IPagedResult<IResult> Query(Portal.Core.Indexing.IQuery query)
        {
            var result = Core.Query(query);

            var foundCount = result.QueryResult.FoundCount;
            var startIndex = result.QueryResult.StartIndex;
            var keys       = result.QueryResult.Results.Select(item => CreateKey(item.Id));
            var results    = Cache.Get<ObjectViewData>(keys);

            return new PagedResult<IResult>(foundCount, startIndex, results);
        }

        #endregion
    }

    public class ObjectViewData : AResult, IViewData
    {
        #region Initialization

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

        protected Object Object { get; set; }

        protected IPermissionManager PermissionManager { get; set; }

        public KeyValuePair<string, string> UniqueIdentifier { get { return new KeyValuePair<string, string>("Id", Object.Guid.ToString()); } }

        [Serialize]
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

        [Serialize]
        public IList<Metadata> Metadatas { get { return Object.Metadatas; } }

        [Serialize]
        public IList<ObjectFolder> ObjectFolders { get { return Object.ObjectFolders; } }
        
        [Serialize]
        public IList<ObjectRelationInfo> ObjectRelationInfos { get { return Object.ObjectRelationInfos; } }

        [Serialize]
        public IList<FileInfo> Files { get { return Object.Files; } }

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

            if(ObjectFolders != null)
                foreach(var objectFolder in ObjectFolders)
                {
                    yield return new KeyValuePair<string, string>("FolderId", objectFolder.ID.ToString(CultureInfo.InvariantCulture));

                    foreach (var folder in PermissionManager.GetFolders(objectFolder.ID).GetAncestorFolders())
                    {
                        yield return new KeyValuePair<string, string>("FolderAncestors", folder.ID.ToString(CultureInfo.InvariantCulture));
                    }
                }
        }
        
        #endregion

        #region Implementation of IResult
        
        #endregion
    }
}