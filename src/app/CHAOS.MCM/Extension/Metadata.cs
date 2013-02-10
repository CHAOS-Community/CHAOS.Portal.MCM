namespace Chaos.Mcm.Extension
{
    using System;
    using System.Xml.Linq;

    using Chaos.Portal;
    using Chaos.Portal.Data.Dto.Standard;
    using Chaos.Portal.Extension;

    [PortalExtension(configurationName: "MCM")]
    public class Metadata : AMcmExtension
    {
        public ScalarResult Metadata_Set(ICallContext callContext, Guid objectGuid, Guid metadataSchemaGuid, string languageCode, uint revisionID, XDocument metadataXml)
        {
            var metadataGuid = Guid.NewGuid();
            var userGuid     = callContext.User.Guid;

            var result  = McmRepository.MetadataSet(objectGuid, metadataGuid, metadataSchemaGuid, languageCode, revisionID, metadataXml, userGuid);
            var objects = McmRepository.ObjectGet(objectGuid, true, false, false, true, true);
            
            //todo: replace with view indexing
//            if(callContext.IndexManager != null)
//                PutObjectInIndex(callContext.IndexManager.GetIndex<Mcm>(), objects);

            return new ScalarResult((int)result);
        }
    }
}