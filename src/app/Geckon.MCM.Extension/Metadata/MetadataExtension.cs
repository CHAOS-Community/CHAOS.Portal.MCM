using System.Web.Mvc;
using Geckon;
using Geckon.Portal.Core.Standard.Extension;

namespace CHAOS.MCM.Extension.Metadata
{
    public class MetadataExtension : AExtension
    {
        //#region GET

        //public void Get( CallContext callContext, string objectGUID, string metadataSchemaGUID, int languageID )
        //{
            
        //}

        //#endregion
        #region SET

        [ValidateInput(false)]
        [HttpPost]
        public void Set( CallContext callContext, UUID objectGUID, UUID metadataSchemaGUID, string languageCode, uint? revisionID, string metadataXML )
        {
            
        }

        #endregion
    }
}
