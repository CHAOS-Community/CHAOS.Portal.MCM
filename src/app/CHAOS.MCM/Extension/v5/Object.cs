﻿using Chaos.Mcm.Extension.v6;

namespace Chaos.Mcm.Extension.v5
{
    using System;
    using System.Linq;
    using CHAOS;
    using CHAOS.Extensions;
    using Data;
    using Domain;
    using Domain.Object;
    using Permission;
    using Portal.Core;
    using Portal.Core.Data.Model;
    using Portal.Core.Indexing;
    using View;

    public class Object : AMcmExtension
    {
        #region Initialization

        public Object(IPortalApplication portalApplication, IMcmRepository mcmRepository, IPermissionManager permissionManager)
            : this(portalApplication, mcmRepository, permissionManager, new ObjectCreate(mcmRepository, permissionManager, portalApplication.ViewManager))
        {
        }

        public Object(IPortalApplication portalApplication, IMcmRepository mcmRepository, IPermissionManager permissionManager, IObjectCreate objectCreate) : base(portalApplication, mcmRepository, permissionManager)
        {
            ObjectCreate = objectCreate;
        }

        #endregion
        #region Properties

        public IObjectCreate ObjectCreate { get; set; }

        #endregion
        #region Business Logic

        public IPagedResult<Data.Dto.v5.Object> Get(IQuery query, UUID accessPointGUID, bool includeMetadata = false, bool includeFiles = false, bool includeObjectRelations = false, bool includeAccessPoints = false)
        {
            var accesspointGuid = accessPointGUID != null ? accessPointGUID.ToGuid() : (Guid?)null;

            query.Query = query.Query.Replace("GUID:", "Id:");
            query.Query = query.Query.Replace("ObjectTypeID:", "ObjectTypeId:");
            
            var result = ViewManager.GetObjects(query, accesspointGuid, GetFoldersWithAccess(), includeAccessPoints, includeMetadata, includeFiles, includeObjectRelations);

            var page = new PagedResult<Data.Dto.v5.Object>(result.FoundCount,
                                                           result.StartIndex,
                                                           result.Results.Select(item => Data.Dto.v5.Object.Create(((ObjectViewData) item).Object)));

            return page;
        }

        public Data.Dto.v5.Object Create(UUID GUID, uint objectTypeID, uint folderID)
        {
            var userId = Request.User.Guid;
            var groupIds = Request.Groups.Select(group => group.Guid);

            var result = ObjectCreate.Create(GUID.ToGuid(), objectTypeID, folderID, userId, groupIds);

            return Data.Dto.v5.Object.Create(result);
        }

        #endregion
    }
}