namespace Chaos.Mcm.Extension
{
    using System;

    using CHAOS;

    using Chaos.Mcm.Data.Dto;
    using Chaos.Portal;
    using Chaos.Portal.Data.Dto.Standard;

    public class ObjectRelation : AMcmExtension
    {
        public ScalarResult Set(ICallContext callContext, Guid object1GUID, Guid object2GUID, Data.Dto.Metadata metadata, uint objectRelationTypeID, int? sequence)
        {
            uint result;

            if (metadata.Guid.ToString() != UUID.Empty.ToString())
            {
                var objectRelationInfo = new ObjectRelationInfo
                    {
                        Object1Guid          = object1GUID,
                        Object2Guid          = object2GUID,
                        ObjectRelationTypeID = objectRelationTypeID,
                        Sequence             = sequence,
                        MetadataGuid         = metadata.Guid,
                        MetadataSchemaGuid   = metadata.MetadataSchemaGuid,
                        MetadataXml          = metadata.MetadataXml,
                        LanguageCode         = metadata.LanguageCode
                    };

                result = this.McmRepository.ObjectRelationSet(objectRelationInfo, callContext.User.Guid);
            }
            else
                result = McmRepository.ObjectRelationSet(object1GUID, object2GUID, objectRelationTypeID, sequence);

            return new ScalarResult((int)result);
        }

        // todo: implement permision on Delete
        public ScalarResult Delete(ICallContext callContext, Guid object1Guid, Guid object2Guid, uint objectRelationTypeID)
        {
            var result = McmRepository.ObjectRelationDelete(object1Guid, object2Guid, objectRelationTypeID);

            return new ScalarResult((int)result);
        } 
    }
}