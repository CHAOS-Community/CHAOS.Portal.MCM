namespace Chaos.Mcm.Extension
{
    using System;
    using System.Linq;

    using Chaos.Portal;
    using Chaos.Portal.Data.Dto.Standard;
    using Chaos.Portal.Exceptions;

    public class ObjectRelation : AMcmExtension
    {
        public ScalarResult Set(ICallContext callContext, Guid object1GUID, Guid object2GUID, Data.Dto.Standard.Metadata metadata, uint objectRelationTypeID, int? sequence)
        {
            var result = McmRepository.ObjectRelationSet(object1GUID, object2GUID, objectRelationTypeID, sequence);

            return new ScalarResult((int)result);
        }

        public ScalarResult Delete(ICallContext callContext, Guid object1GUID, Guid object2GUID, uint objectRelationTypeID)
        {
            using (var db = DefaultMCMEntities)
            {
                int? result = db.ObjectRelation_Delete(object1GUID.ToByteArray(),
                                                        object2GUID.ToByteArray(),
                                                        (int)objectRelationTypeID).First();

                if (!result.HasValue)
                    throw new UnhandledException("ObjectRelation Delete failed on the database");

                if (result == -100)
                    throw new InsufficientPermissionsException("The user do not have permission to delete object relations");

                return new ScalarResult(result.Value);
            }
        } 
    }
}