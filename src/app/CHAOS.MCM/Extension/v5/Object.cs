using Chaos.Mcm.Extension.v6;

namespace Chaos.Mcm.Extension.v5
{
    using System;

    using CHAOS;

    using Chaos.Mcm.Data;
    using Chaos.Mcm.Extension.Domain;
    using Chaos.Mcm.Permission;
    using Chaos.Portal.Core;
    using Chaos.Portal.Core.Data.Model;
    using Chaos.Portal.Core.Indexing;

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
            
            var pagedResult = ViewManager.GetObjects(query, accesspointGuid, GetFoldersWithAccess(), includeAccessPoints, includeMetadata, includeFiles, includeObjectRelations);

            // todo map to v5 object


            // todo filter based on include parameters

            return pagedResult;
        }

        #endregion
    }
}