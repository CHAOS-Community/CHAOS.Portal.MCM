using System;
using System.Web.Mvc;
using Geckon.Portal.Core.Standard.Extension;

namespace Geckon.MCM.Extension.Metadata
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
        public void Set( CallContext callContext, UUID objectGUID, int metadataSchemaID, string languageCode, uint? revisionID, string metadataXML )
        {
            
        }

        #endregion
    }
}
