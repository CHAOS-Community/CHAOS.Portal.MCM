namespace Chaos.Mcm.Extension
{
    using System.Collections.Generic;

    using Chaos.Mcm.Data;
    using Chaos.Mcm.Permission;
    using Chaos.Portal.Core;
    using Chaos.Portal.Core.Data.Model;
    using Chaos.Portal.Core.Exceptions;

    public class ObjectType : AMcmExtension
    {
        #region Initialization

        public ObjectType(IPortalApplication portalApplication, IMcmRepository mcmRepository, IPermissionManager permissionManager) : base(portalApplication, mcmRepository, permissionManager)
        {
        }

        public ObjectType(IPortalApplication portalApplication): base(portalApplication)
        {
        }

        #endregion
        #region Business Logic

        public Data.Dto.ObjectType Set(uint id, string name)
		{
            if( !Request.User.SystemPermissonsEnum.HasFlag( SystemPermissons.Manage ) ) throw new InsufficientPermissionsException( "User does not have permission to create an Object Type" );

            McmRepository.ObjectTypeSet(name, id: id);

            var result = McmRepository.ObjectTypeGet(id, null);
            
            if(result.Count == 0) throw new UnhandledException("ObjectType was created but couldn't be retrieved, try to Call Get specifically");

            return result[0];
		}

		public IEnumerable<Data.Dto.ObjectType> Get()
		{
		    return McmRepository.ObjectTypeGet(null, null);
		}

		public ScalarResult Delete(uint id )
		{
            if (!Request.User.SystemPermissonsEnum.HasFlag(SystemPermissons.Manage)) throw new InsufficientPermissionsException("User does not have permission to create an Object Type");

		    var result = McmRepository.ObjectTypeDelete(id);

            return new ScalarResult((int)result);
		}

		#endregion
    }

}
