using System;
using System.Xml.Linq;
using Chaos.Mcm.Data;
using Chaos.Mcm.Data.Configuration;
using Chaos.Mcm.Permission;
using Chaos.Portal.Core;
using Chaos.Portal.Core.Data.Model;

namespace Chaos.Mcm.Extension
{
	public class UserProfile : AMcmExtensionWithConfiguration<UserProfileConfiguration>
	{
		public UserProfile(IPortalApplication portalApplication) : base(portalApplication)
		{
		}

		public UserProfile(IPortalApplication portalApplication, IMcmRepository mcmRepository, IPermissionManager permissionManager) : base(portalApplication, mcmRepository, permissionManager)
		{
		}

		#region Get

		public Data.Dto.UserProfile Get(Guid metadataSchemaGuid, Guid? userGuid = null)
		{
			throw new NotImplementedException();
		}

		#endregion
		#region Get

		public ScalarResult Set(XDocument metadata, Guid metadataSchemaGuid, Guid? userGuid = null)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}