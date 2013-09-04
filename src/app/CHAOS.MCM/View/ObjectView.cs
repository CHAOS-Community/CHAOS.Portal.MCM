namespace Chaos.Mcm.View
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using CHAOS.Extensions;
    using CHAOS.Serialization;

    using Chaos.Mcm.Data.Dto;
    using Chaos.Portal.Core.Data.Model;
    using Chaos.Portal.Core.Indexing.View;

    using Object = Chaos.Mcm.Data.Dto.Object;

    public class ObjectView : AView
    {
        public ObjectView() : base("Object")
        {
        }

        #region Overrides of AView

        public override IList<IViewData> Index(object objectsToIndex)
        {
            var obj = objectsToIndex as Object;

            if (obj == null) return new List<IViewData>();

            return new[] { new ObjectViewData(obj) };

            
        }

        public override IPagedResult<IResult> Query(Portal.Core.Indexing.IQuery query)
        {
            var result = Core.Query(query);

            var foundCount = result.QueryResult.FoundCount;
            var startIndex = result.QueryResult.StartIndex;
            var keys       = result.QueryResult.Results.Select(item => item.Id);
            var results    = Cache.Get<ObjectViewData>(keys);

            return new PagedResult<IResult>(foundCount, startIndex, results);
        }

        #endregion
    }

    [Serialize("Object")]
    public class ObjectViewData : AResult, IViewData
    {
        #region Initialization

        public ObjectViewData(Object obj)
        {
            Object = obj;
        }

        #endregion
        #region Properties

        protected Object Object { get; set; }

        public KeyValuePair<string, string> UniqueIdentifier { get { return new KeyValuePair<string, string>("Id", Object.Guid.ToString()); } }

        [Serialize]
        public string Id { get { return Object.Guid.ToString(); } }

        [Serialize]
        public uint ObjectTypeId { get { return Object.ObjectTypeID; } }

        [Serialize]
        public DateTime DateCreated { get { return Object.DateCreated; } }

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

        public IEnumerable<KeyValuePair<string, string>> GetIndexableFields()
        {
            yield return UniqueIdentifier;
        
            if (AccessPoints != null)
                foreach (var ap in AccessPoints)
                {
                    var start = ap.StartDate.HasValue ? ap.StartDate.Value : DateTime.MaxValue;
                    var end = ap.EndDate.HasValue ? ap.EndDate.Value : DateTime.MaxValue;
            
                    yield return new KeyValuePair<string, string>(string.Format("{0}_PubStart", ap.AccessPointGuid), start.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'"));
                    yield return new KeyValuePair<string, string>(string.Format("{0}_PubEnd", ap.AccessPointGuid), end.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'"));
                }
        }
        
        #endregion

        #region Implementation of IResult
        
        #endregion
    }
}