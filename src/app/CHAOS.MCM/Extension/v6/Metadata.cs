using System;
using System.Linq;
using System.Xml.Linq;
using Chaos.Mcm.Data;
using Chaos.Mcm.Permission;
using Chaos.Portal.Core;
using Chaos.Portal.Core.Data.Model;
using Chaos.Portal.Core.Exceptions;

namespace Chaos.Mcm.Extension.v6
{
  public class Metadata : AMcmExtension
  {
    #region Initialization

    public Metadata(IPortalApplication portalApplication, IMcmRepository mcmRepository,
                    IPermissionManager permissionManager) : base(portalApplication, mcmRepository, permissionManager)
    {
    }

    #endregion

    public ScalarResult Set(Guid objectGuid, Guid metadataSchemaGuid, string languageCode, uint revisionID,
                            XDocument metadataXml)
    {
      if (!HasPermissionToObject(objectGuid, FolderPermission.CreateUpdateObjects))
        throw new InsufficientPermissionsException(
          "User does not have permissions to set metadata on object or the object doesn't exist");

      var metadataGuid = Guid.NewGuid();
      var userGuid = Request.User.Guid;

      var objects = McmRepository.ObjectGet(objectGuid, true, true, true, true, true);

      var firstOrDefault = objects.Metadatas.FirstOrDefault(m => m.MetadataSchemaGuid == metadataSchemaGuid && m.LanguageCode == languageCode);
      if (firstOrDefault != null)
        metadataGuid = firstOrDefault.Guid;
      else
      {
        objects.Metadatas.Add(new Data.Dto.Metadata
          {
            Guid = metadataGuid,
            MetadataSchemaGuid = metadataSchemaGuid,
            LanguageCode = languageCode,
            RevisionID = revisionID,
            MetadataXml = metadataXml,
            DateCreated = DateTime.UtcNow
          });
      }

      var result = McmRepository.MetadataSet(objectGuid, metadataGuid, metadataSchemaGuid, languageCode, revisionID,
                                             metadataXml, userGuid);

      objects = McmRepository.ObjectGet(objectGuid, true, true, true, true, true);

      ViewManager.Index(objects);

      return new ScalarResult((int) result);
    }
  }
}