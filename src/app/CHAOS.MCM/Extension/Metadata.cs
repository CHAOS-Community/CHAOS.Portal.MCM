namespace Chaos.Mcm.Extension
{
    using System;
    using System.Linq;
    using System.Xml.Linq;

    using CHAOS;
    using CHAOS.Extensions;

    using Chaos.Mcm.Data.EF;
    using Chaos.Mcm.Exception;
    using Chaos.Mcm.Permission;
    using Chaos.Portal;
    using Chaos.Portal.Data.Dto.Standard;
    using Chaos.Portal.Exceptions;
    using Chaos.Portal.Extension;

    [PortalExtension(configurationName: "MCM")]
    public class Metadata : AMcmExtension
    {
//        public ScalarResult Metadata_Set(ICallContext callContext, UUID objectGUID, UUID metadataSchemaGUID, string languageCode, uint? revisionID, string metadataXML)
//        {
//            // TODO: replace with proper XML validation, Quick ugly fix, to make sure it's valid XML
//            XDocument.Parse(metadataXML);
//
//            using (var db = DefaultMCMEntities)
//            {
//                if (!HasPermissionToObject(callContext, objectGUID, FolderPermission.CreateUpdateObjects))
//                    throw new InsufficientPermissionsException("User does not have permissions to set metadata on this object");
//
//                var result = db.Metadata_Set(new UUID().ToByteArray(), objectGUID.ToByteArray(), metadataSchemaGUID.ToByteArray(), languageCode, (int?)revisionID, metadataXML, callContext.User.GUID.ToByteArray()).FirstOrDefault();
//
//                if (!result.HasValue || result.Value == -200)
//                    throw new UnhandledException("NewMetadata set failed on the database and was rolled back");
//
//                if (result.Value == -300)
//                    throw new InvalidRevisionException("RevisionID is too old, set metadata with the latest revisionID.");
//
//                if (result.Value == -350)
//                    throw new InvalidRevisionException("RevisionID can only be null if there is no metadata already on the object");
//
//                var objects = db.Object_Get(objectGUID, true, false, false, true, true).ToDto().ToList();
//
//                PutObjectInIndex(callContext.IndexManager.GetIndex<Mcm>(), objects);
//
//                return new ScalarResult(result.Value);
//            }
//        }

        //todo: make a Parameterbinding for XDocument
        public ScalarResult Metadata_Set(ICallContext callContext, Guid objectGuid, Guid metadataSchemaGuid, string languageCode, uint revisionID, XDocument metadataXml)
        {
            var metadataGuid = Guid.NewGuid();
            var userGuid     = callContext.User.GUID.ToGuid();

            var result = McmRepository.MetadataSet(objectGuid, metadataGuid, metadataSchemaGuid, languageCode, revisionID, metadataXml, userGuid);

            return new ScalarResult((int)result);
        }
    }
}