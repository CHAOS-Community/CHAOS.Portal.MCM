using Chaos.Mcm.Extension.v6;

namespace Chaos.Mcm.Extension.v5
{
    using System;
    using System.Linq;
    using CHAOS;

    using Data;
    using Domain;
    using Permission;
    using Portal.Core;
    using Portal.Core.Data.Model;
    using Portal.Core.Indexing;

    public class Object : AMcmExtension
    {
        #region Initialization

        public Object(IPortalApplication portalApplication, IMcmRepository mcmRepository, IPermissionManager permissionManager): base(portalApplication, mcmRepository, permissionManager)
        {
        }

        #endregion
        #region Properties


        #endregion
        #region Business Logic

        public IPagedResult<IResult> Get(IQuery query, UUID accessPointGUID, bool includeMetadata = false, bool includeFiles = false, bool includeObjectRelations = false, bool includeAccessPoints = false)
        {
            var accesspointGuid = accessPointGUID != null ? CHAOS.Extensions.GuidExtensions.ToGuid(accessPointGUID) : (Guid?)null;

            query.Query = query.Query.Replace("GUID:", "Id:");
            query.Query = query.Query.Replace("ObjectTypeID:", "ObjectTypeId:");
            
            var result = ViewManager.GetObjects(query, accesspointGuid, GetFoldersWithAccess(), includeAccessPoints, includeMetadata, includeFiles, includeObjectRelations);

            var page = new PagedResult<IResult>(result.FoundCount,
                                                result.StartIndex,
                                                result.Results.Select(item => Data.Dto.v5.Object.Create((Data.Dto.Object) item)));

            // todo filter based on include parameters

            return page;
        }

        #endregion
    }
}