namespace Chaos.Mcm.Extension
{
    using System.Linq;
    using System.Xml.Linq;
    using System.Collections.Generic;

    using Chaos.Mcm.Data;
    using Chaos.Mcm.Permission;
    using Chaos.Portal;

    public class Format : AMcmExtension
    {
        #region Initialization

        public Format(IPortalApplication portalApplication, IMcmRepository mcmRepository, IPermissionManager permissionManager) : base(portalApplication, mcmRepository, permissionManager)
        {
        }

        public Format()
        {
        }

        #endregion
        #region Business Logic

        public IEnumerable<Data.Dto.Format> Get(ICallContext callContext, uint? id, string name)
        {
            return McmRepository.FormatGet(id, name);
        }

        public Data.Dto.Format Create(ICallContext callContext, uint? formatCategoryID, string name, XDocument formatXml, string mimeType, string extension)
        {
            var result = McmRepository.FormatCreate(formatCategoryID, name, formatXml, mimeType, extension);

            return Get(callContext, result, null).First();
        }

        #endregion
    }
}
