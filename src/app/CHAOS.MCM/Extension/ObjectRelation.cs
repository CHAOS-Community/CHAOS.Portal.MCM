namespace Chaos.Mcm.Extension
{
    using System;
    using System.Xml.Linq;

    using Chaos.Portal;
    using Chaos.Portal.Data.Dto.Standard;

    public class ObjectRelation : AMcmExtension
    {
        // todo: implement permission on Set
        public ScalarResult Set( ICallContext callContext, Guid object1Guid, Guid object2Guid, uint objectRelationTypeID, int? sequence, Guid? metadataGuid, Guid? metadataSchemaGuid, string languageCode, XDocument metadataXml )
        {
            uint result;
            var hasMetadata = metadataSchemaGuid.HasValue && !string.IsNullOrEmpty( languageCode ) && metadataXml != null;

            if(hasMetadata)
                result = McmRepository.ObjectRelationSet(object1Guid, object2Guid, objectRelationTypeID, sequence, metadataGuid ?? Guid.NewGuid(), metadataSchemaGuid.Value, languageCode, metadataXml, callContext.User.Guid);
            else
                result = McmRepository.ObjectRelationSet(object1Guid, object2Guid, objectRelationTypeID, sequence);

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