﻿using System;
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

        public void Set( CallContext callContext, Guid objectGUID, int metadataSchemaID, string languageCode, string metadataXML )
        {
            
        }

        #endregion
    }
}
