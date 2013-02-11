namespace Chaos.Mcm.Extension
{
    using System.Collections.Generic;

    using Chaos.Portal;
    using Chaos.Portal.Data.Dto.Standard;
    using Chaos.Portal.Exceptions;

    public class ObjectType : AMcmExtension
    {
        #region Business Logic

        public Data.Dto.ObjectType Set(ICallContext callContext, string name)
		{
            if( !callContext.User.SystemPermissonsEnum.HasFlag( SystemPermissons.Manage ) )
                throw new InsufficientPermissionsException( "User does not have permission to create an Object Type" );

            var id     = McmRepository.ObjectTypeSet(name);
            var result = McmRepository.ObjectTypeGet(id, null);
            
            if(result.Count == 0) 
                throw new UnhandledException("ObjectType was created but couldn't be retrieved, try to Call Get specifically");

            return result[0];
		}

		public IEnumerable<Data.Dto.ObjectType> Get( ICallContext callContext )
		{
		    return McmRepository.ObjectTypeGet(null, null);
		}

		public ScalarResult Delete( ICallContext callContext, uint id )
		{
            if( !callContext.User.SystemPermissonsEnum.HasFlag( SystemPermissons.Manage ) )
                throw new InsufficientPermissionsException( "User does not have permission to create an Object Type" );

		    var result = McmRepository.ObjectTypeDelete(id);

            return new ScalarResult((int)result);
		}

		#endregion
    }

}
